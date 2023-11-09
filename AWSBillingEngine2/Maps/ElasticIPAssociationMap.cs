using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AWSBillingEngine2.Parser_model;
using CsvHelper.Configuration;

namespace AWSBillingEngine2.Maps
{
    public sealed class ElasticIPAssociationMap : ClassMap<ParsedElasticIpAssociation>
    {
        public ElasticIPAssociationMap()
        {
            Map(m => m.ElasticIPAddress).Name("IP Address");
            Map(m => m.Ec2InstanceID).Name("EC2 Instance");
            Map(m => m.AssociatedFrom).Name("Associated From");
            Map(m => m.AssociatedUntil).Name("Associate Until");
        }
    }
}
