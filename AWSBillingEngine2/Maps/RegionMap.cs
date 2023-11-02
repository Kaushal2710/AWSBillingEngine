using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AWSBillingEngine2.Parser_model;
using CsvHelper.Configuration;

namespace AWSBillingEngine2.Maps
{
    public sealed class RegionMap : ClassMap<ParsedRegion>
    {
        public RegionMap()
        {
            Map(m => m.RegionName).Name("Region");
            Map(m => m.FreeTierEligibleInstance).Name("Free Tier Eligible");
        }
    }
}
