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
        private readonly object _missing = new object();
        
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
            var newPropertyDefaults = new Dictionary<string, object>();
            foreach (var defaulterAction in defaulterActions)
            {
                var defaulter = new Defaulter<T>();
                defaulter.OnPropertyNameSet = propertyName =>
                {
                    newPropertyDefaults[propertyName] = _missing;
                };
                defaulter.OnPropertyValueSet = (propertyName, propertyValue) =>
                {
                    newPropertyDefaults[propertyName] = propertyValue;
                };

                defaulterAction(defaulter);
            };

            ReportInvalidProperties(typeof(T), newPropertyDefaults, isDefaultOverride: true);

            return newPropertyDefaults;
        }

        private Dictionary<string, object> GetPropertyDefaults<T>(params Action<DefaultOverride<T>>[] defaultOverrideActions)
        {
            var newPropertyDefaults = new Dictionary<string, object>();
            foreach (var defaultOverrideAction in defaultOverrideActions)
            {
                var defaultOverrider = new DefaultOverride<T>();
                defaultOverrider.OnPropertyNameSet = propertyName =>
                {
                    newPropertyDefaults[propertyName] = _missing;
                };
                defaultOverrider.OnPropertyValueSet = (propertyName, propertyValue) =>
                {
                    newPropertyDefaults[propertyName] = propertyValue;
                };
                defaultOverrideAction(defaultOverrider);
            };

            ReportInvalidProperties(typeof(T), newPropertyDefaults, isDefaultOverride: false);

            return newPropertyDefaults;
        }

        private void ReportInvalidProperties(
            Type type, Dictionary<string, object> propertyDefaults, bool isDefaultOverride)
        {
            var invalidlyOverridenPropertyNamesQuery =
                from propertyDefault in propertyDefaults
                where propertyDefault.Value == _missing
                select propertyDefault.Key;
            var invalidlyOverridenPropertyNames = invalidlyOverridenPropertyNamesQuery.ToList();
            if (invalidlyOverridenPropertyNames.Any())
            {
                throw new Exception(BuildInvalidOverridesExceptionMessage(
                    type, invalidlyOverridenPropertyNames, isDefaultOverride));
            }
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
