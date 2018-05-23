using System;

namespace CherryPicker
{
    public interface ITestDataContainer
    {
        T Build<T>(params Action<DefaultOverride<T>>[] defaultOverrideActions);
        TestDataContainer CreateChildInstance();
        TestDataContainer For<T>(params Action<Defaulter<T>>[] defaulterActions);
    }
}