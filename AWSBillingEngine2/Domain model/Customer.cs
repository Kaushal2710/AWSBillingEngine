using AWSBillingEngine2.Domain_model.Model_Generator;
using AWSBillingEngine2.Parser_model;

namespace AWSBillingEngine2.Domain_model;

public class Customer
{
    public string CustomerId;
    public string CustomerName;
    public List<Ec2Instance> Ec2Instances;

    public Customer()
    {

    }

    public Customer(string customerId, string customerName)
    {
        CustomerId = customerId;
        CustomerName = customerName;
        Ec2Instances = new List<Ec2Instance>();
    }

    public Ec2Instance? GetEc2InstanceById(string usageId)
    {
        return Ec2Instances.FirstOrDefault(ec2Instance => ec2Instance.InstanceId == usageId);
    }

    public void AddEc2Instance(Ec2Instance ec2Instance)
    {
        Ec2Instances.Add(ec2Instance);
    }

    
    public Customer AddOnDemandAwsResourceUsages(Customer customer,
        List<ParsedAwsOnDemandResourceUsage> customerOnDemandAwsResourceUsages, List<ParsedRegion> regions,
        List<Ec2InstanceType> ec2InstanceTypes)
    {
        foreach (var usage in customerOnDemandAwsResourceUsages)
        {
            var ec2Instance = customer.GetEc2InstanceById(usage.InstanceId);
            var category = UsageType.OnDemand;
            var region = regions.First(region => region.RegionName.Equals(usage.RegionName));
            var operatingSystem = usage.OperatingSystem;
            var ec2InstanceType = ec2InstanceTypes.First(type =>
                type.Type.Equals(usage.InstanceType) && type.Region.RegionName.Equals(usage.RegionName));

            if (ec2Instance == null)
            {
                ec2Instance = new Ec2Instance(usage.InstanceId, usage.OperatingSystem, region.RegionName,
                    ec2InstanceType);
                customer.AddEc2Instance(ec2Instance);
            }

            ec2Instance.AwsResourceUsages.Add(new AwsResourceUsage(usage.UsedFrom, usage.UsedUntil, category, operatingSystem));
        }

        return customer;
    }

    public Customer AddReservedInstanceUsages(Customer customer,
        List<ParsedAwsReservedInstanceUsage> customerReservedInstanceUsages, List<ParsedRegion> regions,
        List<Ec2InstanceType> ec2InstanceTypes)
    {
        foreach (var usage in customerReservedInstanceUsages)
        {
            var ec2Instance = customer.GetEc2InstanceById(usage.InstanceId);
            var category = UsageType.Reserved;
            var region = regions.First(region => region.RegionName.Equals(usage.RegionName));
            var operatingSystem = usage.OperatingSystem;

            var ec2InstanceType = ec2InstanceTypes.First(type =>
                type.Type.Equals(usage.InstanceType) && type.Region.RegionName.Equals(usage.RegionName));

            if (ec2Instance == null)
            {
                ec2Instance = new Ec2Instance(usage.InstanceId, usage.OperatingSystem, region.RegionName,
                    ec2InstanceType);
                customer.AddEc2Instance(ec2Instance);
            }

            ec2Instance.AwsResourceUsages.Add(new AwsResourceUsage(usage.StartDate, usage.EndDate, category, operatingSystem));
        }

        return customer;
    }

    public DateTime GetFreeUsageEndDate()
    {
        var freeUsageStartDate = Ec2Instances
            .SelectMany(instance => instance.AwsResourceUsages)
            .OrderBy(usage => usage.UsedFrom)
            .Select(usage => usage.UsedFrom)
            .FirstOrDefault();
        /*
         * FreeUsageStartDate is considered as the first day of the month of the first usage.
         * FreeUsageEndDate is the freeUsageStartDate past 12 months.
         */
        freeUsageStartDate = new DateTime(freeUsageStartDate.Year, freeUsageStartDate.Month, 1);
        var freeUsageEndDate = freeUsageStartDate.AddMonths(12);
        return freeUsageEndDate;
    }
    //public List<MonthlyUsage> GetMainUsages(Customer customer)
    //{
    //    var monthlyUsages = new List<MonthlyUsage?>();

    //    foreach (var ec2Instance in customer.Ec2Instances)
    //    {
    //        var subUsages = new List<SubUsage>();
    //        foreach (var usage in ec2Instance.AwsResourceUsages)
    //        {
    //            var monthlyUsage = GetMonthlyUsage(monthlyUsages, usage.UsedFrom.Month, usage.UsedFrom.Year);
    //            if (monthlyUsage == null)
    //            {
    //                monthlyUsage = new MonthlyUsage
    //                {
    //                    Year = usage.UsedFrom.Year,
    //                    Month = usage.UsedFrom.Month
    //                };
    //            }

