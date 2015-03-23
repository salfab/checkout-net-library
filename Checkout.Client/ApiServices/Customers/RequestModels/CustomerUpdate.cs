using System;

namespace Checkout.ApiServices.Customers.RequestModels
{
    public class CustomerUpdate : BaseCustomer
    {
        public string CustomerId { get; set; }
    }
}