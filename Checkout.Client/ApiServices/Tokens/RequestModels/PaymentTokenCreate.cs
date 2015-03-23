using System;
using System.Collections.Generic;

namespace Checkout.ApiServices.Tokens.RequestModels
{
    public class PaymentTokenCreate
    {
        public int Value { get; set; }
        public string Currency { get; set; }
    }
}
