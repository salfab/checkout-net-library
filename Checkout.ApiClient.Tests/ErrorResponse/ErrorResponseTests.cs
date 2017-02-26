﻿using System.Net;

using Checkout.ApiServices.SharedModels;

using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture(Category = "ErrorResponseTests")]
    public class ErrorResponseTests : BaseServiceTests
    {
        [Test]
        public void CreateCharge_FailsWithError_IfCardNumberIsInvalid()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            ;
            cardCreateModel.Card.Number = "4242424242424243";

            var response = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);

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

            var response = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().NotBe(HttpStatusCode.OK);
            response.HasError.Should().BeTrue();
            response.GetError<ResponseError>().ErrorCode.Should().Be("70000");
            response.GetError<ResponseError>().Message.Should().BeEquivalentTo("validation error");
        }
    }
}