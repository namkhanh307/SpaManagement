using Repos.ViewModels.ServiceVM;
using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.PackageVM
{
    public class PutPackageVM
    {
        [Required (ErrorMessage = "Vui lòng nhập tên package")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập chú thích")]
        public string Description { get; set; } = string.Empty;

        public ICollection<GetServicesVM> PackageServices { get; set; } = new List<GetServicesVM>();
    }
}
