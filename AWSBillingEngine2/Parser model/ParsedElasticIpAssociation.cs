using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSBillingEngine2.Parser_model
{
    public class ParsedElasticIpAssociation
    {
        public string ElasticIPAddress { get; set; }
        public string Ec2InstanceID { get; set; }
        public DateTime AssociatedFrom { get; set; }
        public DateTime AssociatedUntil { get; set;}
    }
}
