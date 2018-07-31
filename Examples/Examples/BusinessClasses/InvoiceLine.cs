using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.BusinessClasses
{
    public class InvoiceLine
    {
        public string Product { get; set; }
        public PoundsShillingsPence Cost { get; set; }
    }
}
