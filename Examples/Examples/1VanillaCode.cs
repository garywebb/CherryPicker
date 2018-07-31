using Examples.BusinessClasses;
using System;
using System.Collections.Generic;
using Xunit;

namespace Examples
{
    public class VanillaCodeTest
    {
        /// <summary>
        /// Building an object from scratch with all of its various properties
        /// and children's properties set can take quite a few lines of code.
        /// 
        /// By introducing the Test Data Builder pattern, we can reduce the 
        /// number of lines of code, make the code more readable and draw
        /// the reader's attention to the lines of code most pertinent for
        /// this test. See <see cref="TestDataBuilderTest"/> for the first steps
        /// we can take to improve this code.
        /// </summary>
        [Fact]
        public void VanillaCode()
        {
            var invoice = new Invoice
            {
                Recipient = new Recipient
                {
                    CustomerName = "Sherlock Holmes",
                    Address = new Address
                    {
                        FirstLine = "221b Baker Street",
                        City = "London",
                        PostCode = new PostCode { OutwardCode = "NW1", InwardCode = "3RX" }
                    }
                },
                InvoiceLines = new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        Product = "Deerstalker Hat",
                        Cost = new PoundsShillingsPence { Pounds = 0, Shillings = 3, Pence = 10 }
                    },
                    new InvoiceLine
                    {
                        Product = "Tweed Cape",
                        Cost = new PoundsShillingsPence { Pounds = 0, Shillings = 4, Pence = 12 }
                    }
                }
            };
        }
    }
}
