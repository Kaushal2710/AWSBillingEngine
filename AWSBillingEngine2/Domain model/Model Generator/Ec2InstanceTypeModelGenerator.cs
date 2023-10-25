using AWSBillingEngine2.Parser_model;

namespace AWSBillingEngine2.Domain_model.Model_Generator;

public class Ec2InstanceTypeModelGenerator
{
    public static List<Ec2InstanceType> GenerateEc2InstanceTypeModel(List<ParsedTypes> parsedTypes)
    {
        var ec2InstanceTypes = new List<Ec2InstanceType>();
        foreach (var type in parsedTypes)
        {
            ec2InstanceTypes.Add(new Ec2InstanceType(type.Charge, type.InstanceType));
        }
        return ec2InstanceTypes;
    }
}