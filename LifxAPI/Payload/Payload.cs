using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Solarflare.LifxAPI
{
    public abstract class Payload
    {
        public readonly string Name;

        protected readonly Dictionary<PayloadNameAttribute, PropertyInfo> payloadParameterPropertyInfos = new ();
        
        protected Payload()
        {
            Type type = GetType();
            PayloadCollectionAttribute payloadCollectionAttribute = type.GetCustomAttribute<PayloadCollectionAttribute>();
            if (payloadCollectionAttribute == null)
                throw new Exception($"{nameof(Payload)} class must have a {nameof(PayloadCollectionAttribute)} attribute.");

            Name = payloadCollectionAttribute.PayloadName;

            IEnumerable<PropertyInfo> payloadParameterProperties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(pi => pi.GetCustomAttribute<PayloadNameAttribute>() != null);
            
            foreach (PropertyInfo payloadParameterPropertyInfo in payloadParameterProperties)
            {
                PayloadNameAttribute payloadParameterAttribute = payloadParameterPropertyInfo.GetCustomAttribute<PayloadNameAttribute>();
                payloadParameterPropertyInfos[payloadParameterAttribute] = payloadParameterPropertyInfo;
            }
        }

        public override string ToString()
        {
            // Create a string readable by Strapi and send. 
            string payloadRequestString = "{";

            foreach (KeyValuePair<PayloadNameAttribute, PropertyInfo> payloadParameter in payloadParameterPropertyInfos)
            {
                PayloadNameAttribute payloadParameterAttribute = payloadParameter.Key;
                object payloadParameterValue = payloadParameter.Value.GetValue(this);
                payloadRequestString += $@"'{payloadParameterAttribute.PropertyName}': ";
                payloadRequestString += $@"'{payloadParameterValue}',";
            }
            // Remove last comma and space
            payloadRequestString = payloadRequestString.Remove(payloadRequestString.Length - 2, 2);
            
            payloadRequestString += @"'}";
            
            return payloadRequestString;
        }
    }
}