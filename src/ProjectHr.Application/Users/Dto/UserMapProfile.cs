using AutoMapper;
using ProjectHr.Authorization.Users;
using ProjectHr.DataAccess.Dto;
using ProjectHr.Entities;
using ProjectHr.Extensions;

namespace ProjectHr.Users.Dto
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => src.BloodType.GetAlternateValue()))
                .ForMember(dest => dest.DisabilityLevel,
                    opt => opt.MapFrom(src => src.DisabilityLevel.GetAlternateValue()))
                .ForMember(dest => dest.HigherEducationStatus,
                    opt => opt.MapFrom(src => src.HigherEducationStatus.GetAlternateValue()))
                .ForMember(dest => dest.EmploymentType,
                    opt => opt.MapFrom(src => src.EmploymentType.GetAlternateValue()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.GetAlternateValue()))
                .ForMember(dest => dest.MarriedStatus, opt => opt.MapFrom(src => src.MarriedStatus.GetAlternateValue()))
                .ForMember(dest => dest.MilitaryStatus,
                    opt => opt.MapFrom(src => src.MilitaryStatus.GetAlternateValue()));
            CreateMap<UserDto, User>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());

            CreateMap<CreateUserDto, UserDto>().ReverseMap();

            CreateMap<UserOwnUpdateDto, User>();
            CreateMap<UserAllUpdateDto, User>();
            CreateMap<User, GetUserGeneralInfo>()
                .ForMember(dest => dest.EmploymentType,
                    opt => opt.MapFrom(src => src.EmploymentType.GetAlternateValue()));
            CreateMap<User, ResetPasswordMailInput>();
            CreateMap<EmployeeLayoffInfo, EmployeeLayoffInfoWithLayoffNameDto>();
            CreateMap<EmployeeLayoff, EmployeeLayoffDto>();
        }
    }
}