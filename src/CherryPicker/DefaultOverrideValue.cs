using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPicker
{
    public class DefaultOverrideValue<T, TSetterType>
    {
        private readonly string _propertyName;
        private readonly Action<string, object> _onPropertyValueSetCallback;
        private readonly Action<string, Type> _onPropertyValueSetToAutoBuildCallback;
        private readonly DefaultOverride<T> _parent;

        public DefaultOverrideValue(string propertyName, 
            Action<string, object> onPropertyValueSetCallback,
            Action<string, Type> onPropertyValueSetToAutoBuildCallback,
            DefaultOverride<T> parent)
        {
            _propertyName = propertyName;
            _onPropertyValueSetCallback = onPropertyValueSetCallback;
            _onPropertyValueSetToAutoBuildCallback = onPropertyValueSetToAutoBuildCallback;
            _parent = parent;
        }

        public DefaultOverride<T> To(TSetterType defaultValue)
        {
            _onPropertyValueSetCallback(_propertyName, defaultValue);
            return _parent;
        }

        public DefaultOverride<T> ToAutoBuild()
        {
            _onPropertyValueSetToAutoBuildCallback(_propertyName, typeof(TSetterType));
            return _parent;
        }

        public DefaultOverride<T> ToAutoBuild<TConcreteSetterType>()
            where TConcreteSetterType : TSetterType
        {
            _onPropertyValueSetToAutoBuildCallback(_propertyName, typeof(TConcreteSetterType));
            return _parent;
        }
    }
}
