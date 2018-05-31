using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CherryPicker
{
    public class DefaultOverride<T> : BaseDefaulter<T>
    {
        public DefaultValue<T, TSetterType> Set<TSetterType>(Expression<Func<T, TSetterType>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return DefaultImpl(expression);
        }
    }
}
