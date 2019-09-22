using CherryPicker.PropertyValueBuilders;
using StructureMap.Building.Interception;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;

namespace CherryPicker
{
    internal class CherryPickerInterceptionPolicy : IInterceptorPolicy
    {
        private Dictionary<string, PropertyValueBuilder> _propertyDefaults;
        private readonly Dictionary<Type, PropertySetterInterceptor> _interceptors;

        public CherryPickerInterceptionPolicy()
        {
            _interceptors = new Dictionary<Type, PropertySetterInterceptor>();
        }

        public string Description => "CherryPicker Interception Policy";

        public IEnumerable<IInterceptor> DetermineInterceptors(Type pluginType, Instance instance)
        {
            if (_interceptors.ContainsKey(pluginType))
            {
                yield return _interceptors[pluginType];
            }
            else
            {
                var propertySetterInterceptor = new PropertySetterInterceptor(pluginType);
                propertySetterInterceptor.SetDefaults(_propertyDefaults);
                _interceptors.Add(pluginType, propertySetterInterceptor);

                yield return propertySetterInterceptor;
            }
        }

        internal void SetDefaults(Type propertyDefaultsType, Dictionary<string, PropertyValueBuilder> propertyDefaults)
        {
            if (_interceptors.ContainsKey(propertyDefaultsType))
            {
                var propertySetterInterceptor = _interceptors[propertyDefaultsType];
                propertySetterInterceptor.SetDefaults(propertyDefaults);
            }
            else
            {
                _propertyDefaults = propertyDefaults;
            }
        }
    }
}
