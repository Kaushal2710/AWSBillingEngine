using System.Globalization;

namespace AWSBillingEngine2.Domain_model;

public class Ec2Instance
{
    public string InstanceId { get; set; }
    public OperatingSystem OperatingSystem { get; set; }
    public Ec2InstanceType Ec2InstanceType;
    public List<AwsResourceUsage> AwsResourceUsages;
   
    public Ec2Instance(string instanceId, OperatingSystem operatingSystem, string region, Ec2InstanceType ec2InstanceType)
    {
        InstanceId = instanceId;
        OperatingSystem = operatingSystem;
        Ec2InstanceType = ec2InstanceType;
        AwsResourceUsages = new List<AwsResourceUsage>();
    }


    //public static Ec2Instance CreateEc2Instance(Customer customer)
    //{
    //    customer.Ec2Instances.Add(new Ec2Instance());
    //    var instance = customer.Ec2Instances[^1];
    //    return instance;
    //}
}