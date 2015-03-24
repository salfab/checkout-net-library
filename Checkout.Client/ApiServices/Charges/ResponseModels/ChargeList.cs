using System;
using System.Collections.Generic;

namespace Checkout.ApiServices.Charges.ResponseModels
{
    public class ChargeList
    {
        public string Object;
        public int Count;
        public List<Charge> Data;
    }
}