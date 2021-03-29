namespace DocuShareIndexingWorker.Entities
{
    public class QueueItem
    {
        public string Id { get; set; }
        public string FromPath { get; set; }
        public string ToPath { get; set; }
        public string ShipmentType { get; set; }
    }
}