using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPicker
{
    public class DefaultOverrideValue<T, TSetterType>
    {
        private readonly string _propertyName;
        private readonly Action<string, object> _populatePropertyDefaultsCallback;
        private readonly DefaultOverride<T> _parent;

        public DefaultOverrideValue(string propertyName, Action<string, object> populatePropertyDefaultsCallback, DefaultOverride<T> parent)
        {
            _propertyName = propertyName;
            _populatePropertyDefaultsCallback = populatePropertyDefaultsCallback;
            _parent = parent;
        }

        public DefaultOverride<T> To(TSetterType defaultValue)
        {
            _populatePropertyDefaultsCallback(_propertyName, defaultValue);
            return _parent;
        }
    }
}
