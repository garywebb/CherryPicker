using CherryPicker;
using Examples.BusinessClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Examples
{
    /// <summary>
    /// The base class containing the set of default property data for each of the 
    /// data objects being used in the tests.
    /// 
    /// Also contains the helper methods <code>A()</code> and <code>An()</code>
    /// which wrap the building of data objects.
    /// </summary>
    public abstract class CherryPickerTestBase
    {
        protected static ITestDataContainer testDataContainer;

        static CherryPickerTestBase()
        {
            testDataContainer = new TestDataContainer();

            testDataContainer
                .For<Invoice>(x => x
                    .Default(i => i.Recipient).ToAutoBuild())
                .For<Recipient>(x => x
                    .Default(r => r.CustomerName).To("Sherlock Holmes")
                    .Default(r => r.Address).ToAutoBuild())
                .For<Address>(x => x
                    .Default(a => a.FirstLine).To("221b Baker Street")
                    .Default(a => a.City).To("London")
                    .Default(a => a.PostCode).ToAutoBuild())
                .For<PostCode>(x => x
                    .Default(pc => pc.OutwardCode).To("NW1")
                    .Default(pc => pc.InwardCode).To("3RX"));
        }

        protected virtual T A<T>(params Action<DefaultOverride<T>>[] defaultOverrides)
        {
            return testDataContainer.Build(defaultOverrides);
        }

        protected virtual T An<T>(params Action<DefaultOverride<T>>[] defaultOverrides)
        {
            return testDataContainer.Build(defaultOverrides);
        }
    }
}
