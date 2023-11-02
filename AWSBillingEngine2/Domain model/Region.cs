namespace AWSBillingEngine2.Domain_model;

public class Region
{
    public string RegionName { get; set; }

    public Region(string regionName)
    {
        RegionName = regionName;
    }
}
