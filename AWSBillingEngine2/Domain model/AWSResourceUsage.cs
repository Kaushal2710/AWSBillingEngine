using System.ComponentModel;
using AWSBillingEngine2.Domain_model.Model_Generator;

namespace AWSBillingEngine2.Domain_model;

public class AwsResourceUsage
{
    public DateTime UsedFrom;
    public DateTime UsedUntil;
    public UsageType UsageType;
    public OperatingSystem OperatingSystem;
    public AwsResourceUsage(DateTime usedFrom, DateTime usedUntil, UsageType usageType, OperatingSystem operatingSystem)
    {
        UsedFrom = usedFrom;
        UsedUntil = usedUntil;
        UsageType = usageType;
        OperatingSystem = operatingSystem;
    }
}