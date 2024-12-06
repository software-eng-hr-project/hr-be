using System.Text.Json.Serialization;
using ProjectHr.Extensions;

namespace ProjectHr.Enums;

public enum MilitaryStatus
{
    [AlternateValue("Yapıldı ")]
    Done= 1,
    [AlternateValue("Yapılmadı")]
    NotDone,
    [AlternateValue("Muaf")]
    Exempt,
    
}