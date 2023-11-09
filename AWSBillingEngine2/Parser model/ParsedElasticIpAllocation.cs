using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AWSBillingEngine2.Domain_model;

namespace AWSBillingEngine2.Parser_model
{
    public class ParsedElasticIpAllocation
    {
        public string CustomerID { get; set; }
        public Region Region { get; set; }
        public string ElasticIPAddress { get; set; }
        public DateTime AllocatedFrom { get; set; }
        public DateTime AllocatedUntil { get; set;}
        public bool IsOwnIP { get; set; }

    }
}
