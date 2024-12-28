using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.ViewModels
{
    public class PagingVM<T> where T : BaseVM
    {
        public IEnumerable<T>? List { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
