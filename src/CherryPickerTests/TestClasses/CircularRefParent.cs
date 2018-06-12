using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPickerTests.TestClasses
{
    public class CircularRefParent
    {
        public string Name { get; set; }
        public CircularRefChild Child { get; set; }
    }
}
