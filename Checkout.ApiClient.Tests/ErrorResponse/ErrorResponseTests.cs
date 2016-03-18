using Checkout;
using NUnit.Framework;
using System;
using System.Net;
using FluentAssertions;


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

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().NotBe(HttpStatusCode.OK);
            response.HasError.Should().BeTrue();
        }

        [Test]
        public void CreateChargeWithCard_FailsWithValidationError_IfDetailsInvalid()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel();
            cardCreateModel.Currency = string.Empty;
            cardCreateModel.Value = "-100";

            var response = new APIClient().ChargeService.ChargeWithCard(cardCreateModel);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().NotBe(HttpStatusCode.OK);
            response.HasError.Should().BeTrue();
            response.Error.ErrorCode.Should().Be("70000");
            response.Error.Message.Should().BeEquivalentTo("validation error");
        }
    }
}
