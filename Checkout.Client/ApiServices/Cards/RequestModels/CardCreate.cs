using System;
using System.Collections.Generic;

namespace Checkout.ApiServices.Cards.RequestModels
{
    public class CardCreate
    {
        public string CustomerId { get; set; }
        public BaseCardCreate Card { get; set; }
    }
}