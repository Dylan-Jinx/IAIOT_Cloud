using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IAIOT_alpha_0_1_1.Models;
using IAIOT_alpha_0_1_1.Models.DTO;

namespace IAIOT_alpha_0_1_1.DataListeningService
{
    //IOCP 服务器
    public class IOCPServer : IDisposable
    {
        private const int OPSTOPREALLOC = 2;
        private IAIOTCloudContext context = new IAIOTCloudContext();
        private int _maxClient;
        private Socket _serverSocket;
        private int _clientCount;
        private int _bufferSize = 1024;
        /// <summary>
        /// 信号量
        /// </summary>
        Semaphore _maxAcceptedClients;
        /// <summary>
        /// 缓冲区管理
        /// </summary>
        BufferManager _bufferManager;
        /// <summary>
        /// 对象池
        /// </summary>
        SocketAsyncEventArgsPool _objectPool;

        private bool disposed = false;
        /// <summary>
        /// 服务器是否在运行
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// 监听的IP地址
        /// </summary>
        public IPAddress Address { get; private set; }
        /// <summary>
        /// 监听的端口
        /// </summary>
        public int Port { get; private set; }

        public Encoding Encoding { get; private set; }
        /// <summary>
        /// 异步IOCP SOCKET服务器
        /// </summary>
        /// <param name="localEP"></param>
        /// <param name="maxClient"></param>
        public IOCPServer(int listenPort, int maxClient)
            : this(IPAddress.Any, listenPort, maxClient)
        {

        }
        /// <summary>
        /// 异步Socket TCP服务器
        /// </summary>
        /// <param name="localEP"></param>
        /// <param name="maxClient"></param>
        public IOCPServer(IPEndPoint localEP, int maxClient)
            : this(localEP.Address, localEP.Port, maxClient)
        {

        }

