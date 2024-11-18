using System.Collections.Generic;
using Abp.Application.Services.Dto;
using ProjectHr.Authorization.Users;

namespace ProjectHr.Entities;

public class TechStack: EntityDto
{
    public string Name { get; set; }
    public ICollection<User> Users {get; set; }
}