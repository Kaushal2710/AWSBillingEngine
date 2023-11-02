namespace AWSBillingEngine2.Parser_model
{
    public class ParsedAwsResourceType
    {
        public string InstanceType { get; set; }
        public decimal OnDemandInstanceChargePerHour { get; set; }
        public decimal ReservedInstanceChargePerHour { get; set; }
        //public decimal Charge { get; set; }
        public string RegionName { get; set; }
    }
}
