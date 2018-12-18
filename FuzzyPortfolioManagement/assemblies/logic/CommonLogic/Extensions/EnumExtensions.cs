using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CommonLogic.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumValue) where T : struct
        {
            Type type = enumValue.GetType();
            if (!type.IsEnum)
                throw new ArgumentException("Parameter must be an enum: ", nameof(enumValue));
            
            MemberInfo[] memberInfos = type.GetMember(enumValue.ToString());
            if (memberInfos.Length > 0)
            {
                object[] attributes = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Any())
                {
                    return ((DescriptionAttribute) attributes[0]).Description;
                }
            }

            return enumValue.ToString();
        }

        public static T ToEnum<T>(this string enumString) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Generic parameter must be an enum: ", nameof(T));

            return (T) Enum.Parse(typeof(T), enumString, true);
        }
    }
}