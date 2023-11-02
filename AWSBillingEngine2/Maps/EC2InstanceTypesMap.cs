using CsvHelper.Configuration;
using AWSBillingEngine2.Parser_model;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace AWSBillingEngine2.Maps;

public sealed class Ec2InstanceTypesMap : ClassMap<ParsedAwsResourceType>
{
    public Ec2InstanceTypesMap()
    {
        Map(m => m.InstanceType).Name("Instance Type");
        Map(m => m.OnDemandInstanceChargePerHour).Name("Charge/Hour(OnDemand)")
            .TypeConverter<CustomStringToDecimalConverter>();
        Map(m => m.ReservedInstanceChargePerHour).Name("Charge/Hour(Reserved)")
            .TypeConverter<CustomStringToDecimalConverter>();
        Map(m => m.RegionName).Name("Region");
    }
    public class CustomStringToDecimalConverter : DecimalConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (!string.IsNullOrEmpty(text) && text.StartsWith("$"))
            {
                if (decimal.TryParse(text, out decimal output))
                {
                    return output;
                }
                if (decimal.TryParse(text.Substring(1), out decimal result))
                {
                    return result;
                }
            }
            // If the text doesn't match the expected format, fall back to the base DecimalConverter
            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}