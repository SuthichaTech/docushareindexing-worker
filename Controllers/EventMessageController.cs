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
    public class EventMessageController
    {
        /**
        * @notice readonly variables.
        */
        private readonly AppConfig _config;

        public EventMessageController(AppConfig config)
        {
            _config = config;
        }


        /**
        * @dev The function will send event message to API.
        * @param eventMessage The event from transaction.
        */
        public void addEvent(EventMessage eventMessage)
        {
            try
            {
                // 1. Create HttpWebRequest
                string url = string.Format("{0}/api/eventmessage/add", _config.ApiHost);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";

                // 2. Add token key to header.
                request.Headers["Authorization"] = string.Format("Bearer {0}", _config.ApiToken);


                // 3. Send Json to request body.
                var jsonEventMessage = JsonConvert.SerializeObject(eventMessage);
                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(jsonEventMessage);
                    sw.Flush();
                    sw.Close();
                }


                // 4. Get response message.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    
            } catch (Exception ex)
            {
                Logger.Error("addEvent : " + ex.Message);
            }
        }
    }
}