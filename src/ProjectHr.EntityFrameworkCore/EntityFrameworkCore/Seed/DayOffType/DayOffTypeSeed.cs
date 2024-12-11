using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ProjectHr.EntityFrameworkCore.Seed.DayOffType;

public class DayOffTypeSeed
{
    private readonly ProjectHrDbContext _context;

    public DayOffTypeSeed(ProjectHrDbContext context)
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
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "EntityFrameworkCore", "Seed", "DayOffType",
            "DayOffTypes.json"); 

        using (StreamReader r = new StreamReader(filePath))
        {
            
            string json = r.ReadToEnd();
            List<Entities.DayOffType> items = JsonConvert.DeserializeObject<List<Entities.DayOffType>>(json);
            foreach (var dayOffType in items)
            {
                if (!_context.DayOffType.Any(x => x.Name == dayOffType.Name))
                    _context.DayOffType.Add(dayOffType);
            }
        }

        _context.SaveChanges();
    }
}