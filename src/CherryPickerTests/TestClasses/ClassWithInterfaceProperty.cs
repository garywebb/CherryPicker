using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPickerTests.TestClasses
{
    public class ClassWithInterfaceProperty
    {
        public AnInterface AnInterface { get; set; }
    }

    public interface AnInterface
    {
        int AnInt { get; set; }
    }

    public class AnInterfaceImplementation : AnInterface
    {
        public int AnInt { get; set; }
    }
}
