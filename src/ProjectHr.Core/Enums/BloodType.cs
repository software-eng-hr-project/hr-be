using System.Text.Json.Serialization;
using ProjectHr.Extensions;

namespace ProjectHr.Enums;

public enum BloodType
{
    [AlternateValue("0 Rh-")]
    ONegative = 1,
    [AlternateValue("0 Rh+")]
    OPositive,
    [AlternateValue("A Rh-")]
    ANegative,
    [AlternateValue("A Rh+")]
    APositive,
    [AlternateValue("B Rh-")]
    BNegative,
    [AlternateValue("B Rh+")]
    BPositive,
    [AlternateValue("AB Rh-")]
    ABNegative,
    [AlternateValue("AB Rh+")]
    ABPositive,
}