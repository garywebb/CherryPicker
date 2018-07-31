using Examples.BusinessClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.TestDataBuilders
{
    public class InvoiceLineBuilder
    {
        private string _product = "Deerstalker Hat";
        private PoundsShillingsPence _cost = new PoundsShillingsPenceBuilder().Build();

        public InvoiceLineBuilder With(PoundsShillingsPenceBuilder costBuilder)
        {
            return With(costBuilder.Build());
        }

        public InvoiceLineBuilder With(PoundsShillingsPence cost)
        {
            _cost = cost;
            return this;
        }

        public InvoiceLineBuilder With(
            string product = null)
        {
            if (product != null) _product = product;

            return this;
        }

        public InvoiceLine Build()
        {
            return new InvoiceLine
            {
                Product = _product,
                Cost = _cost
            };
        }
    }
}
