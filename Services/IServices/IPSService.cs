using Repos.ViewModels.PackageVM;
using Repos.ViewModels;
using Repos.Entities;
using System.Linq.Expressions;

namespace Services.IServices
{
    public interface IPSService
    {
        Task<PagingVM<GetPackagesVM>> GetPackages(int pageNumber = 1, int pageSize = 10);
        Task PostPackage(PostPackageVM model);
        Task PutPackage(string id, PutPackageVM model);
        Task DeletePackage(string packageId);
    }
}
