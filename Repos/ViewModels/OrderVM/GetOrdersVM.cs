using Repos.ViewModels.OrderDetailVM;

namespace Repos.ViewModels.OrderVM
{
    public class GetOrdersVM : BaseVM
    {
        public string Name { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public ICollection<GetOrderDetailVM> GetOrderDetailVMs { get; set; } = [];

    }
}
