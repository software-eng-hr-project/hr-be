using ProjectHr.Extensions;

namespace ProjectHr.Enums;

public enum MarriedStatus
{
    [AlternateValue("Bekar")]
    Single= 1,
    [AlternateValue("Evli")]
    Married,
    [AlternateValue("Boşanmış")]
    Divorced,
}