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

        /**
        * @dev Worker service will be start this process.
        */
        public void Start()
        {
            // 1. Read configuration data to variables object.
            AppConfigController appConfigCtl = new AppConfigController();
            AppConfig appConfig = appConfigCtl.getAppConfig();

            // 2. Init the controllers.
            JobController jobCtl = new JobController(appConfig);
            WorkerController workerCtl = new WorkerController(appConfig);

            // 3. Get job list from API.
            List<Job> jobs = jobCtl.getJobs();
            jobs.ForEach(job => jobCtl.setJobEnvaronment(job));

            // 4. Command worker controller to progress job.
            for (var i = 0; i < jobs.Count; i++)
            {
                workerCtl.DoWork(jobs[i]);
            }
            

        }
    }
}