using AWSBillingEngine2.Domain_model;

namespace AWSBillingEngine2;

public class BillEntry
{
    public Region Region { get; set; }
    public string ResourceType { get; set; }
    public List<string> InstanceIds { get; set; }
    public int TotalResources { get; set; }
    public TimeSpan TotalUsedTime { get; set; }
    public TimeSpan TotalBilledTime { get; set; }
    public decimal EachTotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal ActualAmount { get; set; }
    public int LinuxHours { get; set; }
    public int WindowsHours { get; set; }


    public TimeSpan CalculateUsedTime(AwsResourceUsage usage)
    {
        var usedFrom = usage.UsedFrom;
        var usedUntil = usage.UsedUntil;
        TimeSpan usedTime;
        if (usedFrom.Month == usedUntil.Month && usedFrom.Year == usedUntil.Year)
        {
            usedTime = usedUntil - usedFrom;
        }
        else
        {
            var nextMonthDate = usedFrom.AddMonths(months: 1);
            var nextMonthFirstDate = new DateTime(year: nextMonthDate.Year, month: nextMonthDate.Month, day: 1);

            usedTime = nextMonthFirstDate - usedFrom;
        }

        return usedTime;
    }
    public void AddUsedTime(TimeSpan usedTime)
    {
        TotalUsedTime += usedTime;
    }

    public void AddBilledTime(TimeSpan billedTime)
    {
        TotalBilledTime += billedTime;
    }

    public TimeSpan CalculateBilledtime(TimeSpan usedTime)
    {
        var billedTime = (new TimeSpan(days: 0, hours: (int)Math.Ceiling(a: usedTime.TotalHours), minutes: 00, seconds: 00));
        return billedTime;
    }

    public void AddEachTotalAmount(decimal eachTotalAmount)
    {
        EachTotalAmount += eachTotalAmount;
    }

    public void AddDiscountHours(Ec2InstanceType ec2InstanceType, AwsResourceUsage usage, DateTime freeUsageEndDate, TimeSpan billedTime)
    {
        if (!ec2InstanceType.IsFreeTierEligible || usage.UsedFrom >= freeUsageEndDate) return;
       
        if (usage.OperatingSystem == OperatingSystem.Linux && usage.UsageType != UsageType.Reserved)
        {
            LinuxHours += (int)billedTime.TotalHours;
        }
        else if (usage.OperatingSystem == OperatingSystem.Windows && usage.UsageType != UsageType.Reserved)
        {
            WindowsHours += (int)billedTime.TotalHours;
        }
    }
    public decimal CalculateDiscount(Ec2Instance ec2Instance, AwsResourceUsage usage, DateTime freeUsageEndDate, decimal rate, TimeSpan billedTime, Bill bill)
    {
        var discount = 0.0000m;
        if (LinuxHours < bill.LinuxHours)
        {
            discount += LinuxHours * rate;
            bill.LinuxHours -= LinuxHours;
        }
        else
        {
            discount += bill.LinuxHours * rate;
            bill.LinuxHours = 0;
        }

        if (WindowsHours < bill.WindowsHours)
        {
            discount += WindowsHours * rate;
            bill.WindowsHours -= WindowsHours;
        }
        else
        {
            discount += bill.WindowsHours * rate;
            bill.WindowsHours = 0;
        }

        LinuxHours = 0;
        WindowsHours = 0;
        return discount;
    }

    public void AddInstanceIdIfNotPresent(string ec2InstanceInstanceId)
    {
        var instanceId = InstanceIds.Find(_ => _.Equals(ec2InstanceInstanceId));
        if (instanceId == null)
        {
            InstanceIds.Add(ec2InstanceInstanceId);
        }
    }
}