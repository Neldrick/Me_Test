using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
using Jil;
using MatchingEngine.Modules.DataType;
using MatchingEngine.Modules.Helper;

namespace MatchingEngine.Controllers
{
    
    [Route("api/[controller]")]
    public class MarketController : Controller
    {
        private IConfiguration _config ;
        public MarketController(IConfiguration Configuration)
        {
            _config  = Configuration;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "hi", "There" };
        }
        [HttpGet ("{type}")]
        public JsonResult Get(string type,bool detail)
        {
            //type :all,first,last, 
            //detail: true , false
            List<Market> markets = SettingHelper.markets;
            List<string> resultList = new List<string>();
            switch(type){
                case "all":
                    if(detail){
                        return new JsonResult(markets);
                    }
                    else{
                         foreach (Market m in markets)
                        {
                            resultList.Add(m.name);
                        }
                        return  new JsonResult(resultList);
                    }                
                case "first":
                    if(detail){
                        return new JsonResult(markets.First());
                    }
                    else{
                        return new JsonResult(markets[0].name);
                    }
                case "last":
                    if(detail){
                        return new JsonResult(markets.Last());
                    }
                    else{
                        return new JsonResult(markets.Last().name);
                    }
                default:
                    break;
            }
           
           
            return new JsonResult(resultList);
        }
       

    }
}