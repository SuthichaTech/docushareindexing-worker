using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DocuShareIndexingWorker.Entities;
using DocuShareIndexingWorker.Utils;
using Newtonsoft.Json;

namespace DocuShareIndexingWorker.Controllers
{
    public class JobController
    {

        /**
        * @notice readonly variable.
        */
        private readonly AppConfig _config;
        public JobController(AppConfig config)
        {
            _config = config;
        }

        public List<Job> getJobs()
        {
            // NOTE: varialbe for get response json string.
            string retJSONString = string.Empty;

            try
            {

                // 1. Create HttpWebRequest
                string url = string.Format("{0}/api/workerjob", _config.ApiHost);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "GET";

                // 2. Add token key to header.
                request.Headers["Authorization"] = string.Format("Bearer {0}", _config.ApiToken);

                // 3. Instand response object.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // 4. Read stream data to string object.
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    retJSONString = sr.ReadToEnd();
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            // FINAL : Return jobs object.
            return JsonConvert.DeserializeObject<List<Job>>(retJSONString);
        }

    }
}