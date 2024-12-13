using ProjectHr.Extensions;

namespace ProjectHr.Enums;

public enum Gender
{
    [AlternateValue("Erkek")]
    Male= 1,
    [AlternateValue("Kadın")]
    Female,
    [AlternateValue("Diğer")]
    Other,
}