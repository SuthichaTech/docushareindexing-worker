using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using DocuShareIndexingWorker.Entities;
using DocuShareIndexingWorker.Utils;
using Newtonsoft.Json;

namespace DocuShareIndexingWorker.Controllers
{
    public class DeclarationMessageController
    {

        private readonly AppConfig _config;

        public DeclarationMessageController(AppConfig config)
        {
            _config = config;

        }
        

        /**
        * @dev Return Export Declaration Message Data Indexing.
        * @param refno The reference number to find object.
        */
        public List<DeclarationMessageResponse> getExportShipment(string refno)
        {
            string retJSONString = string.Empty;
            try
            {
                // 1. Create HttpWebRequest
                string url = string.Format("{0}/api/declarationmessage/export/{1}", _config.ApiHost, refno);
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
                // Write error log.
                Logger.Info("getExportShipment : " + ex.Message);
            }

            return JsonConvert.DeserializeObject<List<DeclarationMessageResponse>>(retJSONString);
        }


        /**
        * @dev Return Import Declaration Message Data Indexing.
        * @param refno The reference number to find object.
        */
        public List<DeclarationMessageResponse> getImportShipment(string refno)
        {
            string retJSONString = string.Empty;
            try
            {
                // 1. Create HttpWebRequest
                string url = string.Format("{0}/api/declarationmessage/import/{1}", _config.ApiHost, refno);
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
                // Write error log.
                Logger.Info("getImportShipment : " + ex.Message);
            }

            return JsonConvert.DeserializeObject<List<DeclarationMessageResponse>>(retJSONString);
        }

    }

}