using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPicker
{
    public class DefaultValue<T, TSetterType>
    {
        private readonly string _propertyName;
        private readonly Action<string, object> _populatePropertyDefaultsCallback;
        private readonly Defaulter<T> _parent;

        public DefaultValue(string propertyName, Action<string, object> populatePropertyDefaultsCallback, Defaulter<T> parent)
        {
            _propertyName = propertyName;
            _populatePropertyDefaultsCallback = populatePropertyDefaultsCallback;
            _parent = parent;
        }

        public Defaulter<T> To(TSetterType defaultValue)
        {
            _populatePropertyDefaultsCallback(_propertyName, defaultValue);
            return _parent;
        }
    }
}