        public IOCPServer(IPAddress localIPAddress, int listenPort, int maxClient)
        {
            this.Address = localIPAddress;
            this.Port = listenPort;
            this.Encoding = Encoding.Default;

            _maxClient = maxClient;
            _serverSocket = new Socket(localIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _bufferManager = new BufferManager(_bufferSize * _maxClient * OPSTOPREALLOC, _bufferSize);
            _objectPool = new SocketAsyncEventArgsPool(_maxClient);
            _maxAcceptedClients = new Semaphore(_maxClient, _maxClient);
        }

        public void Init()
        {
            //Allocates one large byte buffer which all I/O opertions use a piece of, This guards
            //against memory fragementation
            _bufferManager.InitBuffer();

            //pre-allocate pool of SocketAsyncEventArgs objects
            SocketAsyncEventArgs readWriteEventArgs;
            for (int i = 0; i < _maxClient; i++)
            {
                //pre-allocate a set of reusable SocketAsyncEventArgs
                readWriteEventArgs = new SocketAsyncEventArgs();
                readWriteEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                readWriteEventArgs.UserToken = null;

                //assign a byte buffer from the buffer pool to the SocketAsyncEventArgs object
                _bufferManager.SetBuffer(readWriteEventArgs);
                //add SocketAsyncEventArgs to the pool
                _objectPool.Push(readWriteEventArgs);
            }
        }

        /// <summary>
        /// Start IOCP Server
        /// </summary>
        public void Start()
        {
            if (!IsRunning)
            {
                Init();
                IsRunning = true;
                IPEndPoint localEndPoint = new IPEndPoint(Address, Port);
                //create socket listen
                _serverSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                if (localEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    _serverSocket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
                    _serverSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, localEndPoint.Port));
                }
                else
                {
                    _serverSocket.Bind(localEndPoint);
                }
                //start listening
                _serverSocket.Listen(this._maxClient);
                //post an accept request on the listening Socket.
                StartAccept(null);
            }
        }
        /// <summary>
        /// stop the service
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _serverSocket.Close();
                //TODO:close all client connection
            }
        }

        private void StartAccept(SocketAsyncEventArgs asyncEventArgs)
        {
            if (asyncEventArgs is null)
            {
                asyncEventArgs = new SocketAsyncEventArgs();
                asyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            }
            else
            {
                //socket must be cleared since the context object is being reused
                asyncEventArgs.AcceptSocket = null;
            }
            _maxAcceptedClients.WaitOne();
            if (!_serverSocket.AcceptAsync(asyncEventArgs))
            {
                ProcessAccept(asyncEventArgs);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Socket socket = e.AcceptSocket;
                if (socket.Connected)
                {
                    try
                    {
                        Interlocked.Increment(ref _clientCount);//atom opertion add 1
                        SocketAsyncEventArgs asyncEventArgs = _objectPool.Pop();
                        asyncEventArgs.UserToken = socket;

                        Console.WriteLine("客户{0}个连入，共有{1}个连接", socket.RemoteEndPoint.ToString(), _clientCount);

                        if (!socket.ReceiveAsync(asyncEventArgs))
                        {
                            ProcessReceive(asyncEventArgs);
                        }
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine("接收客户 {0} 数据出错, 异常信息： {1} 。", socket.RemoteEndPoint, ex.ToString());
                    }
                    //accept the next post
                    StartAccept(e);
                }
            }
        }
        /// <summary>
        /// Sending data asynchronously
        /// </summary>
        /// <param name="e"></param>
        /// <param name="data"></param>
        public void Send(SocketAsyncEventArgs e, byte[] data)
        {
            if (e.SocketError == SocketError.Success)
            {
                Socket socket = e.AcceptSocket;
                if (socket.Connected)
                {
                    Array.Copy(data, 0, e.Buffer, 0, data.Length);

                    if (!socket.SendAsync(e))
                    {
                        ProcessSend(e);
                    }
                    else
                    {
                        CloseClientSocket(e);
                    }
                }
            }
        }
        /// <summary>
        /// 同步的使用socket发送数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="timeout"></param>
        public void Send(Socket socket, byte[] buffer, int offset, int size, int timeout)
        {
            socket.SendTimeout = 0;
            int startTickCount = Environment.TickCount;
            int sent = 0;//how many bytes is already sent
            do
            {
                if (Environment.TickCount > startTickCount + timeout)
                {

                }
                try
                {
                    sent += socket.Send(buffer, offset + sent, size - sent, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock
                        || ex.SocketErrorCode == SocketError.IOPending
                        || ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        Thread.Sleep(30);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            while (sent < size);
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            Console.WriteLine(String.Format("客户 {0} 断开连接!", ((Socket)e.UserToken).RemoteEndPoint.ToString()));
            Socket socket = e.UserToken as Socket;
            CloseClientSocket(socket, e);
        }

        /// <summary>
        /// 关闭socket连接
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void CloseClientSocket(Socket socket, SocketAsyncEventArgs e)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception)
            {
                // Throw if client has closed, so it is not necessary to catch.
            }
            finally
            {
                socket.Close();
            }
            Interlocked.Decrement(ref _clientCount);
            _maxAcceptedClients.Release();
            _objectPool.Push(e);//SocketAsyncEventArg 对象被释放，压入可重用队列。
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Socket socket = (Socket)e.UserToken;
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                if (e.BytesTransferred > 0)
                {
                    Socket socket = (Socket)e.UserToken;
                    if (socket.Available == 0)
                    {
                        DateTime currentTime = DateTime.Now;
                        byte[] data = new byte[e.BytesTransferred];
                        Array.Copy(e.Buffer, e.Offset, data, 0, data.Length);
                        string info = Encoding.UTF8.GetString(data);
                        //TODO：deal the receive datas

                        if(info.Substring(0,1) == "x")
                        {
                            Console.WriteLine("收到 {0} 模拟数据为 {1}", socket.RemoteEndPoint.ToString(), "项目ID：" + info.Substring(1, 2) + " 温度：" + info.Substring(3, 2) + " 湿度：" + info.Substring(5, 2) + " 噪音：" + info.Substring(7, 2) + " 可燃气体：" + info.Substring(9, 4) + " 光照：" + info.Substring(13, 4));
                            int projId = int.Parse(info.Substring(1, 2));
                            TSensorData gasx = new TSensorData()
                            {
                                CreateDate = currentTime,
                                SensorData = info.Substring(9, 4) + "PPM",
                                SensorUnit = "PPM",
                                SensorName = "可燃气体传感器",
                                DeviceId = 2,
                                SensorId = 7,
                                SensorTag = "data_gas",
                                DeviceName = "模拟数据生成器",
                                DataType = 1,
                                ProjectId = projId
                            };
                            TSensorData tempx = new TSensorData()
                            {
                                CreateDate = currentTime,
                                SensorData = info.Substring(3, 2) + "℃",
                                SensorUnit = "℃",
                                SensorName = "温度传感器",
                                DeviceId = 2,
                                SensorId = 1,
                                SensorTag = "data_temp",
                                DeviceName = "模拟数据生成器",
                                DataType = 1,
                                ProjectId = projId
                            };
                            TSensorData humix = new TSensorData()
                            {
                                CreateDate = currentTime,
                                SensorData = info.Substring(5,2) + "%",
                                SensorUnit = "%",
                                SensorName = "湿度传感器",
                                DeviceId = 2,
                                SensorId = 2,
                                SensorTag = "data_humi",
                                DeviceName = "模拟数据生成器",
                                DataType = 1,
                                ProjectId = projId
                            };
                            TSensorData lxx = new TSensorData()
                            {
                                CreateDate = currentTime,
                                SensorData = new Random().Next(200, 2800) + "lx",
                                SensorUnit = "lx",
                                SensorName = "光照传感器",
                                DeviceId = 2,
                                SensorId = 3,
                                SensorTag = "data_lx",
                                DeviceName = "模拟数据生成器",
                                DataType = 1,
                                ProjectId = projId
                            };
                            TSensorData voicex = new TSensorData()
                            {
                                CreateDate = currentTime,
                                SensorData = info.Substring(7,2) + "db",
                                SensorUnit = "db",
                                SensorName = "噪音传感器",
                                DeviceId = 2,
                                SensorId = 4,
                                SensorTag = "data_voice",
                                DeviceName = "模拟数据生成器",
                                DataType = 1,
                                ProjectId = projId
                            };
                            TSensorData wind_speedx = new TSensorData()
                            {
                                CreateDate = currentTime,
                                SensorData = new Random().Next(0, 60) + "m/s",
                                SensorUnit = "m/s",
                                SensorName = "风速传感器",
                                DeviceId = 2,
                                SensorId = 10,
                                SensorTag = "data_windspeed",
                                DeviceName = "模拟数据生成器",
                                DataType = 1,
                                ProjectId = projId
                            };
                            Task.Run(new Action(() =>
                            {
                                TSensorData[] datas = { tempx, humix, gasx, lxx, wind_speedx, voicex };
                                context.TSensorData.AddRangeAsync(datas);
                                try
                                {
                                    if (context.SaveChangesAsync().Result > 0)
                                    {
                                        Console.WriteLine("data save keep container successful!");
                                    }
                                }
                                catch (Exception)
                                {
                                    //throw;
                                }
                            }));
                        }
                        else
                        {
                            TSensorData gas = new TSensorData()
                            {
                                CreateDate = currentTime,
                                SensorData = info.Substring(0, 4) + "PPM",
                                SensorUnit = "PPM",
                                SensorName = "可燃气体传感器",
                                DeviceId = 2,
                                SensorId = 7,
                                SensorTag = "data_gas",
                                DeviceName = "数据采集网关",
                                DataType = 1,
                                ProjectId = 1
                            };
                            TSensorData temp = new TSensorData()
                            {
                                CreateDate = currentTime,
                                SensorData = info.Substring(4, 2) + "℃",
                                SensorUnit = "℃",
                                SensorName = "温度传感器",
                                DeviceId = 2,
                                SensorId = 1,
                                SensorTag = "data_temp",
                                DeviceName = "数据采集网关",
                                DataType = 1,
                                ProjectId = 1
                            };
                            TSensorData humi = new TSensorData()
                            {
                                CreateDate = currentTime,
                                SensorData = info.Substring(6) + "%",
                                SensorUnit = "%",
                                SensorName = "湿度传感器",
                                DeviceId = 2,
                                SensorId = 2,
                                SensorTag = "data_humi",
                                DeviceName = "数据采集网关",
                                DataType = 1,
                                ProjectId = 1
                            };
                            Task.Run(new Action(() =>
                            {
                                TSensorData[] datas = { temp, humi, gas };
                                context.TSensorData.AddRangeAsync(datas);
                                try
                                {
                                    if (context.SaveChangesAsync().Result > 0)
                                    {
                                        Console.WriteLine("data save keep container successful!");
                                    }
                                }
                                catch (Exception)
                                {
                                    //throw;
                                }
                            }));
                        }
                    }

                    if (!socket.ReceiveAsync(e))
                    {
                        //
                        ProcessReceive(e);
                    }
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        /// <summary>
        /// call back when the Accept operation completes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    ProcessAccept(e);
                    break;
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release 
        /// both managed and unmanaged resources; <c>false</c> 
        /// to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        Stop();
                        if (_serverSocket != null)
                        {
                            _serverSocket = null;
                        }
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine("an exception has occured:", ex.Message);
                        //TODO 事件
                    }
                }
                disposed = true;
            }
        }
    }
}
