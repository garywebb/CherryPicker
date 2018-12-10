using CherryPicker.Lib;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CherryPicker
{
    public class DefaultOverride<T>
    {
        internal Action<string> OnPropertyNameSet { get; set; }
        internal Action<string, object> OnPropertyValueSet { get; set; }

        public DefaultOverrideValue<T, TSetterType> Set<TSetterType>(Expression<Func<T, TSetterType>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var propertyName = expression.GetPropertyName();
            OnPropertyNameSet(propertyName);
            return new DefaultOverrideValue<T, TSetterType>(propertyName, OnPropertyValueSet, this);
        }
    }
}
