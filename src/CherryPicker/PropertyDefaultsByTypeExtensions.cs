using System;
using System.Collections.Generic;
using System.Linq;

namespace CherryPicker
{
    internal static class PropertyDefaultsByTypeExtensions
    {
        public static Dictionary<Type, Dictionary<string, PropertyValueBuilder>> Clone(this Dictionary<Type, Dictionary<string, PropertyValueBuilder>> propertyDefaultsByTypes)
        {
            var propertyDefaultsByTypesClone = propertyDefaultsByTypes.ToDictionary(
                propertyDefaultsByType => propertyDefaultsByType.Key, 
                propertyDefaultsByType => propertyDefaultsByType.Value.Clone());
            return propertyDefaultsByTypesClone;
        }

        public static void Set<T>(this Dictionary<Type, Dictionary<string, PropertyValueBuilder>> propertyDefaultsByType,
            Dictionary<string, PropertyValueBuilder> newPropertyDefaults)
        {
            var type = typeof(T);
            if (!propertyDefaultsByType.ContainsKey(type))
            {
                propertyDefaultsByType.Add(type, newPropertyDefaults.Clone());
            }
            else
            {
                var existingPropertyDefaults = propertyDefaultsByType[type];
                foreach (var newPropertyDefault in newPropertyDefaults)
                {
                    existingPropertyDefaults[newPropertyDefault.Key] = newPropertyDefault.Value;
                }
            }
        }

        private static Dictionary<string, PropertyValueBuilder> Clone(this Dictionary<string, PropertyValueBuilder> propertyDefaults)
        {
            var propertyDefaultsClone = propertyDefaults.ToDictionary(
                propertyDefault => propertyDefault.Key,
                propertyDefault => propertyDefault.Value);
            return propertyDefaultsClone;
        }
    }
}
