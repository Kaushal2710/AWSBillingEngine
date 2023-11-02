using AWSBillingEngine2.Domain_model;
using System;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AWSBillingEngine2
{
    internal class BillGenerator
    {
        public static List<Bill> GenerateBill(List<Customer> customers)
        {
            var bills = new List<Bill>();

            foreach (var customer in customers)
            {
                var freeUsageEndDate = customer.GetFreeUsageEndDate();
                
                foreach (var ec2Instance in customer.Ec2Instances)
                {
                    var isSameInstance = false;
                    
                    foreach (var usage in ec2Instance.AwsResourceUsages)
                    { 
                        while (usage.UsedFrom < usage.UsedUntil)
                        {
                            BillEntry? billEntry;
                            var bill = FindBill(bills, customer.CustomerId, usage.UsedFrom);

                            if (bill == null)
                            {
                                bill = AddBill(bills, customer, usage, ec2Instance);
                                billEntry = bill.AddBillEntry(ec2Instance.Ec2InstanceType.Region,
                                    ec2Instance.Ec2InstanceType.Type);
                            }
                            else
                            {
                                billEntry = bill.GetBillEntryByRegionAndType(ec2Instance.Ec2InstanceType.Type, ec2Instance.Ec2InstanceType.Region);

                                if (billEntry == null)
                                {
                                    billEntry = bill.AddBillEntry(ec2Instance.Ec2InstanceType.Region,
                                        ec2Instance.Ec2InstanceType.Type);
                                }
                                else if (!isSameInstance)
                                {
                                    billEntry.TotalResources++;
                                }
                            }

                            var usedTime = billEntry.AddUsedTime(usage);
                            var billedTime = billEntry.AddBilledTime(usedTime);

                            var rate = GetRate(usage.UsageType, ec2Instance.Ec2InstanceType);
                            billEntry.AddEachTotalAmount(billedTime, rate);

                            if (usage.UsedFrom < freeUsageEndDate) //usage in Discount period
                            {
                                billEntry.AddDiscount(ec2Instance, usage, freeUsageEndDate, rate, billedTime);
                            }

                            usage.UsedFrom = new DateTime(usage.UsedFrom.AddMonths(1).Year, usage.UsedFrom.AddMonths(1).Month, 1, 0, 0,
                                0);
                        }
                        isSameInstance = true;
                    }
                }
            }
            
            CalculateBills(bills);
            return bills;
        }

        private static void CalculateBills(List<Bill> bills)
        {
            foreach (var bill in bills)
            {
                bill.TotalAmount = bill.BillEntries.Sum(eachTotalAmount => eachTotalAmount.EachTotalAmount);
                bill.DiscountAmount = bill.BillEntries.Sum(discount => discount.Discount);
            }
        }

        public static decimal GetRate(UsageType usageType, Ec2InstanceType ec2InstanceType)
        {
            var rate = usageType == UsageType.OnDemand
                ? ec2InstanceType.OnDemandEc2InstanceChargePerHour
                : ec2InstanceType.ReservedEc2InstanceChargePerHour;
            return rate;
        }

        public static Bill? FindBill(List<Bill> bills, string customerId, DateTime usedFrom)
        {
            return bills.FirstOrDefault(bill => bill.BillMonth == usedFrom.Month && bill.CustomerId == customerId && bill.BillYear == usedFrom.Year);
        }
        private static Bill AddBill(List<Bill> bills, Customer customer, AwsResourceUsage awsOnDemandResourceUsage,
            Ec2Instance ec2Instance)
        {
            var bill = new Bill( customer.CustomerId, customer.CustomerName)
            {
                BillMonth = awsOnDemandResourceUsage.UsedFrom.Month,
                BillYear = awsOnDemandResourceUsage.UsedFrom.Year
            };
            
            bills.Add(bill);
            return bill;
        }
    }
}
