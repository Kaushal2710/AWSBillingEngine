namespace AWSBillingEngine2.Parser_model
{
    public class ParsedUsage
    {
        public string CustomerId { get; set; }
        public string InstanceId { get; set; }
        public string InstanceType { get; set; }
        public DateTime UsedFrom { get; set; }
        public DateTime UsedUntil { get; set;}
    }
}
