



using System;
using MatchingEngine.Modules.Helper;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace MatchingEngine.Controllers
{

    [Route("api/[controller]")]
    public class BidAskListController : Controller
    {
        [HttpGet("{market}")]
        public string Get(string market,bool withId)
        {
            string result = "";
            using (var conn = RedisHelper.CreateConnection())
            {
                var db = conn.GetDatabase();
                RedisValue[] values = { market};
                RedisKey[] keys = { };
                RedisResult rResult;
                try
                {
                    if (withId)
                    {
                        rResult = db.ScriptEvaluate(LuaScriptHandler.queryTenBidAskId, keys, values);

                    }
                    else
                    {
                        rResult = db.ScriptEvaluate(LuaScriptHandler.queryTenBidAsk, keys, values);

                    }
                    result = (string)rResult;
                    result = "{\"Status\":\"Good\","+ result.Substring(1);
     
                }
                catch (Exception e)
                {
                    result = "{\"Status\":\"Server Busy\"}";
                }
           }
            return result;
        }

    }
}