using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.DataListeningService
{
    public class DataCenterCore
    {
        private int _port { get; set; }
        private int _listenNum { get; set; }
        IOCPServer server;
        public DataCenterCore(int port, int listenNum)
        {
            _port = port;
            _listenNum = listenNum;
            server = new IOCPServer(_port, _listenNum);
        }
        /// <summary>
        /// 数据侦听核心启动
        /// </summary>
        public void CoreOpen()
        {
            if (server != null)
            {
                server.Start();
                Console.WriteLine("设备感知与数据侦听核心已启动");
            }
            else
            {
                Console.WriteLine("正在尝试启动设备感知与数据侦听核心");
                server = new IOCPServer(_port, _listenNum);
                server.Start();
                Console.WriteLine("设备感知与数据侦听核心已启动");
            }
        }
        /// <summary>
        /// 数据侦听核心关闭
        /// </summary>
        public void CoreClose()
        {
            if (server != null)
            {
                server.Stop();
                Console.WriteLine("设备感知与数据侦听核心已关闭");
            }
        }
    }
}
