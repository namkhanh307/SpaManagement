using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.ViewModels.PayRateVM
{
    public class GetPayRatesVM : BaseVM
    {
        public string UserId { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public double Amount { get; set; }
    }
}
