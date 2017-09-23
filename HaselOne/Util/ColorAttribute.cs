using System;
using System.Drawing;

namespace HaselOne.Util
{
    [System.AttributeUsage(AttributeTargets.Field)]
    public class ColorAttribute : System.Attribute
    {
        public Color Color { get; set; }

        public ColorAttribute(Int32 color)
        {
            Color = Color.FromArgb(color);
        }
    }
}