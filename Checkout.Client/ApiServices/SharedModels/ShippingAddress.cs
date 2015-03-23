using System;

namespace Checkout.ApiServices.SharedModels
{
    public class ShippingAddress: Address
    {
        public string RecipientName { get; set; }
    }
}
