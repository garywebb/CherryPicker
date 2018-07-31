using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.TestDataBuilders
{
    public static class DataBuilderFactory
    {
        public static InvoiceBuilder AnInvoice() => new InvoiceBuilder();
        public static RecipientBuilder ARecipient() => new RecipientBuilder();
        public static AddressBuilder AnAddress() => new AddressBuilder();
        public static PostCodeBuilder APostCode() => new PostCodeBuilder();
        public static PoundsShillingsPenceBuilder APrice() => new PoundsShillingsPenceBuilder();
        public static InvoiceLineBuilder AnInvoiceLine() => new InvoiceLineBuilder();
    }
}
