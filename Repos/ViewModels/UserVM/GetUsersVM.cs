namespace Repos.ViewModels.UserVM
{
    public class GetUsersVM : BaseVM
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string RoleFullName { get; set; } = string.Empty;
    }
}
