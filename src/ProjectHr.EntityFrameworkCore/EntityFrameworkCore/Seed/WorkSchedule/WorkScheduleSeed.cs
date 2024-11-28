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
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "EntityFrameworkCore", "Seed", "WorkSchedule",
            "WorkDate.json");

        using (StreamReader r = new StreamReader(filePath))
        {
            string json = r.ReadToEnd();
            List<Entities.WorkDate> items = JsonConvert.DeserializeObject<List<Entities.WorkDate>>(json);


            foreach (var item in items)
            {
                if (!_context.WorkDates.Any(x => x.DayOfTheWeek == item.DayOfTheWeek))
                    _context.WorkDates.Add(item);
            }

            if (!_context.WorkSchedules.Any(x => x.Name == "Default Work Schedule"))
                _context.WorkSchedules.Add(new WorkSchedule
                {
                    Name = "Default Work Schedule",
                    Dates = items // Associate the work dates with the new work schedule
                });
            _context.SaveChanges();
        }
    }
}