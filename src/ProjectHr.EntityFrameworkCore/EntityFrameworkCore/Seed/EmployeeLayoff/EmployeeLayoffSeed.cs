using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ProjectHr.EntityFrameworkCore.Seed.EmployeeLayoff;

public class EmployeeLayoffSeed
{
    private readonly ProjectHrDbContext _context;

    public EmployeeLayoffSeed(ProjectHrDbContext context)
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
            "EmployeeLayoff.json"); 

        using (StreamReader r = new StreamReader(filePath))
        {
            
            string json = r.ReadToEnd();
            List<Entities.EmployeeLayoff> items = JsonConvert.DeserializeObject<List<Entities.EmployeeLayoff>>(json);
            foreach (var layoff in items)
            {
                if (!_context.EmployeeLayoff.Any(x => x.Id == layoff.Id))
                    _context.EmployeeLayoff.Add(layoff);
            }
        }

        _context.SaveChanges();
    }
}