using ProjectHr.Extensions;

namespace ProjectHr.Enums;

public enum EducationStatus
{
    [AlternateValue("İlkokul")]
    PrimarySchool= 1,
    [AlternateValue("Ortaokul")]
    MiddleSchool,
    [AlternateValue("Lise")]
    HighSchool,
    [AlternateValue("Ön Lisans")]
    AssociateDegree,
    [AlternateValue("Lisans")]
    BachelorsDegree,
    [AlternateValue("Yüksek Lisans")]
    MasterDegree,
    [AlternateValue("Doktora")]
    PhdDegree
    
}
