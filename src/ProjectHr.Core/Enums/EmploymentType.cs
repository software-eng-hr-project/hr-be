using System.Text.Json.Serialization;
using ProjectHr.Extensions;

namespace ProjectHr.Enums;

public enum EmploymentType
{
    [AlternateValue("Tam Zamanlı")]
    FullTime = 1,
    [AlternateValue("Yarı Zamanlı")]
    PartTime
}