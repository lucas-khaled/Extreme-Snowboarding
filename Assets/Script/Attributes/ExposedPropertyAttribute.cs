using System;

namespace ExtremeSnowboarding.Script.Attributes
{
    public class ExposedPropertyAttribute : Attribute
    {
        public string displayName;
        public ExposedPropertyAttribute(string displayName)
        {
            this.displayName = displayName;
        }
    }
}