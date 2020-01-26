namespace Adventure.Core.Models
{
    public class DocumentDbSettings
    {
        public string Uri { get; set; }

        public string AuthKey { get; set; }

        public string DatabaseId { get; set; }

        public int MaxRetryAttemptsOnThrottledRequests { get; set; }

        public int MaxRetryWaitTimeInSeconds { get; set; }
    }
}
