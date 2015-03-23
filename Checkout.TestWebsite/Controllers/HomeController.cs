using Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tests;

namespace TestLibraryWebsite.Controllers
{
    public class HomeController : ApiController
    {
        public string Get()
        {
            var cardTokenCreateModel = TestHelper.GetCardTokenCreateModel();
            var ckoClient = new CheckoutClient();
            var response1 = ckoClient.TokenService.CreateCardToken(cardTokenCreateModel);

            var paymentTokenCreateModel = TestHelper.GetPaymentTokenCreateModel();
            var response2 = ckoClient.TokenService.CreatePaymentToken(paymentTokenCreateModel);

            return "Hrllo world";


           // Stripe.StripeClient s = new Stripe.StripeClient("");
            //s
        }
    }
}
