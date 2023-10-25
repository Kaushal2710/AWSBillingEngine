using AWSBillingEngine2.Parser_model;
using CsvHelper.Configuration;

namespace AWSBillingEngine2.Maps;

public sealed class AwsResourceUsagesMap : ClassMap<ParsedUsage>
{
    public AwsResourceUsagesMap()
    {
        Map(m => m.CustomerId).Name("Customer ID");
        Map(m => m.UsedFrom).Name("Used From");
        Map(m => m.UsedUntil).Name("Used Until");
        Map(m => m.InstanceId).Name("EC2 Instance ID");
        Map(m => m.InstanceType).Name("EC2 Instance Type");
    }
}