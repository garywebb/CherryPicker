using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPicker.PropertyValueBuilders
{
    internal class SingleValuePropertyValueBuilder : PropertyValueBuilder
    {
        private readonly object _value;

        public SingleValuePropertyValueBuilder(object value)
        {
            _value = value;
        }

        public object Build()
        {
            return _value;
        }
    }
}
