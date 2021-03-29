using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using DocuShareIndexingWorker.Utils;
using DocuShareIndexingWorker.Controllers;
using DocuShareIndexingWorker.Entities;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;
using System.Linq;

namespace DocuShareIndexingWorker.Controllers
{
    public class WorkerController
    {

        /**
        * @notice readonly properties.
        */
        private readonly CultureInfo _cultureInfo;
        private readonly string _dateFormat = "yyyy-MM-dd";
        private readonly AppConfig _config;


        /**
        * @notice The DeclarationMessageController.
        */
        private DeclarationMessageController _declarationMessageController;

        public WorkerController(AppConfig config)
        {
            _config = config;
            _cultureInfo = new CultureInfo("en-US");
        }


        /**
        * @dev That function will be progress a job.
        * @param job The job object to progress.
        */
        public void DoWork(Job job)
        {

            // 1. Check file on the source folder.
            var files = getFiles(job.fromPath);
            
            if (files.Count == 0) return;

            // 2. Progress job.
            progressJob(job, files);
            
        }



        /**
        * @dev This function does progress a job immediately.
        */
        private void progressJob(Job job, List<FileInfo> files)
        {

            // 1. Init DeclarationMessageController object.
            _declarationMessageController = new DeclarationMessageController(_config);


            // 2. Looping files and check condition with jobtype to switch process.
            for (int i = 0; i < files.Count; i++)
            {
                FileInfo file = files[i];
                string sourceFileName = file.FullName;
                string refno = file.Name.ToUpper().Replace(".PDF", "");
                string jobType = job.workerID;

                if (jobType.IndexOf("EXP") >= 0)
                {
                    createExportShipmentIndexing(file, job);
                }
                else if (jobType.IndexOf("IMP") >= 0)
                {
                    createImportShipmentIndexing(file, job);
                }
                else {

                    // NOTE : if the refno does not exists on database its will be move to error folder.
                    moveErrorFile(sourceFileName);
                }
            }
        }


        /**
        * @dev Create the data indexing and transfer to AutoUpload process.
        * @param file The FileInfo object.
        * @param job The Job object.
        */
        private void createExportShipmentIndexing(FileInfo file, Job job)
        {
            string sourceFileName = file.FullName;
            string refno = file.Name.ToUpper().Replace(".PDF", "");
            string jobType = job.workerID;
            string fromPath = job.fromPath;
            string toPath = job.toPath;

            List<DeclarationMessageResponse> expRespMessages = _declarationMessageController.getExportShipment(refno);
            if (expRespMessages != null && expRespMessages.Count > 0) 
            {
                string destinationFile = Path.Combine(job.toPath, refno + ".csv");
                bool writeCsvStatus = writeCsv(expRespMessages[0], destinationFile);

                if (writeCsvStatus)
                {
                    Logger.Info(string.Format("{0} Write CSV have success", refno));
                    string destinationFileName = Path.Combine(job.toPath, refno + ".PDF");

                    moveFile(sourceFileName, destinationFileName);
                }
            }
            else // Move file to error folder if cannot find reference.
            {
                Logger.Info(string.Format("{0} Find not found", refno));
                moveErrorFile(sourceFileName);
            }
        }


        /**
        * @dev Create the data indexing and transfer to AutoUpload process.
        * @param file The FileInfo object.
        * @param job The Job object.
        */
        private void createImportShipmentIndexing(FileInfo file, Job job)
        {
            string sourceFileName = file.FullName;
            string refno = file.Name.ToUpper().Replace(".PDF", "");
            string jobType = job.workerID;
            string fromPath = job.fromPath;
            string toPath = job.toPath;

            List<DeclarationMessageResponse> impRespMessages = _declarationMessageController.getImportShipment(refno);
            if (impRespMessages != null && impRespMessages.Count > 0) 
            {
                string destinationFile = Path.Combine(job.toPath, refno + ".csv");
                bool writeCsvStatus = writeCsv(impRespMessages[0], destinationFile);

                if (writeCsvStatus)
                {
                    Logger.Info(string.Format("{0} Write CSV have success", refno));
                    string destinationFileName = Path.Combine(job.toPath, refno + ".PDF");

                    moveFile(sourceFileName, destinationFileName);
                }
            }
            else // Move file to error folder if cannot find reference.
            {
                Logger.Info(string.Format("{0} Find not found", refno));
                moveErrorFile(sourceFileName);
            }
        }


