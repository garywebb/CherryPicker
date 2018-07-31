using Examples.BusinessClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.TestDataBuilders
{
    public class AddressBuilder
    {
        private string _firstLine = "221b Baker Street";
        private string _city = "London";
        private PostCode _postCode = new PostCodeBuilder().Build();

        public AddressBuilder With(PostCodeBuilder postCodeBuilder)
        {
            return With(postCodeBuilder.Build());
        }

        public AddressBuilder With(PostCode postCode)
        {
            _postCode = postCode;
            return this;
        }

        public AddressBuilder With(
            string firstLine = null,
            string city = null)
        {
            if (firstLine != null) _firstLine = firstLine;
            if (city != null) _city = city;

            return this;
        }

        public Address Build()
        {
            return new Address
            {
                FirstLine = _firstLine,
                City = _city,
                PostCode = _postCode
            };
        }
    }
}
