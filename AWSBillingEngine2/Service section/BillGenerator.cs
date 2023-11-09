using AWSBillingEngine2.Domain_model;
using System;
using System.Diagnostics.Tracing;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AWSBillingEngine2
{
    internal class BillGenerator
    {
        public static List<CustomerMonthlyBills> GenerateBill(List<Customer> customers)
        {
            var bills = new List<CustomerMonthlyBills>();

            foreach (var customer in customers)
            {
                var customerBills = new CustomerMonthlyBills(customer);
                var freeUsageEndDate = customer.GetFreeUsageEndDate();

                foreach (var ec2Instance in customer.Ec2Instances)
                {
                    foreach (var usage in ec2Instance.AwsResourceUsages)
                    {
                        while (usage.UsedFrom < usage.UsedUntil)
                        {
                            var bill = customerBills.FindBill(usage.UsedFrom);

                            if (bill == null)
                            {
                                bill = customerBills.AddBill(usage, ec2Instance);
                            }

                            var billEntry = bill.GetBillEntryByRegionAndType(ec2Instance.Ec2InstanceType);

                            if (billEntry == null)
                            {
                                billEntry = CreateBillEntry(ec2Instance.Ec2InstanceType);
                                bill.AddBillEntry(billEntry);
                            }

                            billEntry.AddInstanceIdIfNotPresent(ec2Instance.InstanceId);

                            var usedTime = billEntry.CalculateUsedTime(usage);
                            billEntry.AddUsedTime(usedTime);

                            var billedTime = billEntry.CalculateBilledtime(usedTime);
                            billEntry.AddBilledTime(billedTime);

                            var rate = GetRate(usage.UsageType, ec2Instance.Ec2InstanceType);
                            var eachTotalAmount = (decimal)billedTime.TotalHours * rate;
                            billEntry.AddEachTotalAmount(eachTotalAmount);

                            if (usage.UsedFrom < freeUsageEndDate) //usage in Discount period
                            { 
                                billEntry.AddDiscountHours(ec2Instance.Ec2InstanceType, usage, freeUsageEndDate, billedTime);
                                var discount = billEntry.CalculateDiscount(ec2Instance, usage, freeUsageEndDate, rate, billedTime,
                                    bill);
                                billEntry.Discount += discount;
                            }

                            var nextMonthDate = usage.UsedFrom.AddMonths(1);
                            usage.UsedFrom = new DateTime(nextMonthDate.Year, nextMonthDate.Month, 1, 0, 0,
                                0);
                        }
                    }
                }
                customerBills.CalculateBills();
                bills.Add(customerBills);
            }
            return bills;
        }

        
        public static decimal GetRate(UsageType usageType, Ec2InstanceType ec2InstanceType)
        {
            var rate = usageType == UsageType.OnDemand
                ? ec2InstanceType.OnDemandEc2InstanceChargePerHour
                : ec2InstanceType.ReservedEc2InstanceChargePerHour;
            return rate;
        }


        private static BillEntry CreateBillEntry(Ec2InstanceType ec2InstanceType)
        {
            var billEntry = new BillEntry()
            {
                ResourceType = ec2InstanceType.Type,
                Region = ec2InstanceType.Region,
                TotalUsedTime = new TimeSpan(),
                TotalBilledTime = new TimeSpan(),
                EachTotalAmount = 0,
                Discount = 0.0000M,
                ActualAmount = 0,
                TotalResources = 1,
                LinuxHours = 0,
                WindowsHours = 0,
                InstanceIds = new List<string>()
            };
            return billEntry;
        }
    }
}
