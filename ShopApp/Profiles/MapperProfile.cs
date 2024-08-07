using AutoMapper;
using ShopApp.Apps.AdminApp.Dtos.CategoryDto;
using ShopApp.Apps.AdminApp.Dtos.ProductDto;
using ShopApp.Entities;

namespace ShopApp.Profiles
{
    public class MapperProfile : Profile
    {
        private readonly IHttpContextAccessor _contextAccessor;


        public MapperProfile(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            var request = _contextAccessor.HttpContext.Request;
            var uriBuilder = new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port.Value);
            var url = uriBuilder.Uri.AbsoluteUri;
            CreateMap<Category, CategoryReturnDto>()
                .ForMember(dest => dest.Image, map => map.MapFrom(src => url + "images/" + src.Image));
            CreateMap<Product, ProductReturnDto>();
            CreateMap<Category, CategoryInProductReturnDto>();
        }
    }
}
