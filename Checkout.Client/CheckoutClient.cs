using Checkout.ApiServices.Cards;
using Checkout.ApiServices.Charges;
using Checkout.ApiServices.Customers;
using Checkout.ApiServices.PaymentProviders;
using Checkout.ApiServices.Tokens;
using Checkout.Infrastructure;
using Checkout.Utilities;
using System;

namespace Checkout
{
    public sealed class 
        CheckoutClient
    {
        private PaymentProviderService _paymentProviderService;
        private TokenService _tokenService;
        private CustomerService _customerService;
        private CardService _cardService;
        private ChargeService _chargeService;

        public ChargeService ChargeService { get { return _chargeService ?? (_chargeService = new ChargeService()); } }
        public CardService CardService { get { return _cardService ?? (_cardService = new CardService()); } }
        public CustomerService CustomerService { get { return _customerService ?? (_customerService = new CustomerService()); } }
        public PaymentProviderService PaymentProviderService { get { return _paymentProviderService ?? (_paymentProviderService = new PaymentProviderService()); } }
        public TokenService TokenService { get { return _tokenService ?? (_tokenService = new TokenService()); } }

        public CheckoutClient()
        {
            ContentAdaptor.Setup();
        }
    }
}
