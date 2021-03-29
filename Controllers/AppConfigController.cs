using System;
using System.Configuration;
using DocuShareIndexingWorker.Entities;

namespace DocuShareIndexingWorker.Controllers
{
    public class AppConfigController
    {
        /**
        * @dev Return configuration settings
        */
        public AppConfig getAppConfig() {
            return new AppConfig 
            {
                ApiHost = ConfigurationManager.AppSettings["api_host"].ToString(),
                ApiToken = ConfigurationManager.AppSettings["api_key"].ToString(),
                Interval = Convert.ToInt32(ConfigurationManager.AppSettings["interval"])
            };
        }
    }
}