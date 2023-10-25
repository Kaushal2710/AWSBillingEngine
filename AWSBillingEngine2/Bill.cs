namespace AWSBillingEngine2
{
    public class Bill
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int BillMonth { get; set; }
        public int BillYear { get; set; }
        public decimal TotalAmount { get; set; }
        public List<string> ResourceType { get; set; }
        public List<int> TotalResources { get; set; }
        public List<TimeSpan> TotalUsedTime { get; set; }
        public List<TimeSpan> TotalBilledTime { get; set; }
        public List<decimal> Rate { get; set; }
        public List<decimal> EachTotalAmount { get; set; }

        public Bill(string customerId, string customerName)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            ResourceType = new List<string>();
            TotalResources = new List<int>();
            TotalUsedTime = new List<TimeSpan>();
            TotalBilledTime = new List<TimeSpan>();
            Rate = new List<decimal>();
            EachTotalAmount = new List<decimal>();
        }
        public static Bill? FindBill(List<Bill> bills,string customerName, int billMonth)
        {
            return bills.FirstOrDefault(bill => bill.BillMonth == billMonth && bill.CustomerName == customerName);
        }

        public static int GetInstancetypeIndexByType(Bill bill, string instanceType)
        {
            var index = bill.ResourceType.IndexOf(instanceType);
            return index;
        }
    }
    
}
