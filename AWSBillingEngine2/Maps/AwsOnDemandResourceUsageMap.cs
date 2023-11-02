using AWSBillingEngine2.Parser_model;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSBillingEngine2.Maps
{
    public sealed class AwsOnDemandResourceUsageMap : ClassMap<ParsedAwsOnDemandResourceUsage>
    {
            public AwsOnDemandResourceUsageMap()
            {
                Map(m => m.CustomerId).Name("Customer ID");
                Map(m => m.UsedFrom).Name("Used From");
                Map(m => m.UsedUntil).Name("Used Until");
                Map(m => m.InstanceId).Name("EC2 Instance ID");
                Map(m => m.InstanceType).Name("EC2 Instance Type");
                Map(m => m.RegionName).Name("Region");
                Map(m => m.OperatingSystem).Name("OS");
            }
        
    }
}
