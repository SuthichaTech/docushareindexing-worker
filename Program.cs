using System;
using System.Data;
using System.Configuration;
using System.IO;
using DocuShareIndexingWorker.Utils;
using DocuShareIndexingWorker.Controllers;
using Newtonsoft.Json;

namespace DocuShareIndexingWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiHost = ConfigurationManager.AppSettings["api_host"];
            var apiKey = ConfigurationManager.AppSettings["api_key"];
            
            // Console.WriteLine(apiHost);
            // Console.WriteLine(apiKey);

            // var ctk = new Controllers.DeclarationMessageController();
            // ctk.testGetExportShipment(apiHost, apiKey,"A0051640109410");

            var xmlPath = Path.Combine(Logger.getAssemblyDirectory(), "AppData", "app.xml");
            var queueConfigCtk = new QueueConfigController(xmlPath);
            var configs = queueConfigCtk.getConfigs();

            for(var i = 0; i < configs.Count; i++)
            {
                Console.WriteLine(JsonConvert.SerializeObject(configs[i]));
            }

        }
    }
}
