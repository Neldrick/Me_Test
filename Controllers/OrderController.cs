//for placing order ,get order , cancel order 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchingEngine.Modules.DataType;
using Microsoft.AspNetCore.Mvc;

namespace MatchingEngine.Controllers
{
    
    [Route("api/[controller]")]
    public class OrderController:Controller{
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Enter Id Please" };
        }

        [HttpGet("{id}")]
        public Order Get(string id)
        {
            Order result = new Order();
            
            return result;
        }
    }
}