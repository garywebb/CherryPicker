using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CherryPicker
{
    /// <summary>
    /// 
    /// </summary>
    public class TestDataContainer : ITestDataContainer
    {
        private readonly Dictionary<Type, Dictionary<string, object>> _propertyDefaultsByType;
        private readonly PropertySetterInstancePolicy _propertySetterInstancePolicy;
        
        /// <summary>
        /// Constructor to create a TestDataContainer.
        /// </summary>
        public TestDataContainer()
        {
            _propertyDefaultsByType = new Dictionary<Type, Dictionary<string, object>>();

            //The PropertySetterInstancePolicy is created once and shared among all
            //child instances.
            _propertySetterInstancePolicy = new PropertySetterInstancePolicy();
            
            //The container is created on first instantiation of the TestDataBuilder
            //and shared among all child instances.
            Container = new Container(x =>
            {
                x.Policies.Add(_propertySetterInstancePolicy);
            });
        }

        /// <summary>
        /// Private constructor used to build Child Container.
        /// </summary>
        private TestDataContainer(Dictionary<Type, Dictionary<string, object>> propertyDefaultsByType, 
            PropertySetterInstancePolicy propertySetterInstancePolicy, IContainer container)
        {
            _propertyDefaultsByType = propertyDefaultsByType;
            _propertySetterInstancePolicy = propertySetterInstancePolicy;
            Container = container;
        }

        /// <summary>
        /// StructureMap Container. Use it to configure any non-data related dependencies required when 
        /// building new objects.
        /// </summary>
        public IContainer Container { get; private set; }

        /// <summary>
        /// Creates a child instance which inherits all previous configurations from its parent.
        /// </summary>
        /// <returns>Returns a new TestDataContainer.</returns>
        public TestDataContainer CreateChildInstance()
        {
            return new TestDataContainer(_propertyDefaultsByType.Clone(), _propertySetterInstancePolicy, Container);
        }

        /// <summary>
        /// Configures the default data to be injected into any built instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type being configured.</typeparam>
        /// <param name="defaulterActions">The <typeparamref name="T"/> property defaults.</param>
        /// <returns>The TestDataContainer, used for fluent configuration.</returns>
        public TestDataContainer For<T>(params Action<Defaulter<T>>[] defaulterActions)
        {
            var newPropertyDefaults = GetPropertyDefaults(defaulterActions);
            SetPropertyDefaults<T>(newPropertyDefaults);

            return this;
        }

        /// <summary>
        /// Builds a new <typeparamref name="T"/>, filling it up with the data set up in the
        /// For methods and with the overrides passed in in this method.
        /// </summary>
        /// <typeparam name="T">The type being built.</typeparam>
        /// <param name="defaultOverrideActions">Overrides of default data to be used for this specific instance being built.</param>
        /// <returns>The built and populated object.</returns>
        public T Build<T>(params Action<DefaultOverride<T>>[] defaultOverrideActions)
        {
            if (defaultOverrideActions.Any())
            {
                var tempTestDataBuilder = CreateChildInstance();
                return tempTestDataBuilder.BuildImpl(defaultOverrideActions);
            }

            return BuildImpl(defaultOverrideActions);
        }

        private T BuildImpl<T>(params Action<DefaultOverride<T>>[] defaultOverrideActions)
        {
            if (defaultOverrideActions.Any())
            {
                var newPropertyDefaults = GetPropertyDefaults(defaultOverrideActions);
                SetPropertyDefaults<T>(newPropertyDefaults);
            }

            var typesToRebuild = GetTypesToRebuildFrom(typeof(T));
            //Flush the Container of all cached types to be used in building this object. Needed to ensure new 
            //defaults are picked up.
            foreach (var typeToRebuild in typesToRebuild)
            {
                Container.Configure(x => x.For(typeToRebuild).ClearAll());
            }

            //Set the data builder just before getting the instance to let the property setter instance policy
            //use the latest overrides for this type.
            _propertySetterInstancePolicy.DataBuilder = this;

            return Container.GetInstance<T>();
        }

        internal Dictionary<string, object> GetPropertyDefaults(Type pluginType)
        {
            var success = _propertyDefaultsByType.TryGetValue(pluginType, out var propertyDefaults);
            if (success)
            {
                return propertyDefaults;
            }

            return new Dictionary<string, object>();
        }

        private void SetPropertyDefaults<T>(Dictionary<string, object> newPropertyDefaults)
        {
            var type = typeof(T);
            if (IsNew(type))
            {
                Add(type);
            }
            var existingPropertyDefaults = _propertyDefaultsByType[type];
            existingPropertyDefaults.Merge(newPropertyDefaults);
        }

        private bool IsNew(Type type)
        {
            return !_propertyDefaultsByType.ContainsKey(type);
        }

        private void Add(Type type)
        {
            _propertyDefaultsByType.Add(type, new Dictionary<string, object>());
            Container.Configure(x =>
            {
                x.Policies.SetAllProperties(y => y.TypeMatches(innerType => innerType == type));
            });
        }

        private IEnumerable<Type> GetTypesToRebuildFrom(Type buildType)
        {
            var nonValueTypePropertyInfos = buildType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => !x.PropertyType.GetTypeInfo().IsValueType && x.PropertyType != typeof(String) && x.CanWrite && x.GetSetMethod(false) != null && x.GetSetMethod().GetParameters().Length == 1);

            yield return buildType;
            foreach (var nonValueTypePropertyInfo in nonValueTypePropertyInfos)
            {
                yield return nonValueTypePropertyInfo.PropertyType;
            }
        }

        private Dictionary<string, object> GetPropertyDefaults<T>(params Action<Defaulter<T>>[] defaulterActions)
        {
            var invalidlyOverridenProperties = new List<string>();
            var newPropertyDefaults = new Dictionary<string, object>();
            foreach (var defaulterAction in defaulterActions)
            {
                var wasCallbackCalled = false;
                var defaulter = new Defaulter<T>((propertyName, propertyValue) =>
                {
                    wasCallbackCalled = true;
                    newPropertyDefaults.Add(propertyName, propertyValue);
                });
                defaulterAction(defaulter);

                if (!wasCallbackCalled)
                {
                    invalidlyOverridenProperties.Add(defaulter.PropertyName);
                }
            };

            if (invalidlyOverridenProperties.Any())
            {
                throw new Exception(BuildInvalidOverridesExceptionMessage(
                    typeof(T), invalidlyOverridenProperties, isDefaultOverride: true));
            }

            return newPropertyDefaults;
        }

        private Dictionary<string, object> GetPropertyDefaults<T>(params Action<DefaultOverride<T>>[] defaultOverrideActions)
        {
            var invalidlyOverridenProperties = new List<string>();
            var newPropertyDefaults = new Dictionary<string, object>();
            foreach (var defaultOverrideAction in defaultOverrideActions)
            {
                var wasCallbackCalled = false;
                var defaultOverrider = new DefaultOverride<T>((propertyName, propertyValue) =>
                {
                    wasCallbackCalled = true;
                    newPropertyDefaults.Add(propertyName, propertyValue);
                });
                defaultOverrideAction(defaultOverrider);

                if (!wasCallbackCalled)
                {
                    invalidlyOverridenProperties.Add(defaultOverrider.PropertyName);
                }
            };

            if (invalidlyOverridenProperties.Any())
            {
                throw new Exception(BuildInvalidOverridesExceptionMessage(
                    typeof(T), invalidlyOverridenProperties, isDefaultOverride: true));
            }

            return newPropertyDefaults;
        }

        private string BuildInvalidOverridesExceptionMessage(
            Type type, List<string> invalidlyOverridenPropertyNames, bool isDefaultOverride)
        {
            var stringBuilder = new StringBuilder($"Unable to build {type.Name}.");
            stringBuilder.AppendLine();
            foreach (var invalidlyOverridenPropertyName in invalidlyOverridenPropertyNames)
            {
                stringBuilder.AppendLine($"Property {invalidlyOverridenPropertyName} is missing a value.");
                if (isDefaultOverride)
                {
                    stringBuilder.AppendLine($"Correct usage is container.For<{type.Name}>(x => x.Default(o => o.{invalidlyOverridenPropertyName}).To(<value>));");
                }
                else
                {
                    stringBuilder.AppendLine($"Correct usage is container.Build<{type.Name}>(x => x.Set(o => o.{invalidlyOverridenPropertyName}).To(<value>));");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
