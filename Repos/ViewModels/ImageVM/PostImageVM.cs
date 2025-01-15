using Microsoft.AspNetCore.Http;

namespace Repos.ViewModels.ImageVM
{
    public class PostImageVM
    {
        public ICollection<IFormFile>? Image { get; set; }
    }
}
