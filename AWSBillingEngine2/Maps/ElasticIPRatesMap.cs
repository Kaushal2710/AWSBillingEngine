using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AWSBillingEngine2.Parser_model;
using CsvHelper.Configuration;

namespace AWSBillingEngine2.Maps
{
    public sealed class ElasticIPRatesMap : ClassMap<ParsedElasticIpRates>
    {
        public ElasticIPRatesMap()
        {
            Map(m => m.Region.RegionName).Name("Region");
            Map(m => m.RatePerHour).Name("Rate/Hour");
        }
    }
}