        /**
        * @dev The function will copy file to AutoUpload folder.
        * @param sourceFileName The name of the source file.
        * @param destinationFileName The name of the destination file.
        */
        private void moveFile(string sourceFileName, string destinationFileName)
        {
            try
            {
                // NOTE : Copy file to folder.
                File.Copy(sourceFileName, destinationFileName, true);
                File.Delete(sourceFileName);

                FileInfo file = new FileInfo(destinationFileName);
                Logger.Info(string.Format("{0} Move PDF to AutoUpload", file.Name.ToUpper().Replace(".PDF","")));

            } catch (Exception ex)
            {
                // NOTE : Trace logging.
                Logger.Error("moveFile : " + ex.Message);
            }

        }


        /**
        * @dev The function will move file this its cannot found the indexing data.
        * @param sourceFileName The name of the source file.
        */
        private void moveErrorFile(string sourceFileName)
        {
            try
            {
                // 1. Create error folder path.
                FileInfo fi = new FileInfo(sourceFileName);
                string errorPath = Path.Combine(fi.DirectoryName, "error");
                
                // 2. Check error folder whatever if not already exists.
                if (!Directory.Exists(errorPath))
                    Directory.CreateDirectory(errorPath);

                // 3. Copy file from source folder to destination folder.
                string destinationFileName = Path.Combine(errorPath, fi.Name);
                File.Copy(sourceFileName, "", true);
                File.Delete(sourceFileName);

                Logger.Info(string.Format("{0} Move PDF to Error folder", fi.Name.ToUpper().Replace(".PDF","")));

            } catch (Exception ex)
            {
                // NOTE : Trace logging.
                Logger.Error("moveErrorFile : " + ex.Message);
            }
        }


        /**
        * @dev The function will write CSV file.
        * @param msg The object is a DeclarationMessageResponse.
        * @param destinationFile The destination file reference to write the source.
        */
        private bool writeCsv(DeclarationMessageResponse msg, string destinationFile)
        {
            try
            {
                // NOTE : Prepare csv content from Message.
                StringBuilder sb = new StringBuilder();

                string headerCell = @"META path,title,ShipmentType,BranchCode,RefNo,DecNO,CommercialInvoices,CmpTaxNo,CmpBranch,CmpName,VesselName,VoyNumber,MasterBL,HouseBL,DestCountry,DeptCountry,ETA,ETD,UDateDeclare,UDateRelease,MaterialType,Period";
                sb.AppendLine(headerCell);
                
                buildCsvContent(sb, msg.metaPath + ".PDF");
                buildCsvContent(sb, msg.title);
                buildCsvContent(sb, msg.shipmentType);
                buildCsvContent(sb, msg.branchCode);
                buildCsvContent(sb, msg.refNo);
                buildCsvContent(sb, msg.decNo);
                buildCsvContent(sb, msg.commercialInvoices);
                buildCsvContent(sb, msg.cmpTaxNo);
                buildCsvContent(sb, msg.cmpBranch);
                buildCsvContent(sb, msg.cmpName);
                buildCsvContent(sb, msg.vesselName);
                buildCsvContent(sb, msg.voyNumber);
                buildCsvContent(sb, msg.masterBL);
                buildCsvContent(sb, msg.houseBL);
                buildCsvContent(sb, msg.destCountry);
                buildCsvContent(sb, msg.deptCountry);
                buildCsvContent(sb, msg.eta.ToString(_dateFormat, _cultureInfo));
                buildCsvContent(sb, msg.etd.ToString(_dateFormat, _cultureInfo));
                buildCsvContent(sb, msg.uDateDeclare.ToString(_dateFormat, _cultureInfo));
                buildCsvContent(sb, msg.uDateRelease.ToString(_dateFormat, _cultureInfo));
                buildCsvContent(sb, msg.materialType);
                buildCsvContent(sb, msg.period);


                // NOTE : Write stream data to csv file.
                File.Delete(destinationFile);
                File.WriteAllText(destinationFile, sb.ToString());
                return true;
            }
            catch (Exception ex)
            {
                // NOTE : Trace logging.
                Logger.Error("writeCsv : " + ex.Message);
            }

            // NOTE : Whatever response false if found error.
            return false;
        }

        /**
        * @dev The helper function will combile text with commas (,)
        */
        private void buildCsvContent(StringBuilder sb, string data)
        {
            sb.AppendFormat("{0}{1}", data, ",");
        }


        /**
        * @dev The helper function will return file on the folder.
        * @param path The file path that will be contain pdf files.
        */
        private List<FileInfo> getFiles(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles("*.pdf");

            if (files.Length > _config.MAX_FILE)
            {
                List<FileInfo> limitFiles = new List<FileInfo>();
                for (int i = 0; i < _config.MAX_FILE; i++)
                {
                    limitFiles.Add(files[i]);
                }
                return limitFiles;
            }
            else return files.ToList();
        }
    }
}