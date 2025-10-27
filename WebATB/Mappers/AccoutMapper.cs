using AutoMapper;
using WebATB.Data.Entities.Idenity;
using WebATB.Models.Account;

namespace WebATB.Mappers;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<RegisterViewModel, UserEntity>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email))
            .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.LastName))
            .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.FirstName))
            .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email))
            .ForMember(x => x.Image, opt => opt.Ignore())
            .ForSourceMember(x => x.Image, opt => opt.DoNotValidate())
            .ForSourceMember(x => x.Password, opt => opt.DoNotValidate())
            .ForSourceMember(x => x.PasswordConfirm, opt => opt.DoNotValidate());

        CreateMap<UserEntity, UserLinkViewModel>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => $"{x.LastName} {x.FirstName}"))
            .ForMember(x => x.Image, opt => opt.MapFrom(x => x.Image ?? string.Empty));
    }
}