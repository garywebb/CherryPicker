using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CherryPicker.Lib
{
    internal static class ExpressionExtensions
    {
        public static string GetPropertyName<T, TSettertype>(this Expression<Func<T, TSettertype>> expression)
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
