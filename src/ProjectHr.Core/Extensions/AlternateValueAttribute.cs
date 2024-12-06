using System;
using System.ComponentModel;
using System.Reflection;

namespace ProjectHr.Extensions;

public class AlternateValueAttribute : Attribute
{
    public string AlternateValue { get; protected set; }

    public AlternateValueAttribute(string value)
    {
        this.AlternateValue = value;
    }
}

public static class IEnumExtensions
{
    public static string GetAlternateValue(this Enum Value)
    {
        Type Type = Value.GetType();

        FieldInfo FieldInfo = Type.GetField(Value.ToString());
        if (Value is null)
            return null;

            AlternateValueAttribute Attribute = FieldInfo.GetCustomAttribute(
                typeof(AlternateValueAttribute)
            ) as AlternateValueAttribute;
        return Attribute.AlternateValue;
    }
    public static string GetDescription(this Enum value)
    {
        FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

        if (fieldInfo != null)
        {
            DescriptionAttribute[] attributes = 
                (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
        }

        return value.ToString();
    }
}