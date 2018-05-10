


using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using MatchingEngine.Modules.DataType;
using Newtonsoft.Json.Linq;

namespace MatchingEngine.Modules.Helper
{


    public class SettingHelper
    {

        public static string redisHostAndPort { get; set; }
        public static List<Market> markets { get; set; }
        public static string kafkaHostAndPort { get; set; }

        public static void LoadSetting()
        {

            try
            {
                string rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string jsonString = System.IO.File.ReadAllText(rootPath + "/mysetting.json");
                var result = JObject.Parse(jsonString);
                redisHostAndPort = result["Redis"].Value<string>();
                kafkaHostAndPort = result["Kafka"].Value<string>();
                markets = result["Markets"].Value<JArray>().ToObject<List<Market>>();
               
                
                Console.WriteLine("get Setting Success");
            }
            catch (Exception e)
            {
                Console.WriteLine("error");
                throw e;
            }

        }
    }
}