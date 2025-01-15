using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.ImageVM;

namespace Services.Mappers
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, GetImageVM>().ReverseMap();
            CreateMap<Image, PostImageVM>().ReverseMap();
        }
    }
}
