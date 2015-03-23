using System;
using System.Collections.Generic;

namespace Checkout.ApiServices.Customers.ResponseModels
{
    public class CustomerList
    {
        public string Object;
        public int Count;
        public List<Customer> Data;
        public string Offset;
    }
}