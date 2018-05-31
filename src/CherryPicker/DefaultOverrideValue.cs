using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPicker
{
    public class DefaultOverrideValue<T, TSetterType>
    {
        private DefaultOverride<T> _defaultOverride;

        public DefaultOverrideValue(DefaultOverride<T> defaultOverride)
        {
            _defaultOverride = defaultOverride;
        }

        public void To(TSetterType defaultValue)
        {
            _defaultOverride.PropertyValue = defaultValue;
        }
    }
}
