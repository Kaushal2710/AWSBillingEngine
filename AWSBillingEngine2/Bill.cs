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
        public Bill(string customerId, string customerName)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            BillEntries = new List<BillEntry?>();
        }
        

        public BillEntry? GetBillEntryByRegionAndType(string instanceType, Region instanceRegion)
        {
            var entry = BillEntries.FirstOrDefault(entry => entry != null && entry.Region.Equals(instanceRegion) && entry.ResourceType.Equals(instanceType));
            return entry;
        }

        public BillEntry AddBillEntry(Region region, string type)
        {
            var billEntry = new BillEntry()
            {
                ResourceType = type,
                Region = region,
                TotalUsedTime = new TimeSpan(),
                TotalBilledTime = new TimeSpan(),
                EachTotalAmount = 0,
                Discount = 0,
                ActualAmount = 0,
                TotalResources = 1
            };
            BillEntries.Add(billEntry);
            return billEntry;
        }
    }
}
