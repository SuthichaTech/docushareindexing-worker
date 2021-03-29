using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using DocuShareIndexingWorker.Utils;
using DocuShareIndexingWorker.Controllers;
using DocuShareIndexingWorker.Entities;
using Newtonsoft.Json;


namespace DocuShareIndexingWorker
{
    public class WorkerService
    {
        public void Start()
        {
            AppConfigController appConfigCtl = new AppConfigController();
            AppConfig appConfig = appConfigCtl.getAppConfig();

            JobController jobCtl = new JobController(appConfig);

            List<Job> jobs = jobCtl.getJobs();
            jobs.ForEach(job => jobCtl.setJobEnvaronment(job));

            for (var i = 0; i < jobs.Count; i++)
            {
                
            }
            

        }
    }
}