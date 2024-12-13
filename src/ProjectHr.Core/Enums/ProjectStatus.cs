using ProjectHr.Extensions;

namespace ProjectHr.Enums;

public enum ProjectStatus
{
        [AlternateValue("Taslak")]
        Draft = 1,
        [AlternateValue("Başlamadı ")]
        NotStarted,
        [AlternateValue("Devam Ediyor")]
        InProgress,
        [AlternateValue("Tamamlandı")]
        Completed,
        [AlternateValue("İptal Edildi")]
        Cancelled 
}