using AutoMapper;
using WebATB.Data.Entities;
using WebATB.Models.Product;

namespace WebATB.Mappers;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<ProductCreateModel, ProductEntity>();

        CreateMap<ProductEntity, ProductItemModel>()
            .ForMember(x => x.ProductImages,
                opt => opt.MapFrom(x => x.ProductImages!
                    .OrderBy(pi => pi.Priority)
                    .Select(pi => $"/images/200_{pi.Name}").ToList()))
            .ForMember(opt => opt.CategoryName, prop => prop.MapFrom(x => x.Category!.Name));


        CreateMap<ProductEntity, ProductUpdateModel>()
            .ForMember(x => x.Images,
                opt => opt.MapFrom(x => x.ProductImages!
                .OrderBy(pi => pi.Priority)
                .Select(pi => $"/images/1200_{pi.Name}").ToList()));

        CreateMap<ProductUpdateModel, ProductEntity>()
            .ForMember(x => x.ProductImages, opt => opt.Ignore())
            .ForMember(x => x.Category, opt => opt.Ignore());
    }
}