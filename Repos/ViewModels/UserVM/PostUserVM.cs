using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.ViewModels.UserVM
{
    public class PostUserVM
    {
        public string FullName { get; set; } = string.Empty;
        public string RoleFullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
