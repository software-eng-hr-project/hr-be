
using System.Linq;
namespace ProjectHr.EntityFrameworkCore.Seed.JobTitle;

public class JobTitleSeed
{
    private readonly ProjectHrDbContext _context;

    public JobTitleSeed(ProjectHrDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        CreateJobTitleSeeds();
    }

    public void CreateJobTitleSeeds()
    {
        
        Entities.JobTitle seedEntity1 = new Entities.JobTitle
        {
            Id = 1,
            Name = "Front-end",
        };
        
        Entities.JobTitle seedEntity2 = new Entities.JobTitle
        {
            Id = 2,
            Name = "Back-end"

        };
        
        Entities.JobTitle seedEntity3 = new Entities.JobTitle
        {
            Id = 3,
            Name = "Manager",
        };
        
        if (!_context.JobTitles.Any(x => x.Id == seedEntity1.Id))
            _context.JobTitles.Add(seedEntity1);
        // else
        //     _context.Reward.Update(seedEntity1);
        
        if (!_context.JobTitles.Any(x => x.Id == seedEntity2.Id))
            _context.JobTitles.Add(seedEntity2);
        // else
        //     _context.Reward.Update(seedEntity2);
        
        if (!_context.JobTitles.Any(x => x.Id == seedEntity3.Id))
            _context.JobTitles.Add(seedEntity3);

        _context.SaveChanges();
        // else
        //     _context.Reward.Update(seedEntity3);

    }
}