using AWSBillingEngine2.Parser_model;

namespace AWSBillingEngine2.Domain_model.Model_Generator
{
    public class DomainModelGenerator
    {
        public static List<Customer> GenerateDomainModel(List<ParsedCustomer> parsedCustomers,
            List<ParsedAwsOnDemandResourceUsage> parsedAwsOnDemandResourceUsages,
            List<ParsedAwsReservedInstanceUsage> parsedAwsReservedInstanceUsages,
            List<ParsedAwsResourceType> parsedTypes,
            List<ParsedRegion> parsedRegions)
        {
            var customers = new List<Customer>();
            var ec2InstanceTypes = Ec2InstanceTypeModelGenerator.GenerateEc2InstanceTypeModel(parsedTypes, parsedRegions);
            

            foreach (var parsedCustomer in parsedCustomers)
            {
                var customer = CreateCustomer(parsedCustomer);
                customers.Add(customer);

                var customerOnDemandAwsResourceUsages =
                    parsedAwsOnDemandResourceUsages.Where(usage => usage.CustomerId.Equals(parsedCustomer.CustomerId)).ToList();
                var customerReservedAwsResourceUsages = parsedAwsReservedInstanceUsages
                    .Where(usage => usage.CustomerId.Equals(parsedCustomer.CustomerId)).ToList();
                foreach (var reservedUsage in customerReservedAwsResourceUsages)
                {
                    reservedUsage.EndDate = reservedUsage.EndDate.AddDays(1);
                }

                customer.AddOnDemandAwsResourceUsages(customer, customerOnDemandAwsResourceUsages, parsedRegions, ec2InstanceTypes);
                customer.AddReservedInstanceUsages(customer, customerReservedAwsResourceUsages, parsedRegions, ec2InstanceTypes);
            }
            return customers;
        }
        private static Customer CreateCustomer(ParsedCustomer parsedParsedCustomer)
        {
            return new Customer(parsedParsedCustomer.CustomerId, parsedParsedCustomer.CustomerName);
        }
    }

    public class Category
    {
        public string CategoryName { get; set; }
        public Category(string categoryName)
        {
            CategoryName = categoryName;
        }
    }
}
