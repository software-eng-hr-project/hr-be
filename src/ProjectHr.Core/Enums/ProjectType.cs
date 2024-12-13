using ProjectHr.Extensions;

namespace ProjectHr.Enums;

public enum ProjectType
{
      [AlternateValue("İç Proje ")]
      Internal = 1,
      [AlternateValue("Dış Proje")]
      External 
}