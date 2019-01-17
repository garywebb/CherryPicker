using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPicker.PropertyValueBuilders
{
    internal class AutoBuildPropertyValueBuilder : PropertyValueBuilder
    {
        private readonly Type _propertyValueType;
        private readonly Func<Type, object> _getInstance;

        public AutoBuildPropertyValueBuilder(Type propertyValueType,
            Func<Type, object> getInstance)
        {
            _propertyValueType = propertyValueType;
            _getInstance = getInstance;
        }

        public object Build()
        {
            var newInstance = _getInstance(_propertyValueType);
            return newInstance;
        }
    }
}
