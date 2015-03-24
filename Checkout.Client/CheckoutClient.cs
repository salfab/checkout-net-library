using Checkout.ApiServices.Cards;
using Checkout.ApiServices.Charges;
using Checkout.ApiServices.Customers;
using Checkout.Infrastructure;
using Checkout.Utilities;
using System;

namespace Checkout
{
    public sealed class CheckoutClient
    {
        private CustomerService _customerService;
        private CardService _cardService;
        private ChargeService _chargeService;

        public ChargeService ChargeService { get { return _chargeService ?? (_chargeService = new ChargeService()); } }
        public CardService CardService { get { return _cardService ?? (_cardService = new CardService()); } }
        public CustomerService CustomerService { get { return _customerService ?? (_customerService = new CustomerService()); } }
      
        public CheckoutClient()
        {
            ContentAdaptor.Setup();
        }
    }
}
