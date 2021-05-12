using System;

namespace ExtremeSnowboarding.Script.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MovimentationValueAttribute : Attribute
    {
        public MovimentationValueAttribute()
        {
        }
    }
}
