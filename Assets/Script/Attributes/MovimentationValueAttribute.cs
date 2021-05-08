using System;

namespace Script.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MovimentationValueAttribute : Attribute
    {
        public MovimentationValueAttribute()
        {
        }
    }
}
