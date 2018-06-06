using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPicker
{
    public class DefaultValue<T, TSetterType>
    {
        private readonly string _propertyName;
        private readonly Action<string, object> _populatePropertyDefaultsCallback;

        public DefaultValue(string propertyName, Action<string, object> populatePropertyDefaultsCallback)
        {
            _propertyName = propertyName;
            _populatePropertyDefaultsCallback = populatePropertyDefaultsCallback;
        }

        public void To(TSetterType defaultValue)
        {
            _populatePropertyDefaultsCallback(_propertyName, defaultValue);
        }
    }
}
