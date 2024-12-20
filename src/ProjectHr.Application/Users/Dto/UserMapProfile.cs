﻿using AutoMapper;
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
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());

            CreateMap<CreateUserDto, UserDto>().ReverseMap();

            CreateMap<UserOwnUpdateDto, User>();
            CreateMap<UserAllUpdateDto, User>();
            CreateMap<User, GetUserGeneralInfo>();
            CreateMap<User, ResetPasswordMailInput>();
            CreateMap<EmployeeLayoffInfo, EmployeeLayoffInfoWithLayoffNameDto>();
            CreateMap<EmployeeLayoff, EmployeeLayoffDto>();
            CreateMap<User, UserRolesPageDto>();
        }
    }
}