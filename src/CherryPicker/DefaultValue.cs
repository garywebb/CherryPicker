using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPicker
{
    public class DefaultValue<T, TSetterType>
    {
        private Defaulter<T> _defaulter;

        public DefaultValue(Defaulter<T> defaulter)
        {
            _defaulter = defaulter;
        }

        public void To(TSetterType defaultValue)
        {
            _defaulter.PropertyValue = defaultValue;
        }
    }
}
