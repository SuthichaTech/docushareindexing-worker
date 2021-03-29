namespace DocuShareIndexingWorker.Entities
{
    public class EventMessage
    {
        public string EventType { get; set; }
        public string EventKey { get; set; }
        public string EventStatus { get; set; }
        public string EventDescription { get; set; }
        public string UserID { get; set; }
    }
}