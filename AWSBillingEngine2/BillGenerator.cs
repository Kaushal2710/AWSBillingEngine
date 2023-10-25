using AWSBillingEngine2.Domain_model;
using System;
using System.Linq;

namespace AWSBillingEngine2
{
    internal class BillGenerator
    {
        private static void CalculateBill(List<Bill> bills)
        {
            foreach (var bill in bills)
            {
                var totalAmount = new decimal();
                for (var i = 0; i < bill.ResourceType.Count; i++)
                {
                    //bill.TotalBilledTime.Add(new TimeSpan(0, (int)Math.Ceiling(bill.TotalUsedTime[i].TotalHours), 0, 0));
                    bill.EachTotalAmount.Add((int)bill.TotalBilledTime[i].TotalHours * bill.Rate[i]);
                    totalAmount += bill.EachTotalAmount[i];
                }

                bill.TotalAmount = totalAmount;
            }

        }

        public static List<Bill> GenerateBill(List<Customer> customers)
        {
            var bills = new List<Bill>();
            foreach (var customer in customers)
            {
                foreach (var ec2Instance in customer.Ec2Instances)
                {
                    var isSameInstance = false;

                    foreach (var usage in ec2Instance.Usages)
                    {
                        while (usage.UsedFrom.Month <= usage.UsedUntil.Month)
                        {
                            var bill = Bill.FindBill(bills, customer.CustomerName, usage.UsedFrom.Month);
                            var index = 0;

                            if (bill == null)
                            {
                                bills = AddBill(bills, customer, usage, ec2Instance);
                                bill = bills.Last();
                            }
                            else
                            {
                                index = bill.ResourceType.IndexOf(ec2Instance.Ec2InstanceType.Type);

                                if (!isSameInstance)
                                {
                                    if (index != -1)
                                    {
                                        bill.TotalResources[index]++;
                                    }
                                    else
                                    {
                                        bill.ResourceType.Add(ec2Instance.Ec2InstanceType.Type);
                                        bill.Rate.Add(ec2Instance.Ec2InstanceType.Charge);
                                        bill.TotalUsedTime.Add(new TimeSpan());
                                        bill.TotalBilledTime.Add(new TimeSpan());
                                        bill.TotalResources.Add(1);
                                        index = bill.TotalUsedTime.Count - 1;
                                    }
                                }
                            }

                            if (usage.UsedFrom.Month == usage.UsedUntil.Month)
                            {
                                bill.TotalUsedTime[index] += (usage.UsedUntil - usage.UsedFrom);
                                bill.TotalBilledTime[index] += (new TimeSpan(0,
                                    (int)Math.Ceiling((usage.UsedUntil - usage.UsedFrom).TotalHours), 0, 0));
                                break;
                            }

                            var lastDayOfTheMonth = DateTime.DaysInMonth(usage.UsedFrom.Year, usage.UsedFrom.Month);
                            bill.TotalUsedTime[index] += (new DateTime(usage.UsedFrom.Year, usage.UsedFrom.Month,
                                lastDayOfTheMonth, 23, 59, 59) - usage.UsedFrom);
                            bill.TotalBilledTime[index] += (new TimeSpan(0, (int)Math.Ceiling((new DateTime(
                                usage.UsedFrom.Year, usage.UsedFrom.Month,
                                lastDayOfTheMonth, 23, 59, 59) - usage.UsedFrom).TotalHours), 0, 0));

                            usage.UsedFrom = new DateTime(usage.UsedFrom.Year, usage.UsedFrom.Month + 1, 1, 0, 0,
                                0);
                        }

                        isSameInstance = true;
                    }
                }
            }

            CalculateBill(bills);
            return bills;

        }

        private static List<Bill> AddBill(List<Bill> bills, Customer customer, AwsResourceUsage awsResourceUsage,
            Ec2Instance ec2Instance)
        {
            var bill = new Bill( customer.CustomerId, customer.CustomerName)
            {
                BillMonth = awsResourceUsage.UsedFrom.Month,
                BillYear = awsResourceUsage.UsedFrom.Year
            };
            bill.ResourceType.Add(ec2Instance.Ec2InstanceType.Type);
            bill.TotalResources.Add(1);
            bill.Rate.Add(ec2Instance.Ec2InstanceType.Charge);
            bill.TotalUsedTime.Add(new TimeSpan());
            bill.TotalBilledTime.Add(new TimeSpan());
            bills.Add(bill);
            return bills;
        }
    }
}
