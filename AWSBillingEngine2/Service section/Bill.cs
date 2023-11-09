using AWSBillingEngine2.Domain_model;

namespace AWSBillingEngine2
{
    public class Bill
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int BillMonth { get; set; }
        public int BillYear { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public List<BillEntry?> BillEntries { get; set; }
        public int LinuxHours { get; set; }
        public int WindowsHours { get; set; }
        public Bill(string customerId, string customerName)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            BillEntries = new List<BillEntry?>();
            LinuxHours = 750;
            WindowsHours = 750;
        }
        

        public BillEntry? GetBillEntryByRegionAndType(Ec2InstanceType ec2InstanceType)
        {
            var entry = BillEntries.FirstOrDefault(predicate: entry => entry != null && entry.Region.Equals(ec2InstanceType.Region) && entry.ResourceType.Equals(ec2InstanceType.Type));
            return entry;
        }

        public BillEntry AddBillEntry(BillEntry billEntry)
        {
            BillEntries.Add(item: billEntry);
            return billEntry;
        }
    }
}
