using System;

namespace HaselOne.Util
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ImageUrlAttribute : Attribute
    {
        public string ImageUrl { get; set; }

        public ImageUrlAttribute(string imageUrl)
        {
            if (!String.IsNullOrEmpty(imageUrl))
                if (imageUrl.Trim() != "")
                    ImageUrl = imageUrl;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PropertiesAttribute : Attribute
    {
        public string ImageUrl { get; set; }

        public bool IgnoreInEdit { get; set; }

        public PropertiesAttribute(string imageUrl, bool ignoreInEdit)
        {
            IgnoreInEdit = ignoreInEdit;
            ImageUrl = imageUrl;
        }

        public PropertiesAttribute(bool ignoreInEdit)
        {
            IgnoreInEdit = ignoreInEdit;
        }
    }
}