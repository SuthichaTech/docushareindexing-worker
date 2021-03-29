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

        /**
        * @dev Return the api URL.
        * @param shipmentType The type of shipment.
        * @param host The url link to the service.
        */
        private string createUrl(ShipmentType shipmentType, string host)
        {
            return String.Format("{0}/api/declarationmessage/{1}/", 
                            host, 
                            shipmentType == ShipmentType.EXPORT ? "export" : "import");
        }
        


        /**
        * @dev
        * @param host Refer to url or ip address the service instance.
        * @param token The security key that generate by system.
        * @param refno The reference number to find object.
        */
        public List<DeclarationMessageResponse> getExportShipment(string host, string token, string refno) 
        {
            try 
            {
                // 1. Create HttpWebRequest
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(createUrl(ShipmentType.EXPORT, host) + refno);
                request.ContentType = "application/json";
                request.Method = "GET";

                // 2. Add token key to header.
                request.Headers["Authorization"] = string.Format("Bearer {0}", token);

                // 3. Instand response object.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // 4. Read stream data to string object.
                string retResponse = string.Empty;
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    retResponse = sr.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<List<DeclarationMessageResponse>>(retResponse);

            } catch (Exception ex)
            {
                // Write error log.
                Logger.Info("getExportShipment : " + ex.Message);
            }

            return null;
        }

    }


    /**
    * @notice typeof shipment type.
    */
    public enum ShipmentType 
    {
        EXPORT,
        IMPORT
    }
}