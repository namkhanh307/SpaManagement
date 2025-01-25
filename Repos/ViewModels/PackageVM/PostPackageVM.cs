using Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.PackageVM
{
    public class PostPackageVM
    {
        [Required (ErrorMessage = "Vui lòng nhập tên gói!")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập chú thích!")]
        public string Description { get; set; } = string.Empty;

        public ICollection<PostPackageServiceVM> Services { get; set; } = new List<PostPackageServiceVM>();
    }

    public class PostPackageServiceVM
    {
        public string ServiceId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public EnumPackageService Type { get; set; }
    }
}
