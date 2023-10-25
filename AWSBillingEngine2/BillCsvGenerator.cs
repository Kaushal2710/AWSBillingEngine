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
            csv.AppendLine("Bill for the month of " + month + " " + bill.BillYear);
            csv.AppendLine("Total Amount: $" + bill.TotalAmount);

            csv.AppendLine(
                "Resource Type,Total Resources,Total Used Time (HH:mm:ss),Total Billed Time (HH:mm:ss),Rate (Per Hour),Total Amount");
            for (var i = 0; i < bill.ResourceType.Count; i++)
            {
                {
                    csv.AppendLine(
                        $"{bill.ResourceType[i]}," +
                        $"{bill.TotalResources[i]}," +
                        $"{bill.TotalUsedTime[i].Days*24 + bill.TotalUsedTime[i].Hours}:{bill.TotalUsedTime[i].Minutes}:{bill.TotalUsedTime[i].Seconds}," +
                        $"{bill.TotalBilledTime[i].Days * 24 + bill.TotalBilledTime[i].Hours}:{bill.TotalBilledTime[i].Minutes}:{bill.TotalBilledTime[i].Seconds}," +
                        $"${bill.Rate[i]}," +
                        $"${bill.EachTotalAmount[i]}");
                }
            }

            var fileName = bill.CustomerId.Remove(4,1) + "_" + month.ToUpper().Substring(0,3) + "-" + bill.BillYear + ".csv";
            var fullPath = Path.Combine(@"..\..\..\..\bills", fileName);
            File.WriteAllText(fullPath, csv.ToString());
        }
    }
}
