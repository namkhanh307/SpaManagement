using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.ViewModels
{
    public class PagingVM<T> where T : class
    {
        public IEnumerable<T> List = new List<T>();
        public int PageSize;
        public int PageNumber;
        public int TotalPages;
    }
}
