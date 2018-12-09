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
        private StructureMapWrapper _structureMapWrapper;
        
        /// <summary>
        /// Constructor to create a TestDataContainer.
        /// </summary>
        public TestDataContainer()
        {
            _propertyDefaultsByType = new Dictionary<Type, Dictionary<string, object>>();
            _structureMapWrapper = new StructureMapWrapper();
        }

        private TestDataContainer(Dictionary<Type, Dictionary<string, object>> propertyDefaultsByType, 
            StructureMapWrapper structureMapWrapper)
        {
            _propertyDefaultsByType = propertyDefaultsByType;
            _structureMapWrapper = structureMapWrapper;
        }

        /// <summary>
        /// StructureMap Container. Use it to configure any non-data related dependencies required when 
        /// building new objects.
        /// </summary>
        public IContainer Container => _structureMapWrapper.Container;

        /// <summary>
        /// Creates a child instance which inherits all previous configurations from its parent.
        /// </summary>
        /// <returns>Returns a new TestDataContainer.</returns>
        public TestDataContainer CreateChildInstance()
        {
            return new TestDataContainer(_propertyDefaultsByType.Clone(), _structureMapWrapper);
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
            _propertyDefaultsByType.Set<T>(newPropertyDefaults);

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
                var newPropertyDefaults = GetPropertyDefaults(defaultOverrideActions);

                var tempPropertyDefaultsByType = _propertyDefaultsByType.Clone();
                tempPropertyDefaultsByType.Set<T>(newPropertyDefaults);

                return GetInstance<T>(tempPropertyDefaultsByType);
            }

            return GetInstance<T>();
        }

        private T GetInstance<T>(Dictionary<Type, Dictionary<string, object>> propertyDefaultsByType = null)
        {
            if (propertyDefaultsByType == null)
            {
                propertyDefaultsByType = _propertyDefaultsByType;
            }
            if (propertyDefaultsByType.TryGetValue(typeof(T), out var propertyDefaults))
            {
                return _structureMapWrapper.GetInstance<T>(propertyDefaults);
            }
            return _structureMapWrapper.GetInstance<T>();
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
                    typeof(T), invalidlyOverridenProperties, isDefaultOverride: false));
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
