namespace AWSBillingEngine2.Domain_model;

public class Ec2Instance
{
    public string InstanceId;
    public Ec2InstanceType Ec2InstanceType;
    public List<AwsResourceUsage> Usages;

    public Ec2Instance(string instanceId, Ec2InstanceType ec2InstanceType)
    {
        InstanceId = instanceId;
        Usages = new List<AwsResourceUsage>();
        Ec2InstanceType = ec2InstanceType;
    }


    //public static Ec2Instance CreateEc2Instance(Customer customer)
    //{
    //    customer.Ec2Instances.Add(new Ec2Instance());
    //    var instance = customer.Ec2Instances[^1];
    //    return instance;
    //}
}