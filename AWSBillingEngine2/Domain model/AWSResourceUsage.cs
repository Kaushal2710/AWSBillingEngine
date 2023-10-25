namespace AWSBillingEngine2.Domain_model;

public class AwsResourceUsage
{
    public DateTime UsedFrom;
    public DateTime UsedUntil;

    public AwsResourceUsage(DateTime usedFrom, DateTime usedUntil)
    {
        UsedFrom = usedFrom;
        UsedUntil = usedUntil;
    }
}