using AWSBillingEngine2.Parser_model;

namespace AWSBillingEngine2.Domain_model.Model_Generator;

public class Ec2InstanceTypeModelGenerator
{
    public static List<Ec2InstanceType> GenerateEc2InstanceTypeModel(List<ParsedAwsResourceType> parsedTypes, List<ParsedRegion> regions)
    {
        var ec2InstanceTypes = new List<Ec2InstanceType>();
        foreach (var type in parsedTypes)
        {
            var isFreeTierEligible = CheckFreeTierEligibility(typeInstanceType: type.InstanceType, regionName: type.RegionName, regions: regions);
            ec2InstanceTypes.Add(item: new Ec2InstanceType(type: type.InstanceType, onDemandEc2InstanceChargePerHour: type.OnDemandInstanceChargePerHour,
                reservedEc2InstanceChargePerHour: type.ReservedInstanceChargePerHour, regionName: type.RegionName, isFreeTierEligible: isFreeTierEligible));
        }
        return ec2InstanceTypes;
    }

    private static bool CheckFreeTierEligibility(string typeInstanceType, string regionName, List<ParsedRegion> regions)
    {
        if (regions.Count(predicate: region => region.RegionName.Equals(value: regionName) && region.FreeTierEligibleInstance.Equals(value: typeInstanceType)) == 0)
        {
            return false;
        }
        return true;
    }
}