using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using ProjectHr.Entities;

namespace ProjectHr.EntityFrameworkCore.Seed.WorkDate;

public class WorkScheduleSeed
{
    private readonly ProjectHrDbContext _context;

    public WorkScheduleSeed(ProjectHrDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        CreateWorkScheduleSeeds();
    }

    public void CreateWorkScheduleSeeds()
    {
        string filePath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), 
            "WorkDate.json");

        using (StreamReader r = new StreamReader(filePath))
        {

            string json = r.ReadToEnd();
            List<Entities.WorkDate> items = JsonConvert.DeserializeObject<List<Entities.WorkDate>>(json);

            _context.WorkDates.AddRange(items); // Add all work dates

            _context.WorkSchedules.Add(new WorkSchedule
            {
                Name = "Default Work Schedule",
                Dates = items // Associate the work dates with the new work schedule
            });
            _context.SaveChanges();
        }
    }
}