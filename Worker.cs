using System;
using System.Data;
using System.Configuration;
using System.IO;
using DocuShareIndexingWorker.Utils;
using DocuShareIndexingWorker.Controllers;
using DocuShareIndexingWorker.Entities;
using Newtonsoft.Json;


namespace DocuShareIndexingWorker
{
    public class Worker
    {
        public void Start()
        {
            AppConfigController appConfigCtl = new AppConfigController();
            AppConfig appConfig = appConfigCtl.getAppConfig();

            JobController jobCtl = new JobController(appConfig);

            var jobs = jobCtl.getJobs();
            Console.WriteLine(JsonConvert.SerializeObject(jobs));



        }
    }
}