using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSBillingEngine2.Parser_model
{
    public class ParsedAwsReservedInstanceUsage
    {
        public string CustomerId { get; set; }
        public string InstanceId { get; set; }
        public string InstanceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RegionName { get; set; }
        public OperatingSystem OperatingSystem { get; set; }

    }
}
