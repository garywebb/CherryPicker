using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPicker
{
    internal class EmptyPropertyValueBuilder : PropertyValueBuilder
    {
        public static PropertyValueBuilder Instance { get; private set; } = new EmptyPropertyValueBuilder();

        public object Build()
        {
            throw new NotImplementedException();
        }
    }
}
