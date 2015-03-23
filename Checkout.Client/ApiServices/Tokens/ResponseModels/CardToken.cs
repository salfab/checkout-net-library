using Checkout.ApiServices.Cards.ResponseModels;
using System;

namespace Checkout.ApiServices.Tokens.ResponseModels
{
    public class CardToken
    {
        public string Object;
        public string Id;
        public bool LiveMode;
        public DateTime Created;
        public bool Used;
        public string PaymentType;
        public Card Card;
    }
}
