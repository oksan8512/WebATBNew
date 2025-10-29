using AutoMapper;
using WebATB.Areas.Admin.Models.Users;
using WebATB.Data.Entities.Idenity;

namespace WebATB.Mapper;

public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<UserEntity, UserItemVM>()
            .ForMember(dest => dest.RegistrationDate,
                opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Roles,
                opt => opt.Ignore());
    }
}