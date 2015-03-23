using Checkout;
using Checkout.ApiServices.SharedModels;
using NUnit.Framework;
using System;


namespace Tests
{
    [TestFixture(Category = "TokensApi")]
    public class CardTokensApiTests
    {
       
        [Test]
        public void CreateCardToken()
        {
            var cardTokenCreateModel = TestHelper.GetCardTokenCreateModel();
            var response = new CheckoutClient().TokenService.CreateCardToken(cardTokenCreateModel);
         
            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("card_tok"));
            Assert.IsTrue(response.Model.Object.ToLower() == "token");

            var isBillingAddressSame = ReflectionHelper.CompareProperties<Address>(response.Model.Card.BillingDetails, cardTokenCreateModel.Card.BillingDetails);

            Assert.IsTrue(isBillingAddressSame);
        }
    }
}
