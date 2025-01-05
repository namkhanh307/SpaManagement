using Repos.ViewModels.SalaryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface ISalaryService
    {
        Task PostAsync(PostSalaryVM model);
    }
}
