using CherryPicker.PropertyValueBuilders;
using StructureMap;
using StructureMap.Building.Interception;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CherryPicker
{
    internal class StructureMapWrapper
    {
        private CherryPickerInterceptionPolicy _interceptionPolicy;

        public StructureMapWrapper()
        {
            _interceptionPolicy = new CherryPickerInterceptionPolicy();

            //The container is created on first instantiation of the TestDataBuilder
            //and shared among all child instances.
            Container = new Container(x =>
            {
                x.Policies.Interceptors(_interceptionPolicy);
            });
        }

        /// <summary>
        /// StructureMap Container. Use it to configure any non-data related dependencies required when 
        /// building new objects.
        /// </summary>
        public IContainer Container { get; private set; }

        internal object GetInstance(
             Type propertyDefaultsType, Dictionary<string, PropertyValueBuilder> propertyDefaults)
        {
            //Set the data builder just before getting the instance to let the property setter instance policy
            //use the latest overrides for this type.
            _interceptionPolicy.SetDefaults(propertyDefaultsType, propertyDefaults);

            var instance = Container.GetInstance(propertyDefaultsType);
            return instance;
        }
    }
}
