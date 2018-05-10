using System;
using StackExchange.Redis;

namespace MatchingEngine.Modules.Helper
{
    public  class RedisHelper
    {
        public static string hostAndPort { get; set; }
        public static ConnectionMultiplexer CreateConnection()
        {
            return  ConnectionMultiplexer.Connect(SettingHelper.redisHostAndPort);
            
        }
    }
}