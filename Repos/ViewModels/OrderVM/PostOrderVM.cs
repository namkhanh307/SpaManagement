using Repos.ViewModels.OrderDetailVM;

namespace Repos.ViewModels.OrderVM
{
    public class PostOrderVM
    {
        public required string Name { get; set; }
        public required string UserId { get; set; }
        public ICollection<PostOrderDetailVM> OrderDetails { get; set; } = [];
    }
}
