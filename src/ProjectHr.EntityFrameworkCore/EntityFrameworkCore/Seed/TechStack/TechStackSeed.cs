using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ProjectHr.EntityFrameworkCore.Seed.TechStack;

public class TechStackSeed
{
    private readonly ProjectHrDbContext _context;

    public TechStackSeed(ProjectHrDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        CreateTechStackSeeds();
    }

    public void CreateTechStackSeeds()
    {
        string filePath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "EntityFrameworkCore", "Seed", "TechStack",
            "TechStacks.json"); 

        using (StreamReader r = new StreamReader(filePath))
        {
            
            string json = r.ReadToEnd();
            List<Entities.TechStack> items = JsonConvert.DeserializeObject<List<Entities.TechStack>>(json);
            foreach (var techStack in items)
            {
                if (!_context.TechStacks.Any(x => x.Id == techStack.Id))
                    _context.TechStacks.Add(techStack);
            }
        }

        _context.SaveChanges();
    }
}