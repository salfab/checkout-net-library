using System;

namespace Checkout.ApiServices.Charges.ResponseModels
{
    public class Refund
    {
        public string Object;
        public decimal Value;
        public string Currency;
        public string Created;
        public string BalanceTransaction;
    }
}