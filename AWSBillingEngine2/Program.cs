using System.Reflection.Metadata;
using AWSBillingEngine2.Domain_model;
using AWSBillingEngine2.Domain_model.Model_Generator;
using AWSBillingEngine2.Maps;
using AWSBillingEngine2.Parser_model;
using System.Security.Principal;

namespace AWSBillingEngine2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string customerFilePath = @"..\..\..\Resources\Customer.csv";
            //const string resourceUsageFilePath = @"..\..\..\Resources\AWSResourceUsage.csv";
            const string resourceTypeFilePath = @"..\..\..\Resources\AWSResourceTypes.csv";
            const string awsOnDemandResourceUsageFilePath = @"..\..\..\Resources\AWSOnDemandResourceUsage.csv";
            const string awsReservedInstanceUsageFilePath = @"..\..\..\Resources\AWSReservedInstanceUsage.csv";
            const string regionFilePath = @"..\..\..\Resources\Region.csv";
            const string elasticIPAllocationFilePath = @"..\..\..\Resources\ElasticIPAllocation.csv";
            const string elasticIPAssociationFilePath = @"..\..\..\Resources\ElasticIPAssociation.csv";
            const string elasticIPRatesFilePath = @"..\..\..\Resources\ElasticIPRates.csv";

            var parsedCustomers =
               CsvProcessor.ProcessCsvFile<ParsedCustomer>(customerFilePath, new CustomerMap());

            //var parsedUsages = CsvProcessor.ProcessCsvFile<ParsedAwsOnDemandResourceUsage>(resourceUsageFilePath, new AwsResourceUsagesMap());

            var parsedTypes =
                CsvProcessor.ProcessCsvFile<ParsedAwsResourceType>(resourceTypeFilePath, new Ec2InstanceTypesMap());
            var parsedOnDemandResourceUsages =
                CsvProcessor.ProcessCsvFile<ParsedAwsOnDemandResourceUsage>(awsOnDemandResourceUsageFilePath, new AwsOnDemandResourceUsageMap());
            var parsedReservedInstanceUsages =
                CsvProcessor.ProcessCsvFile<ParsedAwsReservedInstanceUsage>(awsReservedInstanceUsageFilePath, new AwsReservedInstanceUsageMap());
            var parsedRegions = 
                CsvProcessor.ProcessCsvFile<ParsedRegion>(regionFilePath, new RegionMap());
            var parsedElasticIPAllocation =
                CsvProcessor.ProcessCsvFile<ParsedElasticIpAllocation>(elasticIPAllocationFilePath, new ElasticIPAllocationMap());
            var parsedElasticIPAssociation =
                CsvProcessor.ProcessCsvFile<ParsedElasticIpAssociation>(elasticIPAssociationFilePath, new ElasticIPAssociationMap());
            var parsedElasticIPRates =
                CsvProcessor.ProcessCsvFile<ParsedElasticIpRates>(elasticIPRatesFilePath, new ElasticIPRatesMap());
            
            //var customers = DomainModelGenerator.GenerateDomainModel(parsedCustomers, parsedUsages, parsedTypes);
            var customers = DomainModelGenerator.GenerateDomainModel(parsedCustomers,
                parsedOnDemandResourceUsages,
                parsedReservedInstanceUsages,
                parsedTypes,
                parsedRegions,
                parsedElasticIPAllocation,
                parsedElasticIPAssociation,
                parsedElasticIPRates);

            var bills = BillGenerator.GenerateBill(customers);

            BillCsvGenerator.GenerateBillCsvFile(bills);

            Console.WriteLine("Monthly Bills for the given data successfully generated.");
        }
    }
}