    //            while (usage.UsedFrom.Year <= usage.UsedUntil.Year)
    //            {
    //                while (usage.UsedFrom.Month <= usage.UsedUntil.Month ||
    //                       usage.UsedFrom.Year < usage.UsedUntil.Year)
    //                {
    //                    var subUsage =
    //                        GetSubUsageByRegionAndType(monthlyUsage, ec2Instance.Ec2InstanceType.Region, ec2Instance.Ec2InstanceType.Type) ?? new SubUsage
    //                        {
    //                            Ec2InstanceType = ec2Instance.Ec2InstanceType,
    //                            Region = ec2Instance.Ec2InstanceType.Region
    //                        };
    //                    if (!subUsage.Ec2InstancesID.Contains(ec2Instance.InstanceId))
    //                    {
    //                        subUsage.Ec2InstancesID.Add(ec2Instance.InstanceId);
    //                    }
    //                    var rate = BillGenerator.GetRate(usage, ec2Instance);
    //                    if (usage.UsedFrom.Month == usage.UsedUntil.Month &&
    //                        usage.UsedFrom.Year == usage.UsedUntil.Year)
    //                    {
    //                        subUsage.TotalUsedTime += (usage.UsedUntil - usage.UsedFrom);
    //                        subUsage.TotalBilledTime += (new TimeSpan(0,
    //                            (int)Math.Ceiling((usage.UsedUntil - usage.UsedFrom).TotalHours), 0, 0));
    //                        subUsage.TotalAmount = (decimal)subUsage.TotalBilledTime.TotalHours * rate;
    //                        subUsage.OperatingSystem = ec2Instance.OperatingSystem;
    //                        subUsages.Add(subUsage);
    //                        usage.UsedFrom = usage.UsedFrom.Month != 12
    //                            ? new DateTime(usage.UsedFrom.Year, usage.UsedFrom.Month + 1, 1, 0, 0,
    //                                0)
    //                            : usage.UsedFrom = new DateTime(usage.UsedFrom.Year + 1, 1, 1, 0, 0,
    //                                0);
    //                        break;
    //                    }

    //                    var lastDayOfTheMonth = DateTime.DaysInMonth(usage.UsedFrom.Year, usage.UsedFrom.Month);

    //                    if (usage.UsedFrom.Year < usage.UsedUntil.Year && usage.UsedFrom.Month == 12)
    //                    {
    //                        subUsage.TotalUsedTime += (new DateTime(usage.UsedFrom.Year,
    //                            usage.UsedFrom.Month,
    //                            lastDayOfTheMonth, 23, 59, 59) - usage.UsedFrom + new TimeSpan(0, 0, 0, 1));
    //                        subUsage.TotalBilledTime += (new TimeSpan(0, (int)Math.Ceiling((new DateTime(
    //                                    usage.UsedFrom.Year, usage.UsedFrom.Month,
    //                                    lastDayOfTheMonth, 23, 59, 59) - usage.UsedFrom +
    //                                new TimeSpan(0, 0, 0, 1))
    //                            .TotalHours), 0, 0));
    //                    }
    //                    else
    //                    {
    //                        subUsage.TotalUsedTime += (new DateTime(usage.UsedFrom.Year,
    //                            usage.UsedFrom.Month,
    //                            lastDayOfTheMonth, 23, 59, 59) - usage.UsedFrom + new TimeSpan(0, 0, 0, 1));
    //                        subUsage.TotalBilledTime += (new TimeSpan(0, (int)Math.Ceiling((new DateTime(
    //                            usage.UsedFrom.Year, usage.UsedFrom.Month,
    //                            lastDayOfTheMonth, 23, 59, 59) - usage.UsedFrom).TotalHours), 0, 0));

    //                    }

    //                    subUsages.Add(subUsage);

    //                    if (usage.UsedFrom.Month == 12)
    //                    {
    //                        break;
    //                    }

    //                    usage.UsedFrom = new DateTime(usage.UsedFrom.Year, usage.UsedFrom.Month + 1, 1, 0, 0,
    //                        0);
    //                }

    //                usage.UsedFrom = new DateTime(usage.UsedFrom.Year + 1, 1, 1, 0, 0,
    //                    0);
    //                //isSameInstance = true;
    //            }
    //        }
    //    }
    //    return monthlyUsages;
    //}

    //private SubUsage? GetSubUsageByRegionAndType(MonthlyUsage monthlyUsage, Region region, string instanceType)
    //{ 
    //    var subUsage = monthlyUsage.SubUsages.FirstOrDefault(usage => usage.Region.Equals(region) && usage.Ec2InstanceType.Type.Equals(instanceType)); 
    //    return subUsage;
    //}

    //private static MonthlyUsage? GetMonthlyUsage(List<MonthlyUsage?> monthlyUsages,int usedFromMonth, int usedFromYear)
    //{
    //    return monthlyUsages.FirstOrDefault(usage => usage.Month == usedFromMonth && usage.Year == usedFromYear);
    //}

}

//public class MonthlyUsage
//{
//    public int Year { get; set; }
//    public int Month { get; set; }

//    public List<SubUsage> SubUsages{ get; set; }
//}

//public class SubUsage
//{
//    public Region Region { get; set; }
//    public Ec2InstanceType Ec2InstanceType { get; set; }
//    public List<string> Ec2InstancesID { get; set; }
//    public TimeSpan TotalUsedTime { get; set; }
//    public TimeSpan TotalBilledTime { get; set; }
//    public decimal TotalAmount { get; set; }
//    public decimal TotalDiscount { get; set; }
//    public OperatingSystem OperatingSystem { get; set; }
//}