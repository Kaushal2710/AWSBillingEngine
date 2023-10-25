namespace AWSBillingEngine2.Domain_model;

public class Ec2InstanceType
{
    public decimal Charge { get; set; }
    public string Type { get; set; }

    public Ec2InstanceType(decimal charge, string type)
    {
        Charge = charge;
        Type = type;
    }
}