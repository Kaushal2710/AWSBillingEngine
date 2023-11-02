using AWSBillingEngine2.Domain_model;

namespace AWSBillingEngine2;

public class BillEntry
{
    public Region Region { get; set; }
    public string ResourceType { get; set; }
    public int TotalResources { get; set; }
    public TimeSpan TotalUsedTime { get; set; }
    public TimeSpan TotalBilledTime { get; set; }
    public decimal Rate { get; set; }
    public decimal EachTotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal ActualAmount { get; set; }

    private TimeSpan CalculateUsedTime(DateTime usedFrom, DateTime usedUntil)
    {
        TimeSpan usedTime;
        if (usedFrom.Month == usedUntil.Month && usedFrom.Year == usedUntil.Year)
        {
            usedTime = usedUntil - usedFrom;
        }
        else
        {
            var nextMonthDate = usedFrom.AddMonths(1);
            var nextMonthFirstDate = new DateTime(nextMonthDate.Year, nextMonthDate.Month, 1);

            usedTime = nextMonthFirstDate - usedFrom;
        }

        return usedTime;
    }
    public TimeSpan AddUsedTime(AwsResourceUsage usage)
    {
        var usedFrom = usage.UsedFrom;
        var usedUntil = usage.UsedUntil;
        var usedTime = CalculateUsedTime(usedFrom, usedUntil);
        TotalUsedTime += usedTime;
        return usedTime;
    }

    public TimeSpan AddBilledTime(TimeSpan usedTime)
    {
        var billedTime = (new TimeSpan(0, (int)Math.Ceiling(usedTime.TotalHours), 00, 00));
        TotalBilledTime += billedTime;
        return billedTime;
    }

    public void AddEachTotalAmount(TimeSpan billedTime, decimal rate)
    {
        EachTotalAmount += (decimal)billedTime.TotalHours * rate;
    }

    public void AddDiscount(Ec2Instance ec2Instance, AwsResourceUsage usage, DateTime freeUsageEndDate, decimal rate, TimeSpan billedTime)
    {
        var discount = CalculateDiscount(ec2Instance, usage, freeUsageEndDate, rate, billedTime);
        Discount += discount;
    }
    private decimal CalculateDiscount(Ec2Instance ec2Instance, AwsResourceUsage usage, DateTime freeUsageEndDate, decimal rate, TimeSpan billedTime)
    {
        if (!ec2Instance.Ec2InstanceType.IsFreeTierEligible || usage.UsedFrom >= freeUsageEndDate) return 0;
        var freeLinuxHours = 0;
        var freeWindowsHours = 0;
        decimal discount = 0;
        if (ec2Instance.OperatingSystem == OperatingSystem.Linux && usage.UsageType != UsageType.Reserved)
        {
            freeLinuxHours += (int)billedTime.TotalHours;
        }
        else if (ec2Instance.OperatingSystem == OperatingSystem.Windows && usage.UsageType != UsageType.Reserved)
        {
            freeWindowsHours += (int)billedTime.TotalHours;
        }

        if (freeLinuxHours < 750)
        {
            discount += freeLinuxHours * rate;
        }
        else
        {
            discount += 750 * rate;
        }

        if (freeWindowsHours < 750)
        {
            discount += freeWindowsHours * rate;
        }
        else
        {
            discount += 750 * rate;
        }
        return discount;
    }
}