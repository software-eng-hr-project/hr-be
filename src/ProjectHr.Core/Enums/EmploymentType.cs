using System.Text.Json.Serialization;

namespace ProjectHr.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmploymentType
{
    FullTime = 1,
    PartTime = 2,
}