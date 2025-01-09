using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.OrderDetailVM
{
    public class PostOrderDetailVM
    {
        [Required (ErrorMessage = "Vui lòng chọn gói!")]
        public required string PackageId { get; set; }

        [Required (ErrorMessage = "Vuii lòng chọn dịch vụ!")]
        public required string ServiceId { get; set; }

        [Required (ErrorMessage = "Vui lòng chọn sản phẩm!")]
        public required string ProductId { get; set; }


        [Required (ErrorMessage = "Vui lòng chọn đơn hàng!")]
        public required string OrderId { get; set; }

        [Required (ErrorMessage = "Vui lòng chọn số lượng!")]
        public int Quantity { get; set; }
    }
}
