using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
using Jil;
using MatchingEngine.Modules.DataType;

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
            string test = _config["Test"];
            Market[] marketsArray = _config.GetSection("Markets").Get<Market[]>();
           
            List<string> resultList = new List<string>();
            foreach (Market m in marketsArray)
            {
                resultList.Add(m.name);
            }
            return new JsonResult(resultList);
        }

    }
}