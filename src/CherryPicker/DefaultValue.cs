using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPicker
{
    public class DefaultValue<T, TSetterType>
    {
        private readonly string _propertyName;
        private readonly Action<string, object> _onPropertyValueSetCallback;
        private readonly Action<string, Type> _onPropertyValueSetToAutoBuildCallback;
        private readonly Defaulter<T> _parent;

        public DefaultValue(string propertyName, 
            Action<string, object> onPropertyValueSetCallback,
            Action<string, Type> onPropertyValueSetToAutoBuildCallback,
            Defaulter<T> parent)
        {
            _propertyName = propertyName;
            _onPropertyValueSetCallback = onPropertyValueSetCallback;
            _onPropertyValueSetToAutoBuildCallback = onPropertyValueSetToAutoBuildCallback;
            _parent = parent;
        }

        public Defaulter<T> To(TSetterType defaultValue)
        {
            _onPropertyValueSetCallback(_propertyName, defaultValue);
            return _parent;
        }

        public Defaulter<T> ToAutoBuild()
        {
            _onPropertyValueSetToAutoBuildCallback(_propertyName, typeof(TSetterType));
            return _parent;
        }

        public Defaulter<T> ToAutoBuild<TConcreteSetterType>()
            where TConcreteSetterType : TSetterType
        {
            _onPropertyValueSetToAutoBuildCallback(_propertyName, typeof(TConcreteSetterType));
            return _parent;
        }
    }
}
