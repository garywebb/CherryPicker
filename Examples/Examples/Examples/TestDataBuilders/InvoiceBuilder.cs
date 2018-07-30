using Examples.BusinessClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples.TestDataBuilders
{
    public class InvoiceBuilder
    {
        private Recipient _recipient = new RecipientBuilder().Build();
        private List<InvoiceLine> _invoiceLines = new List<InvoiceLine>();

        public InvoiceBuilder With(RecipientBuilder recipientBuilder)
        {
            return With(recipientBuilder.Build());
        }

        public InvoiceBuilder With(Recipient recipient)
        {
            _recipient = recipient;
            return this;
        }

        public InvoiceBuilder With(List<InvoiceLineBuilder> invoiceLineBuilders)
        {
            return With(invoiceLineBuilders.Select(builder => builder.Build()).ToList());
        }

        public InvoiceBuilder With(List<InvoiceLine> invoiceLines)
        {
            _invoiceLines = invoiceLines;
            return this;
        }

        public Invoice Build()
        {
            return new Invoice
            {
                Recipient = _recipient,
                InvoiceLines = _invoiceLines
            };
        }
    }
}
