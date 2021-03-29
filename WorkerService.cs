using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using DocuShareIndexingWorker.Controllers;
using DocuShareIndexingWorker.Entities;
using System.Collections.Generic;

namespace DocuShareIndexingWorker
{
    public class WorkerService : IHostedService, IDisposable
    {

        /**
        * @notice worker local variables.
        */
        private Timer _timer;
        private CultureInfo _cultureInfo = new CultureInfo("en-US");


        /**
        * @dev The function will be starting a bot.
        */
        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            int timespan = 3000;
            try {
                timespan = Convert.ToInt32(ConfigurationManager.AppSettings["interval"]);
            } catch {}

            _timer = new Timer(
                (e) => Start(), 
                null, 
                TimeSpan.Zero, 
                TimeSpan.FromMilliseconds(timespan));
            return Task.CompletedTask;
        }


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

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}