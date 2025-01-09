using Repos.ViewModels;
using Repos.ViewModels.SalaryVM;

namespace Services.IServices
{
    public interface ISalaryService
    {
        Task PostAsync(PostSalaryVM model);
    }
}
