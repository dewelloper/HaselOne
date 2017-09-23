using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace HaselOne.Util
{
    public static class EnumHelper<T>
    {
        public static string GetEnumDescription(string enumValue)
        {
            var type = typeof(T);
            var memInfo = type.GetMember(enumValue);
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length != 0)
                return ((DescriptionAttribute)attributes[0]).Description;
            return null;
        }

        public static string GetEnumImagePath(string enumValue, string rootPath)
        {
            var type = typeof(T);
            var memInfo = type.GetMember(enumValue);
            var attributes = memInfo[0].GetCustomAttributes(typeof(ImageUrlAttribute), false);

            if (attributes.Length != 0)
            {
                string path = ((ImageUrlAttribute)attributes[0]).ImageUrl;
                return path != null ? Path.Combine(rootPath, path) : null;
            }
            return null;
        }

        public static Color GetEnumColor(string enumValue)
        {
            var memInfo = typeof(T).GetMember(enumValue);
            var attributes = memInfo[0].GetCustomAttributes(typeof(ColorAttribute), false);

            foreach (var item in attributes)
            {
                if (item is ColorAttribute)
                    return (item as ColorAttribute).Color;
            }
            return Color.Empty;
        }

        public static PropertiesAttribute GetEnumCustomAttr(string enumValue)
        {
            var type = typeof(T);
            var memInfo = type.GetMember(enumValue);
            var attributes = memInfo[0].GetCustomAttributes(typeof(PropertiesAttribute), false);
            if (attributes == null)
                throw new ArgumentNullException("PropertiesAttribute");
            return attributes.Length > 0 ? (PropertiesAttribute)attributes[0] : null;
            //return (PropertiesAttribute)attributes[0];
        }

        public static string GetEnumDescriptionText(int enumValue)
        {
            List<string> list = new List<string>();
            foreach (var v in Enum.GetValues(typeof(T)))
            {
                if ((enumValue & Convert.ToInt32(v)) > 0)
                {
                    FieldInfo fi = typeof(T).GetField(v.ToString());
                    DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attributes.Length > 0)
                        list.Add(attributes[0].Description);
                }
            }
            return string.Join(", ", list.ToArray());
        }
    }
}