namespace Repos.ViewModels.SalaryVM
{
    public class GetSalariesVM : BaseVM
    {
        public string UserId { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public double Total { get; set; }
    }
}
