using Examples.BusinessClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.TestDataBuilders
{
    public class PoundsShillingsPenceBuilder
    {
        private int _pounds = 0;
        private int _shillings = 3;
        private int _pence = 10;

        public PoundsShillingsPenceBuilder With(
            int? pounds = null,
            int? shillings = null,
            int? pence = null)
        {
            if (pounds != null) _pounds = pounds.Value;
            if (shillings != null) _shillings = shillings.Value;
            if (pence != null) _pence = pence.Value;

            return this;
        }

        public PoundsShillingsPence Build()
        {
            return new PoundsShillingsPence
            {
                Pounds = _pounds,
                Shillings = _shillings,
                Pence = _pence
            };
        }
    }
}
