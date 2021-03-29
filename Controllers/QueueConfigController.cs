using System;
using System.Collections.Generic;
using System.Data;
using DocuShareIndexingWorker.Entities;
using DocuShareIndexingWorker.Utils;

namespace DocuShareIndexingWorker.Controllers
{
    public class QueueConfigController
    {
        /**
        * @notice readonly variables.
        */ 
        private readonly string _configPath;
        public QueueConfigController(string configPath)
        {
            _configPath = configPath;
        }

        /**
        * @dev Return queue config from xml data.
        */
        public List<QueueItem> getConfigs()
        {
            // 1. Create DataSet objcet.
            List<QueueItem> configs = new List<QueueItem>();

            try
            {
                // 2. Read xml data to DataSet object.
                DataSet dsConfig = new DataSet();
                dsConfig.ReadXml(_configPath);

                // 3. Convert DataRow to QueueItem object.
                foreach(DataRow row in dsConfig.Tables[0].Rows)
                {
                    configs.Add(new QueueItem {
                        Id = row["id"].ToString(),
                        ShipmentType = row["shipment_type"].ToString(),
                        FromPath = row["from_path"].ToString(),
                        ToPath = row["to_path"].ToString()
                    });
                }

            } catch (Exception ex)
            {
                // Write error log when catch exception.
                Logger.Error("getConfigs : " + ex.Message);
            }

            return configs;
        }
    }
}