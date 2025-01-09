using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.PackageVM
{
    public class PostPackageVM
    {
        [Required (ErrorMessage = "Vui lòng nhập tên gói!")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập chú thích!")]
        public string Description { get; set; } = string.Empty;
    }
}
