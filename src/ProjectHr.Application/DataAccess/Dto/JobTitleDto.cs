using Abp.Application.Services.Dto;

namespace ProjectHr.DataAccess.Dto;

public class JobTitleDto : EntityDto
{
    public string Name { get; set; }
}