//for placing order ,get order , cancel order 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchingEngine.Modules.DataType;
using MatchingEngine.Modules.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace MatchingEngine.Controllers
{

    [Route("api/[controller]")]
    public class OrderController : Controller
    {


        [HttpGet("{id}")]
        public CryptoOrder Get(string id)
        {
            CryptoOrder result = new CryptoOrder();
            using (var conn = RedisHelper.CreateConnection())
            {
                var db = conn.GetDatabase();
                HashEntry[] order = db.HashGetAll(id);
                result.orderId = long.Parse(id);
                foreach (HashEntry item in order)
                {
                    switch (item.Name)
                    {
                        case "User_Id":
                            result.userId = int.Parse(item.Value);
                            break;
                        case "BuySell":
                            result.isBid = item.Value == 0;
                            break;
                        case "Market":
                            result.market = item.Value;
                            break;
                        case "Price":
                            result.price = decimal.Parse(item.Value);
                            break;
                        case "Amount":
                            result.amount = decimal.Parse(item.Value);
                            break;
                        case "AmountLastLeft":
                            result.amountLastTimeRemain = decimal.Parse(item.Value);
                            break;
                        case "AmountLeft":
                            result.amountRemain = decimal.Parse(item.Value);
                            break;

                    }
                }
            }


            return result;
        }
        [HttpPost]
        public  OrderResult Post([FromBody] CryptoOrder order)
        {
            //security check before posting
            OrderResult result = new OrderResult();
            string buySell = order.isBid ? "0" : "1";
            bool allPass = true;
            order.orderId = DateTime.Now.Ticks/10;//microsecond
            using (var conn = RedisHelper.CreateConnection())
            {
                var db = conn.GetDatabase();
                try
                {
                    if (order.type == 0)
                    { //0=="limited"

                        RedisValue[] values = { buySell,
                    order.userId.ToString(),order.market,order.price.ToString(),
                    order.amount.ToString(),order.orderId.ToString() };
                        RedisKey[] keys = { };
                        RedisResult rResult;

                        if (order.isBid)
                        {
                            rResult = db.ScriptEvaluate(LuaScriptHandler.placeBuyLimitedOrder, keys, values);
                        }
                        else
                        {
                            rResult = db.ScriptEvaluate(LuaScriptHandler.placeBuyLimitedOrder, keys, values);

                        }
                        string redisJson = (string)rResult;
                        result.Status = "Good";
                        result.message = redisJson;
                        //convert it to json object and parse it 
                    }
                    else
                    {
                        RedisValue[] values = { order.market,order.amount.ToString(),order.orderId.ToString() };
                        RedisKey[] keys = { };
                        RedisResult rResult;
                        if (order.isBid)
                        {
                            rResult = db.ScriptEvaluate(LuaScriptHandler.placeBuyMarketOrder, keys, values);

                        }
                        else
                        {
                            rResult = db.ScriptEvaluate(LuaScriptHandler.placeSellMarketOrder, keys, values);
                        }
                        string redisJson = (string)rResult;
                        result.Status = "Good";
                        result.message = redisJson;
                    }
                }
                catch (Exception e)
                {
                    result.Status = "Error";
                    result.message = "Server Busy";
                }
            }

            return result;
        }
        [HttpDelete ("{id}")]
        public OrderResult Delete(string id){
            //cancel order
            //security check before posting
            OrderResult result = new OrderResult();
             using (var conn = RedisHelper.CreateConnection())
            {
                var db = conn.GetDatabase();
                try
                {
                    RedisValue[] values = { id};
                    RedisKey[] keys = { };
                    RedisResult rResult;
                    rResult = db.ScriptEvaluate(LuaScriptHandler.cancelOrder, keys, values);
                    string redisJson = (string)rResult;
                    result.Status = "Good";
                        result.message = redisJson;

                }
                catch(Exception){
                     result.Status = "Error";
                    result.message = "Server Busy";
                }
            }
            return result; 
        }
    }
}