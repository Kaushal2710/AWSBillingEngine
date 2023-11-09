using AWSBillingEngine2.Domain_model;

namespace AWSBillingEngine2;

public class CustomerMonthlyBills
{
    public Customer Customer { get; set; }
    public List<Bill> MonthlyBills { get; set; }

    public CustomerMonthlyBills(Customer customer)
    {
        Customer = customer;
        MonthlyBills = new List<Bill>();
    }
    public Bill? FindBill(DateTime usedFrom)
    {
        return MonthlyBills.FirstOrDefault(bill => bill.BillMonth == usedFrom.Month /*&& bill.CustomerId == customerId*/ && bill.BillYear == usedFrom.Year);
    }
    public Bill AddBill(AwsResourceUsage awsOnDemandResourceUsage,
        Ec2Instance ec2Instance)
    {
        var bill = new Bill(Customer.CustomerId, Customer.CustomerName)
        {
            BillMonth = awsOnDemandResourceUsage.UsedFrom.Month,
            BillYear = awsOnDemandResourceUsage.UsedFrom.Year
        };

        MonthlyBills.Add(bill);
        return bill;
    }
    public void CalculateBills()
    {
        foreach (var customerBill in MonthlyBills)
        {
            customerBill.TotalAmount = customerBill.BillEntries.Sum(eachTotalAmount => eachTotalAmount.EachTotalAmount);
            customerBill.DiscountAmount = customerBill.BillEntries.Sum(discount => discount.Discount);
            foreach (var billEntry in customerBill.BillEntries)
            {
                billEntry.TotalResources = billEntry.InstanceIds.Count;
            }
        }
    }
}