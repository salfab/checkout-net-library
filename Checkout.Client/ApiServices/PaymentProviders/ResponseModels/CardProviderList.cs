using System;
using System.Collections.Generic;

namespace Checkout.ApiServices.PaymentProviders.ResponseModels
{
    public class CardProviderList
    {
        public string Object;
        public int Count;
        public List<CardProvider> Data;
    }
}