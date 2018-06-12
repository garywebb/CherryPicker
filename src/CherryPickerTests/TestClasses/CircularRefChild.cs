using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPickerTests.TestClasses
{
    public class CircularRefChild
    {
        public string Name { get; set; }
        public CircularRefParent Parent { get; set; }
    }
}
