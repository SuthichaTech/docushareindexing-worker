namespace DocuShareIndexingWorker.Entities
{
    public class Job
    {
        /**
        * @notice properties variables.
        */
        public int trxNo { get; set; }
        public string workerID {get; set; }
        public string fromPath {get; set; }
        public string toPath {get; set; }

    }
}