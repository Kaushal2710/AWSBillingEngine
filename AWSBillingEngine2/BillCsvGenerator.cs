using System;
using System.Text;

namespace AWSBillingEngine2;

public class BillCsvGenerator
{
    public static void GenerateBillCsvFile(List<Bill> bills)
    {
        foreach (var bill in bills)
        {
            var csv = new StringBuilder();
            csv.AppendLine(bill.CustomerName);
            var month = new DateTime(2000, bill.BillMonth, 4).ToString("MMMM");
            csv.AppendLine("Bill for month of " + month + " " + bill.BillYear);
            csv.AppendLine("Total Amount: $" + bill.TotalAmount.ToString("0.####"));
            csv.AppendLine("Discount Amount: $" + bill.DiscountAmount.ToString("0.####"));
            csv.AppendLine("Actual Amount: $" + (bill.TotalAmount - bill.DiscountAmount).ToString("0.####"));

            csv.AppendLine(
                "Region,Resource Type,Total Resources,Total Used Time (HH:mm:ss),Total Billed Time (HH:mm:ss),Total Amount,Discount,Actual Amount");
            foreach (var billEntry in bill.BillEntries)
            {
                {
                    csv.AppendLine(
                            $"{billEntry.Region.RegionName}," +
                            $"{billEntry.ResourceType}," +
                            $"{billEntry.TotalResources}," +
                            $"{(int)billEntry.TotalUsedTime.TotalHours}:{billEntry.TotalUsedTime:m\\:s}," +
                            $"{(int)billEntry.TotalBilledTime.TotalHours}:{billEntry.TotalBilledTime:mm\\:ss}," +
                            $"${billEntry.EachTotalAmount.ToString("0.####")}," +
                        $"${billEntry.Discount.ToString("0.####")}," +
                        $"${(billEntry.EachTotalAmount - billEntry.Discount).ToString("0.####")}");
                }
            }

            var fileName = bill.CustomerId.Insert(4,"-") + "_" + month.ToUpper().Substring(0,3) + "-" + bill.BillYear + ".csv";
            var fullPath = Path.Combine(@"..\..\..\Output", fileName);
            if (bill.TotalAmount != 0)
            {
                File.WriteAllText(fullPath, csv.ToString());

            }
        }
    }
}
