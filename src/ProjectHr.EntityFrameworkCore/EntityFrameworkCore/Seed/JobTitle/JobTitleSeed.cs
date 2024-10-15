
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

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
        string filePath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), 
            "JobTitles.json"); 

        using (StreamReader r = new StreamReader(filePath))
        {
            
            string json = r.ReadToEnd();
            List<Entities.JobTitle> items = JsonConvert.DeserializeObject<List<Entities.JobTitle>>(json);
            foreach (var jobTitle in items)
            {
                if (!_context.JobTitles.Any(x => x.Id == jobTitle.Id))
                    _context.JobTitles.Add(jobTitle);
            }
        }
        
       

        
        

        _context.SaveChanges();


    }
}