using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AWSBillingEngine2.Parser_model;
using CsvHelper.Configuration;

namespace AWSBillingEngine2.Maps
{
    public sealed class AwsReservedInstanceUsageMap : ClassMap<ParsedAwsReservedInstanceUsage>
    {
        public AwsReservedInstanceUsageMap()
        {
            Map(m => m.CustomerId).Name("Customer ID");
            Map(m => m.StartDate).Name("Start Date");
            Map(m => m.EndDate).Name("End Date");
            Map(m => m.InstanceId).Name("EC2 Instance ID");
            Map(m => m.InstanceType).Name("EC2 Instance Type");
            Map(m => m.RegionName).Name("Region");
            Map(m => m.OperatingSystem).Name("OS");
        }
    }
}
