using CherryPicker.PropertyValueBuilders;
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
        private Type _propertyDefaultsType;
        private Dictionary<string, PropertyValueBuilder> _propertyDefaults = new Dictionary<string, PropertyValueBuilder>();

        internal void SetDefaults<T>(Dictionary<string, PropertyValueBuilder> propertyDefaults)
        {
            _propertyDefaults.Clear();
            foreach (var propertyDefault in propertyDefaults)
            {
                _propertyDefaults.Add(propertyDefault.Key, propertyDefault.Value);
            }
            _propertyDefaultsType = typeof(T);
        }

        protected override void apply(Type pluginType, IConfiguredInstance instance)
        {
            if (_propertyDefaultsType != pluginType)
            {
                throw new ArgumentException($"Unexpected type being built. Expected: {_propertyDefaultsType.Name}, but instead received: {pluginType.Name}. This is an issue with CherryPicker, please raise an issue with recreatable steps in order for it to be fixed. Thank you!", nameof(pluginType));
            }

            foreach (var propertyDefault in _propertyDefaults)
            {
                var propertyValue = propertyDefault.Value.Build();
                //PropertyValue can be null when the user specifies the property value as null
                //StructureMap throws an exception when a null value is passed into its Dependencies
                if (propertyValue == null)
                {
                    continue;
                }

                var property = instance.SettableProperties().FirstOrDefault(prop => prop.Name == propertyDefault.Key);
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                instance.Dependencies.Add(property.Name, propertyType, propertyValue);
            }
        }
    }
}
