using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using ProjectHr.Entities;
using ProjectHr.JobTitles.Dto;


namespace ProjectHr.JobTitles;

[Route("/api/job-title")]
public class JobTitleAppService: ProjectHrAppServiceBase
{
    private readonly IRepository<JobTitle, int> _jobTitleRepository;

    public JobTitleAppService(IRepository<JobTitle, int> jobTitleRepository)
    {
        _jobTitleRepository = jobTitleRepository;
    }
    
    [HttpGet]
    public async Task<List<JobTitleDto>> GetAll()
    {
        var jobTitles = _jobTitleRepository.GetAllList();
        
        var jobTitleDtos = ObjectMapper.Map<List<JobTitleDto>>(jobTitles);
        return jobTitleDtos;
    }
}