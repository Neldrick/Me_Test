using StackExchange.Redis;
using System;
namespace MatchingEngine.Modules.Helper
{
    public class LuaScriptHandler
    {

        public static string queryTenBidAsk { get; set; }
        public static string queryTenBidAskId { get; set; }
        public static string cancelOrder { get; set; }
        public static string placeBuyLimitedOrder { get; set; }
        public static string placeSellLimitedOrder { get; set; }
        public static string placeBuyMarketOrder { get; set; }
        public static string placeSellMarketOrder { get; set; }

        public static void GetAllScriptSHA()
        {
            ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("127.0.0.1:6379");//$"{host}:{port}"
            IDatabase db = conn.GetDatabase();

            const string Script = "local result={} " +
                                  " result[1]=redis.call('get', 'QueryTenBidAsk') " +
                                  " result[2]=redis.call('get', 'QueryTenBidAskId') " +
                                  " result[3]=redis.call('get', 'CancelOrder') " +
                                  " result[4]=redis.call('get', 'PlaceBuyLimitedOrder') " +
                                  " result[5]=redis.call('get', 'PlaceSellLimitedOrder') " +
                                  " result[6]=redis.call('get', 'PlaceBuyMarketOrder') " +
                                  " result[7]=redis.call('get', 'PlaceSellMarketOrder') "+
                                  " return result";

            var prepared = LuaScript.Prepare(Script);
            RedisResult result = db.ScriptEvaluate(prepared);            
            string [] address = (string[])result;
            queryTenBidAsk = address[0];
            queryTenBidAskId = address[1];
            cancelOrder = address[2];
            placeBuyLimitedOrder = address[3];
            placeSellLimitedOrder = address[4];
            placeBuyMarketOrder = address[5];
            placeSellMarketOrder = address[6];
        }

    }
}