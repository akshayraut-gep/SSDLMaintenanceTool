namespace SSDLMaintenanceTool.Models
{
    public class Domain
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string DatabaseName { get; set; }
        public bool IsChecked { get; set; }
        public string BuyerPartnerCode { get; internal set; }

        public bool IsSubscriptionExists { get; set; }

        public bool IsSubscriptionLock { get; set; }
    }
}
