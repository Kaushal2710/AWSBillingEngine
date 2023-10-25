using AWSBillingEngine2.Domain_model;
using AWSBillingEngine2.Maps;
using AWSBillingEngine2.Parser_model;

namespace AWSBillingEngine2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string customerFilePath = @"..\..\..\Resources\Customer.csv";
            const string resourceUsageFilePath = @"..\..\..\Resources\AWSResourceUsage.csv";
            const string resourceTypeFilePath = @"..\..\..\Resources\AWSResourceTypes.csv";

            var parsedCustomers =
               CsvProcessor.ProcessCsvFile<ParsedCustomer>(customerFilePath, new CustomerMap());

            var parsedUsages = CsvProcessor.ProcessCsvFile<ParsedUsage>(resourceUsageFilePath, new AwsResourceUsagesMap());

            var parsedTypes = CsvProcessor.ProcessCsvFile<ParsedTypes>(resourceTypeFilePath, new Ec2InstanceTypesMap());

            var customers = DomainModelGenerator.GenerateDomainModel(parsedCustomers, parsedUsages, parsedTypes);

            var bills = BillGenerator.GenerateBill(customers);
            BillCsvGenerator.GenerateBillCsvFile(bills);
            Console.WriteLine("Bills for the given data successfully generated.");
        }
    }
}