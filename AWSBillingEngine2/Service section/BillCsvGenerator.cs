using System;
using System.Text;

namespace AWSBillingEngine2;

public class BillCsvGenerator
{
    public static void GenerateBillCsvFile(List<CustomerMonthlyBills> bills)
    {
        foreach (var customerBills in bills)
        {
            foreach (var bill in customerBills.MonthlyBills)
            {
                var csv = new StringBuilder();
                csv.AppendLine(bill.CustomerName);
                var month = new DateTime(2000, bill.BillMonth, 4).ToString("MMMM");
                csv.AppendLine("Bill for month of " + month + " " + bill.BillYear);
                csv.AppendLine("Total Amount: $" + bill.TotalAmount);
                csv.AppendLine("Discount: $" + bill.DiscountAmount);
                csv.AppendLine("Actual Amount: $" + (bill.TotalAmount - bill.DiscountAmount));

                csv.AppendLine(
                    "Resource Type, Total Resouorces, Total Used Time (HH:mm:ss), Total Billed Time (HH:mm:ss), Total Amount, Discount, Actual Amount");
                foreach (var billEntry in bill.BillEntries)
                {
                    {
                        csv.AppendLine(
                            $"{billEntry.Region.RegionName}, " +
                            $"{billEntry.ResourceType}, " +
                            $"{billEntry.TotalResources}, " +
                            $"{(int)billEntry.TotalUsedTime.TotalHours}:{billEntry.TotalUsedTime:m\\:s}, " +
                            $"{(int)billEntry.TotalBilledTime.TotalHours}:{billEntry.TotalBilledTime:m\\:s}, " +
                            $"${billEntry.EachTotalAmount}, " +
                            $"${billEntry.Discount}, " +
                            $"${(billEntry.EachTotalAmount - billEntry.Discount)}");
                    }
                }

                var fileName = bill.CustomerId + "_" + month.ToUpper().Substring(0, 3) + "-" + bill.BillYear + ".csv";
                var fullPath = Path.Combine(@"..\..\..\Bills", fileName);
                if (bill.TotalAmount != 0)
                {
                    File.WriteAllText(fullPath, csv.ToString());

                }
            }
        }
    }
}
