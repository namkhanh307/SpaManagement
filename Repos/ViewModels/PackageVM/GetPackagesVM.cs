using Core.Enum;
using Repos.ViewModels.ServiceVM;

namespace Repos.ViewModels.PackageVM
{
    public class GetPackagesVM : BaseVM
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<GetPackageServiceVM> Services { get; set; } = new List<GetPackageServiceVM>();
    }

    public class GetPackageServiceVM
    {
        public EnumPackageService Type { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceDuration { get; set; } = string.Empty;
        public double ServiceStartPrice { get; set; }
        public double ServiceEndPrice { get; set; }
        public string ServiceDescription { get; set; } = string.Empty;

    }
}