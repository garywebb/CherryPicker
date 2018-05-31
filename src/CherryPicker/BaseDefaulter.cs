using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CherryPicker
{
    public abstract class BaseDefaulter<T>
    {
        internal string PropertyName { get; private set; }
        internal object PropertyValue { get; set; }

        protected virtual DefaultValue<T, TSetterType> DefaultImpl<TSetterType>(Expression<Func<T, TSetterType>> expression)
        {
            PropertyName = GetPropertyName(expression);
            return new DefaultValue<T, TSetterType>(this);
        }

        private string GetPropertyName<TSettertype>(Expression<Func<T, TSettertype>> expression)
        {
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
                throw new ArgumentException("Not a member access", nameof(expression));

            return memberExpression.Member.Name;
        }
    }
}
