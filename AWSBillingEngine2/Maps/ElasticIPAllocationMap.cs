using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AWSBillingEngine2.Parser_model;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace AWSBillingEngine2.Maps
{
    public sealed class ElasticIPAllocationMap : ClassMap<ParsedElasticIpAllocation>
    {
        public ElasticIPAllocationMap()
        {
            Map(m => m.CustomerID).Name("Customer");
            Map(m => m.Region.RegionName).Name("Region");
            Map(m => m.ElasticIPAddress).Name("Elastic IP");
            Map(m => m.AllocatedFrom).Name("Used From");
            Map(m => m.AllocatedUntil).Name("Used Until");
            Map(m => m.IsOwnIP).Name("Your own IP?").TypeConverter<CustomYesNoToBoolConverter>();
        }
    }

    public class CustomYesNoToBoolConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text.ToLower() == "yes")
            {
                return true;
            }

            if (text.ToLower() == "no")
            {
                return false;
            }

            throw new NotSupportedException($"Value {text} cannot be converted to a boolean.");

        }
    }
}
