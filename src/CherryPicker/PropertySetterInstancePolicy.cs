using StructureMap;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CherryPicker
{
    internal class PropertySetterInstancePolicy : ConfiguredInstancePolicy
    {
        internal TestDataContainer DataBuilder { get; set; }

        protected override void apply(Type pluginType, IConfiguredInstance instance)
        {
            var propertyDefaults = DataBuilder.GetPropertyDefaults(pluginType);
            foreach (var propertyDefault in propertyDefaults)
            {
                var property = instance.SettableProperties().FirstOrDefault(prop => prop.Name == propertyDefault.Key);
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                instance.Dependencies.Add(property.Name, propertyType, propertyDefault.Value);
            }
        }
    }
}
