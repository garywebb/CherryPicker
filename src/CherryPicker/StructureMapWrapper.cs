using CherryPicker.PropertyValueBuilders;
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

        public StructureMapWrapper()
        {
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
        /// StructureMap Container. Use it to configure any non-data related dependencies required when 
        /// building new objects.
        /// </summary>
        public IContainer Container { get; private set; }

        internal object GetInstance(
             Type propertyDefaultsType, Dictionary<string, PropertyValueBuilder> propertyDefaults)
        {
            //Flush the Container of the cached values used in building this object.
            Container.Configure(x => x.For(propertyDefaultsType).ClearAll());

            //Set the data builder just before getting the instance to let the property setter instance policy
            //use the latest overrides for this type.
            _propertySetterInstancePolicy.SetDefaults(propertyDefaultsType, propertyDefaults);

            return Container.GetInstance(propertyDefaultsType);
        }
    }
}
