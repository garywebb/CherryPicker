using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.BusinessClasses
{
    public class Invoice
    {
        public Recipient Recipient { get; set; }
        public List<InvoiceLine> InvoiceLines { get; set; }
    }
}
