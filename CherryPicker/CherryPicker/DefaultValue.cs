using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPicker
{
    public class DefaultValue<T, TSetterType>
    {
        private BaseDefaulter<T> _defaulter;

        public DefaultValue(BaseDefaulter<T> defaulter)
        {
            _defaulter = defaulter;
        }

        public void To(TSetterType defaultValue)
        {
            _defaulter.PropertyValue = defaultValue;
        }
    }
}
