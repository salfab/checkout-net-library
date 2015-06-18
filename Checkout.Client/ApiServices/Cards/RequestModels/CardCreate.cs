using System;
using System.Collections.Generic;

namespace Checkout.ApiServices.Cards.RequestModels
{
    public class CardCreate : CardUpdate
    {
        public string Number { get; set; }
        public string Cvv { get; set; }
        //public CardCreate Card { get; set; }
        //public string CustomerId { get; set; }
    }
}