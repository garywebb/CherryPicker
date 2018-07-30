using CherryPicker;
using Examples.BusinessClasses;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Examples
{
    public class CherryPickerTest : CherryPickerTestBase
    {
        /// <summary>
        /// In this test we build a container for storing all of the default 
        /// property values for each of the objects being used.
        /// 
        /// You'll notice, that we only have to set the default values for the
        /// non-complex properties. The complex types will be built by the 
        /// container for you (with their properties also set).
        /// 
        /// We wouldn't normally do this within the test; rather it should be
        /// done in a base or static class to save repeat initialisations of the
        /// container.
        /// 
        /// But, instead of a separate builder class for each of the objects being
        /// built, and a data builder factory, we can achieve the same result in
        /// just a few lines of code. Awesome!
        /// 
        /// The next step is to move the common, run once only, code to the 
        /// <see cref="CherryPickerTestBase"/> class. See
        /// <see cref="CherryPickerTest.BaseClassCherryPickerTest"/> for the 
        /// next improvement.
        /// </summary>
        [Fact]
        public void EndToEndCherryPickerTest()
        {
            ITestDataContainer testDataContainer = new TestDataContainer();

            testDataContainer
                .For<Recipient>(x => x
                    .Default(r => r.CustomerName).To("Sherlock Holmes"))
                .For<Address>(x => x
                    .Default(a => a.FirstLine).To("221b Baker Street")
                    .Default(a => a.City).To("London"))
                .For<PostCode>(x => x
                    .Default(pc => pc.OutwardCode).To("NW1")
                    .Default(pc => pc.InwardCode).To("3RX"));

            var invoice = testDataContainer.Build<Invoice>(x => x
                .Set(i => i.InvoiceLines).To(
                    new List<InvoiceLine>
                    {
                        testDataContainer.Build<InvoiceLine>(y => y
                            .Set(il => il.Product).To("Deerstalker Hat")
                            .Set(il => il.Cost).To(
                                testDataContainer.Build<PoundsShillingsPence>(z => z
                                    .Set(psp => psp.Pounds).To(0)
                                    .Set(psp => psp.Shillings).To(3)
                                    .Set(psp => psp.Pence).To(10)))),
                        testDataContainer.Build<InvoiceLine>(y => y
                            .Set(il => il.Product).To("Tweed Cape")
                            .Set(il => il.Cost).To(
                                testDataContainer.Build<PoundsShillingsPence>(z => z
                                    .Set(psp => psp.Pounds).To(0)
                                    .Set(psp => psp.Shillings).To(4)
                                    .Set(psp => psp.Pence).To(12)))),
                    }));
        }

        /// <summary>
        /// We have moved the common, run once only, defaults code to the 
        /// <see cref="CherryPickerTestBase"/> class.
        /// 
        /// And also introduced the new A/An methods to wrap the 
        /// <code>testDataContainer.Build<>()</code> calls.
        /// 
        /// Have a look back at <see cref="VanillaCodeTest.VanillaCode"/> to see how 
        /// we've improved constructing fully populated, complex data objects for our tests.
        /// Also see <see cref="CherryPickerTest.LessInliningCherryPickerTest"/> for a 
        /// version with less inlining which you may consider a bit prettier.
        /// 
        /// But wait, there's more! Coming soon to CherryPicker will be a new method
        /// <code>ToNextInSequence()</code>, which will let us create a sequence of
        /// defaults, thus allowing us to build different data objects with each
        /// <code>Build()</code> call.
        /// </summary>
        [Fact]
        public void BaseClassCherryPickerTest()
        {
            var invoice = An<Invoice>(x => x
                .Set(i => i.InvoiceLines).To(
                    new List<InvoiceLine>
                    {
                        An<InvoiceLine>(y => y
                            .Set(il => il.Product).To("Deerstalker Hat")
                            .Set(il => il.Cost).To(
                                A<PoundsShillingsPence>(z => z
                                    .Set(psp => psp.Pounds).To(0)
                                    .Set(psp => psp.Shillings).To(3)
                                    .Set(psp => psp.Pence).To(10)))),
                        An<InvoiceLine>(y => y
                            .Set(il => il.Product).To("Tweed Cape")
                            .Set(il => il.Cost).To(
                                A<PoundsShillingsPence>(z => z
                                    .Set(psp => psp.Pounds).To(0)
                                    .Set(psp => psp.Shillings).To(4)
                                    .Set(psp => psp.Pence).To(12)))),
                    }));
        }

        [Fact]
        public void LessInliningCherryPickerTest()
        {
            var invoiceLine1 = An<InvoiceLine>(x => x
                .Set(il => il.Product).To("Deerstalker Hat")
                .Set(il => il.Cost).To(
                    A<PoundsShillingsPence>(y => y
                        .Set(psp => psp.Pounds).To(0)
                        .Set(psp => psp.Shillings).To(3)
                        .Set(psp => psp.Pence).To(10))));

            var invoiceLine2 = An<InvoiceLine>(x => x
                .Set(il => il.Product).To("Tweed Cape")
                .Set(il => il.Cost).To(
                    A<PoundsShillingsPence>(y => y
                        .Set(psp => psp.Pounds).To(0)
                        .Set(psp => psp.Shillings).To(4)
                        .Set(psp => psp.Pence).To(12))));

            var invoice = An<Invoice>(x => x
                .Set(i => i.InvoiceLines).To(
                    new List<InvoiceLine> { invoiceLine1, invoiceLine2 }));
        }
    }
}
