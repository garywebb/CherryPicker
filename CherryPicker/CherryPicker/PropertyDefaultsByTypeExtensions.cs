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

        public static void Merge(this Dictionary<Type, Dictionary<string, object>> currentPropertyDefaultsByType,
            KeyValuePair<Type, Dictionary<string, object>> newPropertyDefaultsForType)
        {
            var newPropertyDefaultsType = newPropertyDefaultsForType.Key;
            if (!currentPropertyDefaultsByType.ContainsKey(newPropertyDefaultsType))
            {
                currentPropertyDefaultsByType.Add(newPropertyDefaultsType, newPropertyDefaultsForType.Value);
            }
            else
            {
                var currentPropertyDefaults = currentPropertyDefaultsByType[newPropertyDefaultsType];
                foreach (var newPropertyDefault in newPropertyDefaultsForType.Value)
                {
                    SetPropertyDefault(currentPropertyDefaults, newPropertyDefault.Key, newPropertyDefault.Value);
                }
            }
        }

        private static void SetPropertyDefault(Dictionary<string, object> propertyDefaults, string propertyName, object propertyValue)
        {
            if (!propertyDefaults.ContainsKey(propertyName))
            {
                propertyDefaults.Add(propertyName, propertyValue);
            }
            else
            {
                propertyDefaults[propertyName] = propertyValue;
            }
        }
    }
}
