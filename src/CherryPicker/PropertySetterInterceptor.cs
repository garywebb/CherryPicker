using CherryPicker.PropertyValueBuilders;
using StructureMap;
using StructureMap.Building.Interception;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace CherryPicker
{
    internal class PropertySetterInterceptor : IInterceptor
    {
        private static readonly MethodInfo InflateMethod;
        private static Dictionary<string, PropertyValueBuilder> PropertyDefaults;

        static PropertySetterInterceptor()
        {
            InflateMethod = typeof(PropertySetterInterceptor).GetMethod(nameof(Inflate), BindingFlags.NonPublic | BindingFlags.Static);
        }

        public PropertySetterInterceptor(Type pluginType)
        {
            Accepts = pluginType;
            Returns = pluginType;
        }

        public InterceptorRole Role => InterceptorRole.Activates;

        public Type Accepts { get; private set; }

        public Type Returns { get; private set; }

        public string Description => "CherryPicker Property Setter Interceptor";

        public Expression ToExpression(Policies policies, ParameterExpression context, ParameterExpression variable)
        {
            var inflateCall = Expression.Call(InflateMethod, variable, context);
            return inflateCall;
        }

        private static void Inflate(object obj, IContext context)
        {
            foreach (var propertyDefault in PropertyDefaults)
            {
                var property = obj.GetType().GetProperty(propertyDefault.Key);
                var propertyValue = propertyDefault.Value.Build();
                property.SetValue(obj, propertyValue);
            }
        }

        internal void SetDefaults(Dictionary<string, PropertyValueBuilder> propertyDefaults)
        {
            PropertyDefaults = propertyDefaults;
        }
    }
}
