using System;

namespace Solarflare.LifxAPI
{
    public class PayloadCollectionAttribute : Attribute
    {
        public readonly string PayloadName;

        public PayloadCollectionAttribute(string payloadName)
        {
            PayloadName = payloadName;
        }
    }
}