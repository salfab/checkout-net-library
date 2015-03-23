using System;

namespace Checkout.ApiServices.Tokens.ResponseModels
{
    public class PaymentToken
    {
        public string Object;
        public string Id;
        public decimal Value;
        public string Currency;
    }
}