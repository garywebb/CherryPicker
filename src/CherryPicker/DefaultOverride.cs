using CherryPicker.Lib;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CherryPicker
{
    public class DefaultOverride<T>
    {
        private readonly Action<string, object> _populatePropertyDefaultsCallback;
        internal string PropertyName { get; private set; }

        public DefaultOverride(Action<string, object> populatePropertyDefaultsCallback)
        {
            _populatePropertyDefaultsCallback = populatePropertyDefaultsCallback;
        }

        public DefaultOverrideValue<T, TSetterType> Set<TSetterType>(Expression<Func<T, TSetterType>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            PropertyName = expression.GetPropertyName();
            return new DefaultOverrideValue<T, TSetterType>(PropertyName, _populatePropertyDefaultsCallback, this);
        }
    }
}
