using ProjectHr.Extensions;

namespace ProjectHr.Enums;

public enum DisabilityLevel
{
    [AlternateValue("Yok")]
    None= 1,
    [AlternateValue("Birinci Derece")]
    FirstDegree,
    [AlternateValue("İkinci Derece")]
    SecondDegree,
    [AlternateValue(" Üçüncü Derece")]
    ThirdDegree,

}