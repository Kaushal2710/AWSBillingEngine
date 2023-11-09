namespace AWSBillingEngine2.Domain_model;

public class Ec2InstanceType
{
   // public decimal Charge { get; set; }
    public string Type { get; set; }
    public decimal OnDemandEc2InstanceChargePerHour { get; set; }
    public decimal ReservedEc2InstanceChargePerHour { get; set; }
    public Region Region { get; set; }
    public bool IsFreeTierEligible { get; set; }
    public Ec2InstanceType(string type, decimal onDemandEc2InstanceChargePerHour, decimal reservedEc2InstanceChargePerHour, string regionName, bool isFreeTierEligible)
    {
        //Charge = charge;
        Type = type;
        OnDemandEc2InstanceChargePerHour = onDemandEc2InstanceChargePerHour;
        ReservedEc2InstanceChargePerHour = reservedEc2InstanceChargePerHour;
        Region = new Region(regionName: regionName);
        IsFreeTierEligible = isFreeTierEligible;
    }
}