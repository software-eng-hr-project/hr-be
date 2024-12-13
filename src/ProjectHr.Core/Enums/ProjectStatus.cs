using ProjectHr.Extensions;

namespace ProjectHr.Enums;

public enum ProjectStatus
{
        [AlternateValue("Taslak")]
        Draft = 1,
        [AlternateValue("Aktif")]
        InProgress,
        [AlternateValue("Tamamlandı")]
        Completed,
}