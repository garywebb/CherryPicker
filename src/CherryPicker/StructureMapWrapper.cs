using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CherryPicker
{
    internal class StructureMapWrapper
    {
        private PropertySetterInstancePolicy _propertySetterInstancePolicy;

        public StructureMapWrapper(Func<Dictionary<Type, Dictionary<string, object>>> getPropertyDefaultsByType)
        {
            //The PropertySetterInstancePolicy is created once and shared among all
            //child instances.
            _propertySetterInstancePolicy = new PropertySetterInstancePolicy();

            //The container is created on first instantiation of the TestDataBuilder
            //and shared among all child instances.
            Container = new Container(x =>
            {
                x.Policies.Add(_propertySetterInstancePolicy);

                var propertyDefaultsByType = getPropertyDefaultsByType();
                x.Policies.SetAllProperties(y => y.TypeMatches(type =>
                    propertyDefaultsByType.ContainsKey(type)));
            });
        }

        /// <summary>
        /// StructureMap Container. Use it to configure any non-data related dependencies required when 
        /// building new objects.
        /// </summary>
        public IContainer Container { get; private set; }

        internal T GetInstance<T>(Dictionary<Type, Dictionary<string, object>> propertyDefaultsByType)
        {
            var typesToRebuild = GetTypesToRebuildFrom(typeof(T));
            //Flush the Container of all cached types to be used in building this object. Needed to ensure new 
            //defaults are picked up.
            foreach (var typeToRebuild in typesToRebuild)
            {
                Container.Configure(x => x.For(typeToRebuild).ClearAll());
            }

            //Set the data builder just before getting the instance to let the property setter instance policy
            //use the latest overrides for this type.
            _propertySetterInstancePolicy.PropertyDefaultsByType = propertyDefaultsByType;

            return Container.GetInstance<T>();
        }

        private IEnumerable<Type> GetTypesToRebuildFrom(Type buildType)
        {
            var nonValueTypePropertyInfos = 
                buildType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => 
                           !x.PropertyType.GetTypeInfo().IsValueType && 
                           x.PropertyType != typeof(String) && 
                           x.CanWrite && 
                           x.GetSetMethod(false) != null && 
                           x.GetSetMethod().GetParameters().Length == 1);

            yield return buildType;
            foreach (var nonValueTypePropertyInfo in nonValueTypePropertyInfos)
            {
                yield return nonValueTypePropertyInfo.PropertyType;
            }
        }
    }
}
