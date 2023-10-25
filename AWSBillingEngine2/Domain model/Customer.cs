namespace AWSBillingEngine2.Domain_model;

public class Customer
{
    public string CustomerId;
    public string CustomerName;
    public List<Ec2Instance> Ec2Instances;

    public Customer()
    {

    }
    public Customer(string customerId, string customerName)
    {
        CustomerId = customerId;
        CustomerName = customerName;
        Ec2Instances = new List<Ec2Instance>();
    }
    
    public Ec2Instance? GetEc2InstanceById(string usageId)
    {
        return Ec2Instances.FirstOrDefault(ec2Instance => ec2Instance.InstanceId == usageId);
    }

    public void AddEc2Instance(Ec2Instance ec2Instance)
    {
        Ec2Instances.Add(ec2Instance);
    }
}