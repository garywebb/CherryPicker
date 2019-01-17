using CherryPicker.Lib;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CherryPicker
{
    public class Defaulter<T>
    {
        internal Action<string> OnPropertyNameSet { get; set; }
        internal Action<string, object> OnPropertyValueSet { get; set; }
        internal Action<string, Type> OnPropertyValueSetToAuto { get; set; }

        public DefaultValue<T, TSetterType> Default<TSetterType>(Expression<Func<T, TSetterType>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var propertyName = expression.GetPropertyName();
            OnPropertyNameSet(propertyName);
            return new DefaultValue<T, TSetterType>(
                propertyName, OnPropertyValueSet, OnPropertyValueSetToAuto, this);
        }
    }
}
