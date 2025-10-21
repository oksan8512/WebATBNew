using AutoMapper;
using WebATB.Areas.Admin.Models.Users;
using WebATB.Data.Entities.Idenity;

namespace WebATB.Areas.Admin.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UserEntity, UserItemVM>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName}"))

            .ForMember(x => x.Image,
                opt => opt.MapFrom(x => string.IsNullOrEmpty(x.Image) ? $"/images/noimage.png" : $"/images/200_{x.Image}"))

            .ForMember(dest => dest.Roles,
                opt => opt.MapFrom(src => src.UserRoles!.Select(ur => ur.Role.Name).ToList()));
    }
}