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
