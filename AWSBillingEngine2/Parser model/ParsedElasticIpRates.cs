using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AWSBillingEngine2.Domain_model;

namespace AWSBillingEngine2.Parser_model
{
    public class ParsedElasticIpRates
    {
        public Region Region { get; set; }
        public decimal RatePerHour { get; set; }
    }
}
