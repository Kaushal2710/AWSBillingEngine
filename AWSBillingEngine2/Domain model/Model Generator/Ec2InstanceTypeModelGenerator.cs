using AWSBillingEngine2.Parser_model;

namespace AWSBillingEngine2.Domain_model.Model_Generator;

public class Ec2InstanceTypeModelGenerator
{
    public static List<Ec2InstanceType> GenerateEc2InstanceTypeModel(List<ParsedAwsResourceType> parsedTypes, List<ParsedRegion> regions)
    {
        var ec2InstanceTypes = new List<Ec2InstanceType>();
        foreach (var type in parsedTypes)
        {
            var isFreeTierEligible = CheckFreeTierEligibility(type.InstanceType, type.RegionName, regions);
            ec2InstanceTypes.Add(new Ec2InstanceType(type.InstanceType, type.OnDemandInstanceChargePerHour,
                type.ReservedInstanceChargePerHour, type.RegionName, isFreeTierEligible));
        }
        return ec2InstanceTypes;
    }

    private static bool CheckFreeTierEligibility(string typeInstanceType, string regionName, List<ParsedRegion> regions)
    {
        if (regions.Count(region => region.RegionName.Equals(regionName) && region.FreeTierEligibleInstance.Equals(typeInstanceType)) == 0)
        {
            return false;
        }
        return true;
    }
}