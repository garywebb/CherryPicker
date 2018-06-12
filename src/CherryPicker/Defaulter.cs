using CherryPicker.Lib;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CherryPicker
{
    public class Defaulter<T>
    {
        private readonly Action<string, object> _populatePropertyDefaultsCallback;
        internal string PropertyName { get; private set; }

        public Defaulter(Action<string, object> populatePropertyDefaultsCallback)
        {
            _populatePropertyDefaultsCallback = populatePropertyDefaultsCallback;
        }

        public DefaultValue<T, TSetterType> Default<TSetterType>(Expression<Func<T, TSetterType>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            PropertyName = expression.GetPropertyName();
            return new DefaultValue<T, TSetterType>(PropertyName, _populatePropertyDefaultsCallback, this);
        }
    }
}
