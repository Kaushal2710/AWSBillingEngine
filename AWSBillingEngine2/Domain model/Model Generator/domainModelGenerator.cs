using AWSBillingEngine2.Domain_model.Model_Generator;
using AWSBillingEngine2.Parser_model;

namespace AWSBillingEngine2.Domain_model
{
    public class DomainModelGenerator
    {
        public static List<Customer> GenerateDomainModel(List<ParsedCustomer> parsedCustomers, List<ParsedUsage> parsedUsages, List<ParsedTypes> parsedTypes)
        {
            var ec2InstanceTypes = Ec2InstanceTypeModelGenerator.GenerateEc2InstanceTypeModel(parsedTypes);
            
            var customers = new List<Customer>();
            
            foreach (var parsedCustomer in parsedCustomers)
            {
                var customer = CreateCustomer(parsedCustomer);
                customers.Add(customer);

                var customerAwsResourceUsages =
                    parsedUsages.Where(usage => usage.CustomerId.Equals(parsedCustomer.CustomerId)).ToList();

                foreach (var usage in customerAwsResourceUsages)
                {
                    var ec2Instance = customer.GetEc2InstanceById(usage.InstanceId);
                    var ec2InstanceType = ec2InstanceTypes.First(type => type.Type.Equals(usage.InstanceType));

                    if (ec2Instance == null)
                    {
                        ec2Instance = new Ec2Instance(usage.InstanceId, ec2InstanceType);
                        customer.Ec2Instances.Add(ec2Instance);
                    }
                    ec2Instance.Usages.Add(new AwsResourceUsage(usage.UsedFrom, usage.UsedUntil));
                }
            }
            return customers;
        }
        private static Customer CreateCustomer(ParsedCustomer parsedParsedCustomer)
        {
            return new Customer(parsedParsedCustomer.CustomerId, parsedParsedCustomer.CustomerName);
        }
    }
}
