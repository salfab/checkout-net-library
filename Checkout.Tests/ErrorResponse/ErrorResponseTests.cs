using Checkout;
using Checkout.ApiServices.Tokens.ResponseModels;
using NUnit.Framework;
using System;


namespace Tests
{
    [TestFixture(Category = "ErrorResponseTests")]
    public class ErrorResponseTests
    {
      
        [Test]
        public void CreateCardToken_FailsWithError_IfCardNumberIsInvalid()
        {
            var cardTokenCreateModel = TestHelper.GetCardTokenCreateModel();
            cardTokenCreateModel.Card.Number = "4242424242424243";

            var response = new CheckoutClient().TokenService.CreateCardToken(cardTokenCreateModel);
            CardToken model = response.Model;

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode != System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.HasError);
            Assert.IsTrue(response.Error.ErrorCode == "20014");
            Assert.IsTrue(response.Error.Message.ToLower().Contains("invalid card number"));

        }

        [Test]
        public void CreateChargeWithCard_FailsWithValidationError_IfDetailsInvalid()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel();
            cardCreateModel.Currency = string.Empty;
            cardCreateModel.Value = -100;

            var response = new CheckoutClient().ChargeService.ChargeWithCard(cardCreateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode != System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.HasError);
            Assert.IsTrue(response.Error.ErrorCode == "70000");
            Assert.IsTrue(response.Error.Message.ToLower() == "validation error");

        }
    }
}
