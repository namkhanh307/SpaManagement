using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.OrderDetailVM
{
    public class PostOrderDetailVM
    {
        [Required]
        public required string PackageId { get; set; }
        [Required]
        public required string ServiceId { get; set; }
        [Required]
        public required string ProductId { get; set; }
        [Required]
        public required string OrderId { get; set; }
        public int Quantity { get; set; }
    }
}
