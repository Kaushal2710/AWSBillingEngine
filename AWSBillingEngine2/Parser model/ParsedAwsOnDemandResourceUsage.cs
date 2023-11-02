namespace AWSBillingEngine2.Parser_model
{
    public class ParsedAwsOnDemandResourceUsage
    {
        public string CustomerId { get; set; }
        public string InstanceId { get; set; }
        public string InstanceType { get; set; }
        public DateTime UsedFrom { get; set; }
        public DateTime UsedUntil { get; set; }
        public string RegionName { get; set; }
        public OperatingSystem OperatingSystem { get; set; }
    }
}
