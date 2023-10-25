using AWSBillingEngine2.Parser_model;
using CsvHelper.Configuration;

namespace AWSBillingEngine2.Maps;

public sealed class CustomerMap : ClassMap<ParsedCustomer>
{
    public CustomerMap()
    {
        Map(m => m.CustomerId).Name("Customer ID");
        Map(m => m.CustomerName).Name("Customer Name");
    }
}