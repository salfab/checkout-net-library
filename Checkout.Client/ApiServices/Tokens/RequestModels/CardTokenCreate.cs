using Checkout.ApiServices.Cards.RequestModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.ApiServices.Tokens.RequestModels
{
    public class CardTokenCreate
    {
        public BaseCardCreate Card { get; set; }
    }
}
