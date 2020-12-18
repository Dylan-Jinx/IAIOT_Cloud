using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Final_project_IAIOTCloud.Utility
{
    public class RedisHelper : IDisposable
    {
        //连接字符串
        private string _connectionString;
        //实例名称
        private string _instanceName;
        //默认数据库
        private int _defaultDB;
        private ConcurrentDictionary<string, ConnectionMultiplexer> _connections;
        public RedisHelper(string connectionString, string instanceName, int defaultDB = 0)
        {
            _connectionString = connectionString;
            _instanceName = instanceName;
            _defaultDB = defaultDB;
            _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }
        /// <summary>
        /// 获取ConnectionMultiplexer
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetConnect()
        {
            return _connections.GetOrAdd(_instanceName, p => ConnectionMultiplexer.Connect(_connectionString));
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="db">默认为0：优先代码的db配置，其次config中的配置</param>
        /// <returns></returns>
        public IDatabase GetDatabase()
        {
            return GetConnect().GetDatabase(_defaultDB);
        }

        public IServer GetServer(string configName = null, int endPointsIndex = 0)
        {
            var confOption = ConfigurationOptions.Parse(_connectionString);
            return GetConnect().GetServer(confOption.EndPoints[endPointsIndex]);
        }

        public ISubscriber GetSubscriber(string configName = null)
        {
            return GetConnect().GetSubscriber();
        }

        #region Redis发布订阅

        public delegate void RedisDeletegate(string str);
        public event RedisDeletegate RedisSubMessageEvent;

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="subChannel"></param>
        public void RedisSub(string subChannel)
        {

            GetConnect().GetSubscriber().Subscribe(subChannel, (channel, message) => {
                RedisSubMessageEvent?.Invoke(message); //触发事件

            });

        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public long RedisPub(string channel, string msg)
        {
            return GetConnect().GetSubscriber().Publish(channel, msg);
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="channel"></param>
        public void Unsubscribe(string channel)
        {
            GetConnect().GetSubscriber().Unsubscribe(channel);
        }

        /// <summary>
        /// 取消全部订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            GetConnect().GetSubscriber().UnsubscribeAll();
        }

        #endregion


        public void Dispose()
        {
            if (_connections != null && _connections.Count > 0)
            {
                foreach (var item in _connections.Values)
                {
                    item.Close();
                }
            }
        }
    }
}
