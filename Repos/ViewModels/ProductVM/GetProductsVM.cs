namespace Repos.ViewModels.ProductVM
{
    public class GetProductsVM : BaseVM
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string url { get; set; } = string.Empty;


    }
}
