namespace Repos.ViewModels.OrderDetailVM
{
    public class GetOrderDetailVM : BaseVM
    {
        public string PackageName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
