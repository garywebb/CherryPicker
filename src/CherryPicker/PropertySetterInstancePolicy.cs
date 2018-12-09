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
        private Dictionary<string, object> _propertyDefaults = new Dictionary<string, object>();

        internal void SetDefaults<T>(Dictionary<string, object> propertyDefaults)
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
                return;
            }

            foreach (var propertyDefault in _propertyDefaults)
            {
                var property = instance.SettableProperties().FirstOrDefault(prop => prop.Name == propertyDefault.Key);
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                instance.Dependencies.Add(property.Name, propertyType, propertyDefault.Value);
            }
        }
    }
}
