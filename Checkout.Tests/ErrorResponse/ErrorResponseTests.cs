using Checkout;
using NUnit.Framework;
using System;


namespace Tests
{
    [TestFixture(Category = "ErrorResponseTests")]
    public class ErrorResponseTests
    {
      
        [Test]
        public void CreateCharge_FailsWithError_IfCardNumberIsInvalid()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);;
            cardCreateModel.Card.Number = "4242424242424243";

            var response = new APIClient().ChargeService.ChargeWithCard(cardCreateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode != System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.HasError);
        }

        [Test]
        public void CreateChargeWithCard_FailsWithValidationError_IfDetailsInvalid()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel();
            cardCreateModel.Currency = string.Empty;
            cardCreateModel.Value = "-100";

            var response = new APIClient().ChargeService.ChargeWithCard(cardCreateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode != System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.HasError);
            Assert.IsTrue(response.Error.ErrorCode == "70000");
            Assert.IsTrue(response.Error.Message.ToLower() == "validation error");

        }
    }
}
