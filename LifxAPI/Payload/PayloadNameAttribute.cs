using System;

namespace Solarflare.LifxAPI
{
    public class PayloadNameAttribute : Attribute
    {
        public readonly string PropertyName;

        public PayloadNameAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}