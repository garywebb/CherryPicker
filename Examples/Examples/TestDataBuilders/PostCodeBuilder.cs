using Examples.BusinessClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.TestDataBuilders
{
    public class PostCodeBuilder
    {
        private string _outwardCode = "NW1";
        private string _inwardCode = "3RX";

        public PostCodeBuilder With(
            string outwardCode = null,
            string inwardCode = null)
        {
            if (outwardCode != null) _outwardCode = outwardCode;
            if (inwardCode != null) _inwardCode = inwardCode;

            return this;
        }

        public PostCode Build()
        {
            return new PostCode
            {
                OutwardCode = _outwardCode,
                InwardCode = _inwardCode
            };
        }
    }
}
