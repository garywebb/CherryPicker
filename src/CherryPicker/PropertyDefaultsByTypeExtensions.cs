using CherryPicker.PropertyValueBuilders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CherryPicker
{
    internal static class PropertyDefaultsByTypeExtensions
    {
        //Reused values, stored at the class level for speed and memory optimisation
        private static readonly Dictionary<string, PropertyValueBuilder> EmptyPropertyDefaults = new Dictionary<string, PropertyValueBuilder>();

        public static Dictionary<Type, Dictionary<string, PropertyValueBuilder>> Clone(this Dictionary<Type, Dictionary<string, PropertyValueBuilder>> propertyDefaultsByTypes)
        {
            var propertyDefaultsByTypesClone = propertyDefaultsByTypes.ToDictionary(
                propertyDefaultsByType => propertyDefaultsByType.Key, 
                propertyDefaultsByType => propertyDefaultsByType.Value.Clone());
            return propertyDefaultsByTypesClone;
        }

        public static Dictionary<string, PropertyValueBuilder> GetValue(this Dictionary<Type, Dictionary<string, PropertyValueBuilder>> propertyDefaultsByType,
            Type propertyDefaultsType)
        {
            var isExisting = propertyDefaultsByType.TryGetValue(propertyDefaultsType, out var propertyDefaults);
            var returnPropertyDefaults = isExisting ? propertyDefaults : EmptyPropertyDefaults;
            return returnPropertyDefaults;
        }

        public static void CombineWith(
            this Dictionary<Type, Dictionary<string, PropertyValueBuilder>> existingPropertyDefaultsByType,
            Type newPropertyDefaultsType, 
            Dictionary<string, PropertyValueBuilder> newPropertyDefaults)
        {
            if (!existingPropertyDefaultsByType.ContainsKey(newPropertyDefaultsType))
            {
                existingPropertyDefaultsByType.Add(newPropertyDefaultsType, newPropertyDefaults.Clone());
            }
            else
            {
                var existingPropertyDefaults = existingPropertyDefaultsByType[newPropertyDefaultsType];
                existingPropertyDefaults.CombineWith(newPropertyDefaults);
            }
        }

        public static void CombineWith(
            this Dictionary<string, PropertyValueBuilder> existingPropertyDefaults,
            Dictionary<string, PropertyValueBuilder> newPropertyDefaults)
        {
            foreach (var newPropertyDefault in newPropertyDefaults)
            {
                existingPropertyDefaults[newPropertyDefault.Key] = newPropertyDefault.Value;
            }
        }

        public static Dictionary<string, PropertyValueBuilder> Clone(this Dictionary<string, PropertyValueBuilder> propertyDefaults)
        {
            var propertyDefaultsClone = propertyDefaults.ToDictionary(
                propertyDefault => propertyDefault.Key,
                propertyDefault => propertyDefault.Value);
            return propertyDefaultsClone;
        }
    }
}
