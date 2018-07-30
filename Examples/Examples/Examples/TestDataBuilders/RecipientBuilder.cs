using Examples.BusinessClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.TestDataBuilders
{
    public class RecipientBuilder
    {
        private string _customerName = "Sherlock Holmes";
        private Address _address = new AddressBuilder().Build();

        public RecipientBuilder With(AddressBuilder addressBuilder)
        {
            return With(addressBuilder.Build());
        }

        public RecipientBuilder With(Address address)
        {
            _address = address;
            return this;
        }

        public RecipientBuilder With(string customerName)
        {
            _customerName = customerName;
            return this;
        }

        public Recipient Build()
        {
            return new Recipient
            {
                Address = _address,
                CustomerName = _customerName
            };
        }
    }
}
