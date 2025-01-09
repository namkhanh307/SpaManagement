using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.TransactionVM
{
    public class PostTransactionVM
    {
        [Required (ErrorMessage = "Vui lòng chọn đơn hàng!")]
        public string OrderId { get; set; } = string.Empty;

        [Required (ErrorMessage = "Vui lòng nhập số lượng!")]
        public double Amount { get; set; }

        public string Note { get; set; } = string.Empty;
    }
}
