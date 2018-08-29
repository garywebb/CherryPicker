using System;
using System.Collections.Generic;

namespace CherryPicker
{
    internal static class PropertyDefaultsByTypeExtensions
    {
        public static Dictionary<Type, Dictionary<string, object>> Clone(this Dictionary<Type, Dictionary<string, object>> propertyDefaultsByTypes)
        {
            var propertyDefaultsByTypesClone = new Dictionary<Type, Dictionary<string, object>>();
            foreach (var propertyDefaultsByType in propertyDefaultsByTypes)
            {
                propertyDefaultsByTypesClone.Add(propertyDefaultsByType.Key, new Dictionary<string, object>(propertyDefaultsByType.Value));
            }
            return propertyDefaultsByTypesClone;
        }

        public static void Set<T>(this Dictionary<Type, Dictionary<string, object>> propertyDefaultsByType,
            Dictionary<string, object> newPropertyDefaults)
        {
            var type = typeof(T);
            if (!propertyDefaultsByType.ContainsKey(type))
            {
                propertyDefaultsByType.Add(type, new Dictionary<string, object>());
            }

            var existingPropertyDefaults = propertyDefaultsByType[type];
            existingPropertyDefaults.Merge(newPropertyDefaults);
        }

        public static void Merge(this Dictionary<string, object> currentPropertyDefaults, 
            Dictionary<string, object> newPropertyDefaults)
        {
            foreach (var newPropertyDefault in newPropertyDefaults)
            {
                currentPropertyDefaults.Set(newPropertyDefault.Key, newPropertyDefault.Value);
            }
        }

        private static void Set(this Dictionary<string, object> currentPropertyDefaults, 
            string newDefaultPropertyName, object newDefaultPropertyValue)
        {
            if (!currentPropertyDefaults.ContainsKey(newDefaultPropertyName))
            {
                if (newDefaultPropertyValue != null)
                {
                    currentPropertyDefaults.Add(newDefaultPropertyName, newDefaultPropertyValue);
                }
            }
            else
            {
                if (newDefaultPropertyValue == null)
                {
                    currentPropertyDefaults.Remove(newDefaultPropertyName);
                }
                else
                {
                    currentPropertyDefaults[newDefaultPropertyName] = newDefaultPropertyValue;
                }
            }
        }
    }
}
