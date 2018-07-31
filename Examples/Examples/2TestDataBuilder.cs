using Examples.BusinessClasses;
using Examples.TestDataBuilders;
using static Examples.TestDataBuilders.DataBuilderFactory;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Examples
{
    public class TestDataBuilderTest
    {
        /// <summary>
        /// Each test data builder comes with a Build method to build a new
        /// object instance and usually has some accompanying With methods
        /// to provide values for each of the built object's properties.
        /// 
        /// This code is an improvement on the vanilla code example, but has
        /// a number of Build() method calls distracting us and adding extra
        /// lines of code to the test.
        /// 
        /// Adding an overload of the With methods that accepts a data builder
        /// should help improve this. See 
        /// <see cref="TestDataBuilderTest.NoBuildTestDataBuilder"/> for the
        /// next improvement.
        /// </summary>
        [Fact]
        public void SimpleTestDataBuilder()
        {
            var invoice = new InvoiceBuilder()
                .With(new RecipientBuilder()
                    .With(customerName: "Sherlock Holmes")
                    .With(new AddressBuilder()
                        .With(firstLine: "221b Baker Street", city: "London")
                        .With(new PostCodeBuilder()
                            .With(outwardCode: "NW1", inwardCode: "3RX")
                            .Build())
                        .Build())
                    .Build())
                .With(new List<InvoiceLine>
                {
                    new InvoiceLineBuilder()
                        .With(product: "Deerstalker Hat")
                        .With(new PoundsShillingsPenceBuilder()
                            .With(pounds: 0, shillings: 3, pence: 10)
                            .Build())
                        .Build(),
                    new InvoiceLineBuilder()
                        .With(product: "Tweed Cape")
                        .With(new PoundsShillingsPenceBuilder()
                            .With(pounds: 0, shillings: 4, pence: 12)
                            .Build())
                        .Build(),
                })
                .Build();
        }

        /// <summary>
        /// Things are looking better. Most of the Build calls are hidden within
        /// the With methods now. There are still a couple left to build the 
        /// InvoiceLines in the List<InvoiceLine>, but that is acceptable.
        /// 
        /// The next step is to hide the <code>new Builder()</code> calls
        /// using a Factory. This is just a cosmetic improvement, but further
        /// draws attention to the test data and away from the plumbing code
        /// used to build the Invoice object. See 
        /// <see cref="TestDataBuilderTest.FactoryTestDataBuilder"/> for the
        /// next improvement.
        /// </summary>
        [Fact]
        public void NoBuildTestDataBuilder()
        {
            var invoice = new InvoiceBuilder()
                .With(new RecipientBuilder()
                    .With(customerName: "Sherlock Holmes")
                    .With(new AddressBuilder()
                        .With(firstLine: "221b Baker Street", city: "London")
                        .With(new PostCodeBuilder()
                            .With(outwardCode: "NW1", inwardCode: "3RX"))))
                .With(new List<InvoiceLine>
                {
                    new InvoiceLineBuilder()
                        .With(product: "Deerstalker Hat")
                        .With(new PoundsShillingsPenceBuilder()
                            .With(pounds: 0, shillings: 3, pence: 10))
                        .Build(),
                    new InvoiceLineBuilder()
                        .With(product: "Tweed Cape")
                        .With(new PoundsShillingsPenceBuilder()
                            .With(pounds: 0, shillings: 4, pence: 12))
                        .Build(),
                })
                .Build();
        }

        /// <summary>
        /// The code now shows the intent of the test far more now, focusing 
        /// on the business concepts rather than the code used to achieve the 
        /// business requirements.
        /// 
        /// We can improve this even further. In this test, the processing of 
        /// the invoice lines is being tested, not who the recipient of the 
        /// invoice is.
        /// 
        /// So in the next test we provide defaults for all of the various 
        /// object's properties. See 
        /// <see cref="TestDataBuilderTest.DefaultsTestDataBuilder"/> for the
        /// next improvement.
        /// </summary>
        [Fact]
        public void FactoryTestDataBuilder()
        {
            var invoice = AnInvoice()
                .With(ARecipient()
                    .With(customerName: "Sherlock Holmes")
                    .With(AnAddress()
                        .With(firstLine: "221b Baker Street", city: "London")
                        .With(APostCode()
                            .With(outwardCode: "NW1", inwardCode: "3RX"))))
                .With(new List<InvoiceLine>
                {
                    AnInvoiceLine()
                        .With(product: "Deerstalker Hat")
                        .With(APrice()
                            .With(pounds: 0, shillings: 3, pence: 10))
                        .Build(),
                    AnInvoiceLine()
                        .With(product: "Tweed Cape")
                        .With(APrice()
                            .With(pounds: 0, shillings: 4, pence: 12))
                        .Build(),
                })
                .Build();
        }

        /// <summary>
        /// We have defaulted the recipient's properties to Sherlock Holmes and 
        /// his address to 221b Baker Street. These defaults have been set in each
        /// of the test data builders.
        /// 
        /// So now, we don't have to define a recipient within the test code, but
        /// one will be built for us within the test data builder code.
        /// 
        /// This is looking like a very focused test which exposes the data pertinent
        /// to the test, whilst hiding the rest of the code needed to build a fully
        /// populated valid Invoice.
        /// 
        /// However, writing test data builders for each of our data objects is a lot of
        /// repetitive work. If only there was a library out there to do all this for us!
        /// 
        /// Let's do the same improvements, only this time using CherryPicker.
        /// <see cref="CherryPickerTest"/>
        /// </summary>
        [Fact]
        public void DefaultsTestDataBuilder()
        {
            var invoice = AnInvoice()
                .With(new List<InvoiceLine>
                {
                    AnInvoiceLine()
                        .With(product: "Deerstalker Hat")
                        .With(APrice()
                            .With(pounds: 0, shillings: 3, pence: 10))
                        .Build(),
                    AnInvoiceLine()
                        .With(product: "Tweed Cape")
                        .With(APrice()
                            .With(pounds: 0, shillings: 4, pence: 12))
                        .Build(),
                })
                .Build();
        }
    }
}
