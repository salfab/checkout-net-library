using System.Net;
using Checkout.ApiServices.Charges.RequestModels;
using Checkout.ApiServices.Charges.ResponseModels;
using Checkout.ApiServices.SharedModels;
using FluentAssertions;
using NUnit.Framework;
using Tests.Utils;

namespace Tests
{
    [TestFixture(Category = "ChargesApi")]
    public class ChargeService : BaseServiceTests
    {
        #region CaptureCharge

        [Test]
        public void CaptureCharge()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var charge = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel).Model;
            var chargeCaptureModel = TestHelper.GetChargeCaptureModel(charge.Value);
            var response = CheckoutClient.ChargeService.CaptureCharge(charge.Id, chargeCaptureModel);

            ValidateCaptureCharge(response, charge);
        }

        [Test]
        public async void CaptureChargeAsync()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var chargeResponse = await CheckoutClient.ChargeService.ChargeWithCardAsync(cardCreateModel);

            var chargeCaptureModel = TestHelper.GetChargeCaptureModel(chargeResponse.Model.Value);
            var response = await CheckoutClient.ChargeService.CaptureChargeAsync(chargeResponse.Model.Id, chargeCaptureModel);

            ValidateCaptureCharge(response, chargeResponse.Model);
        }

        private static void ValidateCaptureCharge(HttpResponse<Capture> response, Charge charge)
        {
            ////Check if charge details match
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.OriginalId.Should().BeEquivalentTo(charge.Id);
        }

        #endregion

        #region CreateChargeWithCard

        [Test]
        public void CreateChargeWithCard()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var response = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);
            ValidateChargeWithCardResponse(response, cardCreateModel);
        }

        [Test]
        public async void CreateChargeWithCardAsync()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var response = await CheckoutClient.ChargeService.ChargeWithCardAsync(cardCreateModel);
            ValidateChargeWithCardResponse(response, cardCreateModel);
        }

        private static void ValidateChargeWithCardResponse(HttpResponse<Charge> response, CardCharge cardCreateModel)
        {
            //Check if charge details match
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Id.Should().StartWith("charge_");

            response.Model.AutoCapTime.Should().Be(cardCreateModel.AutoCapTime);
            response.Model.AutoCapture.Should().BeEquivalentTo(cardCreateModel.AutoCapture);
            response.Model.Email.Should().BeEquivalentTo(cardCreateModel.Email);
            response.Model.Currency.Should().BeEquivalentTo(cardCreateModel.Currency);
            response.Model.Description.Should().BeEquivalentTo(cardCreateModel.Description);
            response.Model.Value.Should().Be(cardCreateModel.Value);
            response.Model.Status.Should().NotBeNullOrEmpty();
            response.Model.AuthCode.Should().NotBeNullOrEmpty();
            response.Model.ResponseCode.Should().NotBeNullOrEmpty();

            //Check if card details match
            response.Model.Card.Name.Should().Be(cardCreateModel.Card.Name);
            response.Model.Card.ExpiryMonth.Should().Be(cardCreateModel.Card.ExpiryMonth);
            response.Model.Card.ExpiryYear.Should().Be(cardCreateModel.Card.ExpiryYear);
            cardCreateModel.Card.Number.Should().EndWith(response.Model.Card.Last4);
            response.Model.Card.BillingDetails.ShouldBeEquivalentTo(cardCreateModel.Card.BillingDetails);

            //Check if shipping details match
            response.Model.Products.ShouldBeEquivalentTo(cardCreateModel.Products);

            //Check if metadatadetails match
            response.Model.Metadata.ShouldBeEquivalentTo(cardCreateModel.Metadata);
        }

        #endregion

        #region CreateChargeWithCardId

        [Test]
        public void CreateChargeWithCardId()
        {
            var customer = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var cardIdChargeCreateModel = TestHelper.GetCardIdChargeCreateModel(customer.Cards.Data[0].Id, customer.Email);
            var response = CheckoutClient.ChargeService.ChargeWithCardId(cardIdChargeCreateModel);

            ValidateChargeWithChargeId(response, cardIdChargeCreateModel);
        }

        [Test]
        public async void CreateChargeWithCardIdAsync()
        {
            var customer = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var cardIdChargeCreateModel = TestHelper.GetCardIdChargeCreateModel(customer.Cards.Data[0].Id, customer.Email);
            var response = await CheckoutClient.ChargeService.ChargeWithCardIdAsync(cardIdChargeCreateModel);

            ValidateChargeWithChargeId(response, cardIdChargeCreateModel);
        }

        private static void ValidateChargeWithChargeId(HttpResponse<Charge> response, CardIdCharge cardIdChargeCreateModel)
        {
            ////Check if charge details match
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Id.Should().StartWith("charge_");

            response.Model.AutoCapTime.Should().Be(cardIdChargeCreateModel.AutoCapTime);
            response.Model.AutoCapture.Should().BeEquivalentTo(cardIdChargeCreateModel.AutoCapture);
            response.Model.Email.Should().BeEquivalentTo(cardIdChargeCreateModel.Email);
            response.Model.Currency.Should().BeEquivalentTo(cardIdChargeCreateModel.Currency);
            response.Model.Description.Should().BeEquivalentTo(cardIdChargeCreateModel.Description);
            response.Model.Value.Should().Be(cardIdChargeCreateModel.Value);
            response.Model.Card.Id.Should().Be(cardIdChargeCreateModel.CardId);
            response.Model.Status.Should().NotBeNullOrEmpty();
            response.Model.AuthCode.Should().NotBeNullOrEmpty();
            response.Model.ResponseCode.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region CreateChargeWithCardToken

        [Test]
        public void CreateChargeWithCardToken()
        {
            var cardToken = "card_tok_34FF74EC-5E8A-41CD-A7FF-8992F54DAA9F"; // card token for the JS charge
            var cardTokenChargeModel = TestHelper.GetCardTokenChargeCreateModel(cardToken, TestHelper.RandomData.Email);
            var response = CheckoutClient.ChargeService.ChargeWithCardToken(cardTokenChargeModel);

            ValidateCreateChargeWithCardToken(response, cardTokenChargeModel);
        }

        [Test]
        public async void CreateChargeWithCardTokenAsync()
        {
            var cardToken = "card_tok_34FF74EC-5E8A-41CD-A7FF-8992F54DAA9F"; // card token for the JS charge
            var cardTokenChargeModel = TestHelper.GetCardTokenChargeCreateModel(cardToken, TestHelper.RandomData.Email);
            var response = await CheckoutClient.ChargeService.ChargeWithCardTokenAsync(cardTokenChargeModel);

            ValidateCreateChargeWithCardToken(response, cardTokenChargeModel);
        }

        private static void ValidateCreateChargeWithCardToken(HttpResponse<Charge> response, CardTokenCharge cardTokenChargeModel)
        {
            ////Check if charge details match
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Id.Should().StartWith("charge_");

            response.Model.AutoCapTime.Should().Be(cardTokenChargeModel.AutoCapTime);
            response.Model.AutoCapture.Should().BeEquivalentTo(cardTokenChargeModel.AutoCapture);
            response.Model.Email.Should().BeEquivalentTo(cardTokenChargeModel.Email);
            response.Model.Currency.Should().BeEquivalentTo(cardTokenChargeModel.Currency);
            response.Model.Description.Should().BeEquivalentTo(cardTokenChargeModel.Description);
            response.Model.Value.Should().Be(cardTokenChargeModel.Value);
            response.Model.Status.Should().NotBeNullOrEmpty();
            response.Model.AuthCode.Should().NotBeNullOrEmpty();
            response.Model.ResponseCode.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region CreateChargeWithCustomerDefaultCard

        [Test]
        public void CreateChargeWithCustomerDefaultCard()
        {
            var customer = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard(CardProvider.Mastercard)).Model;
            var baseChargeModel = TestHelper.GetCustomerDefaultCardChargeCreateModel(customer.Id);
            var response = CheckoutClient.ChargeService.ChargeWithDefaultCustomerCard(baseChargeModel);

            ValidateChargeWithCustomerDefaultCard(response, baseChargeModel);
        }

        [Test]
        public async void CreateChargeWithCustomerDefaultCardAsync()
        {
            var customerResponse = await CheckoutClient.CustomerService.CreateCustomerAsync(TestHelper.GetCustomerCreateModelWithCard(CardProvider.Mastercard));
            var customer = customerResponse.Model;
            var baseChargeModel = TestHelper.GetCustomerDefaultCardChargeCreateModel(customer.Id);
            var response = CheckoutClient.ChargeService.ChargeWithDefaultCustomerCard(baseChargeModel);

            ValidateChargeWithCustomerDefaultCard(response, baseChargeModel);
        }

        private static void ValidateChargeWithCustomerDefaultCard(HttpResponse<Charge> response, DefaultCardCharge baseChargeModel)
        {
            ////Check if charge details match
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Id.Should().StartWith("charge_");

            response.Model.AutoCapTime.Should().Be(baseChargeModel.AutoCapTime);
            response.Model.AutoCapture.Should().BeEquivalentTo(baseChargeModel.AutoCapture);
            response.Model.Currency.Should().BeEquivalentTo(baseChargeModel.Currency);
            response.Model.Description.Should().BeEquivalentTo(baseChargeModel.Description);
            response.Model.Value.Should().Be(baseChargeModel.Value);
            response.Model.Email.Should().NotBeNullOrEmpty();
            response.Model.Status.Should().NotBeNullOrEmpty();
            response.Model.AuthCode.Should().NotBeNullOrEmpty();
            response.Model.ResponseCode.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region GetCharge

        [Test]
        public void GetCharge()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);
            var response = CheckoutClient.ChargeService.GetCharge(chargeResponse.Model.Id);

            ValidateGetCharge(response, chargeResponse);
        }

        [Test]
        public async void GetChargeAsync()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var chargeResponse = await CheckoutClient.ChargeService.ChargeWithCardAsync(cardCreateModel);
            var response = await CheckoutClient.ChargeService.GetChargeAsync(chargeResponse.Model.Id);

            ValidateGetCharge(response, chargeResponse);
        }

        private static void ValidateGetCharge(HttpResponse<Charge> response, HttpResponse<Charge> chargeResponse)
        {
            //Check if charge details match
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Id.Should().StartWith("charge_");

            chargeResponse.Model.Id.Should().Be(response.Model.Id);
        }

        #endregion

        #region GetChargeHistory

        [Test]
        public void GetChargeHistory()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);
            var chargeVoidModel = TestHelper.GetChargeVoidModel();
            var voidResponse = CheckoutClient.ChargeService.VoidCharge(chargeResponse.Model.Id, chargeVoidModel);
            var response = CheckoutClient.ChargeService.GetChargeHistory(voidResponse.Model.Id);

            ValidateGetChargeHistory(response, voidResponse, chargeResponse);
        }

        [Test]
        public async void GetChargeHistoryAsync()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var chargeResponse = await CheckoutClient.ChargeService.ChargeWithCardAsync(cardCreateModel);
            var chargeVoidModel = TestHelper.GetChargeVoidModel();
            var voidResponse = await CheckoutClient.ChargeService.VoidChargeAsync(chargeResponse.Model.Id, chargeVoidModel);
            var response = await CheckoutClient.ChargeService.GetChargeHistoryAsync(voidResponse.Model.Id);

            ValidateGetChargeHistory(response, voidResponse, chargeResponse);
        }

        private static void ValidateGetChargeHistory(HttpResponse<ChargeHistory> response, HttpResponse<Void> voidResponse, HttpResponse<Charge> chargeResponse)
        {
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Charges.Should().HaveCount(2);

            response.Model.Charges[0].Id.Should().Be(voidResponse.Model.Id);
            response.Model.Charges[1].Id.Should().Be(chargeResponse.Model.Id);
        }

        #endregion

        #region GetChargeWithMultipleHistory

        [Test]
        public void GetChargeWithMultipleHistory()
        {
            // charge
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);

            // capture
            var chargeCaptureModel = TestHelper.GetChargeCaptureModel(chargeResponse.Model.Value);
            var captureResponse = CheckoutClient.ChargeService.CaptureCharge(chargeResponse.Model.Id, chargeCaptureModel);

            // refund
            var chargeRefundModel = TestHelper.GetChargeRefundModel(chargeResponse.Model.Value);
            var refundResponse = CheckoutClient.ChargeService.RefundCharge(captureResponse.Model.Id, chargeRefundModel);

            var response = CheckoutClient.ChargeService.GetChargeHistory(chargeResponse.Model.Id);
            ValidateChargeWithMultipleHistory(response, refundResponse, chargeResponse, captureResponse);
        }

        [Test]
        public async void GetChargeWithMultipleHistoryAsync()
        {
            // charge
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var chargeResponse = await CheckoutClient.ChargeService.ChargeWithCardAsync(cardCreateModel);

            // capture
            var chargeCaptureModel = TestHelper.GetChargeCaptureModel(chargeResponse.Model.Value);
            var captureResponse = await CheckoutClient.ChargeService.CaptureChargeAsync(chargeResponse.Model.Id, chargeCaptureModel);

            // refund
            var chargeRefundModel = TestHelper.GetChargeRefundModel(chargeResponse.Model.Value);
            var refundResponse = await CheckoutClient.ChargeService.RefundChargeAsync(captureResponse.Model.Id, chargeRefundModel);

            var response = await CheckoutClient.ChargeService.GetChargeHistoryAsync(chargeResponse.Model.Id);
            ValidateChargeWithMultipleHistory(response, refundResponse, chargeResponse, captureResponse);
        }

        private static void ValidateChargeWithMultipleHistory(HttpResponse<ChargeHistory> response, HttpResponse<Refund> refundResponse, HttpResponse<Charge> chargeResponse, HttpResponse<Capture> captureResponse)
        {
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Charges.Should().HaveCount(3);

            response.Model.Charges[0].Id.Should().Be(refundResponse.Model.Id);
            response.Model.Charges[1].Id.Should().Be(captureResponse.Model.Id);
            response.Model.Charges[2].Id.Should().Be(chargeResponse.Model.Id);

            chargeResponse.Model.Id.Should().Be(captureResponse.Model.OriginalId);
            refundResponse.Model.OriginalId.Should().Be(captureResponse.Model.Id);
        }

        #endregion

        #region RefundCharge

        [Test]
        public void RefundCharge()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var charge = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel).Model;

            var chargeCaptureModel = TestHelper.GetChargeCaptureModel(charge.Value);
            var captureResponse = CheckoutClient.ChargeService.CaptureCharge(charge.Id, chargeCaptureModel);

            var chargeRefundModel = TestHelper.GetChargeRefundModel(charge.Value);
            var response = CheckoutClient.ChargeService.RefundCharge(captureResponse.Model.Id, chargeRefundModel);

            ValidateRefundCharge(response, captureResponse);
        }

        [Test]
        public async void RefundChargeAsync()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var chargeResponse = await CheckoutClient.ChargeService.ChargeWithCardAsync(cardCreateModel);
            var charge = chargeResponse.Model;

            var chargeCaptureModel = TestHelper.GetChargeCaptureModel(charge.Value);
            var captureResponse = await CheckoutClient.ChargeService.CaptureChargeAsync(charge.Id, chargeCaptureModel);

            var chargeRefundModel = TestHelper.GetChargeRefundModel(charge.Value);
            var response = await CheckoutClient.ChargeService.RefundChargeAsync(captureResponse.Model.Id, chargeRefundModel);

            ValidateRefundCharge(response, captureResponse);
        }

        private static void ValidateRefundCharge(HttpResponse<Refund> response, HttpResponse<Capture> captureResponse)
        {
            //Check if charge details match
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.OriginalId.Should().Be(captureResponse.Model.Id);
        }

        #endregion

        #region UpdateCharge

        [Test]
        public void UpdateCharge()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var charge = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel).Model;

            var chargeUpdateModel = TestHelper.GetChargeUpdateModel();
            var response = CheckoutClient.ChargeService.UpdateCharge(charge.Id, chargeUpdateModel);

            ValidateUpdateCharge(response);
        }

        [Test]
        public async void UpdateChargeAsync()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var chargeResponse = await CheckoutClient.ChargeService.ChargeWithCardAsync(cardCreateModel);
            var charge = chargeResponse.Model;

            var chargeUpdateModel = TestHelper.GetChargeUpdateModel();
            var response = await CheckoutClient.ChargeService.UpdateChargeAsync(charge.Id, chargeUpdateModel);

            ValidateUpdateCharge(response);
        }

        private static void ValidateUpdateCharge(HttpResponse<OkResponse> response)
        {
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Message.Should().BeEquivalentTo("Ok");
        }

        #endregion

        #region VerifyChargeByPaymentToken

        [Test]
        public void VerifyChargeByPaymentToken()
        {
            var paymentToken = "pay_tok_cacdc3d0-f912-4ebb-9f84-8fde65e05fbd"; // payment token for the JS charge
            var response = CheckoutClient.ChargeService.VerifyCharge(paymentToken);
            ValidateVerifyChargeByPaymentToken(response);
        }

        [Test]
        public async void VerifyChargeByPaymentTokenAsync()
        {
            var paymentToken = "pay_tok_cacdc3d0-f912-4ebb-9f84-8fde65e05fbd"; // payment token for the JS charge
            var response = await CheckoutClient.ChargeService.VerifyChargeAsync(paymentToken);
            ValidateVerifyChargeByPaymentToken(response);
        }

        private static void ValidateVerifyChargeByPaymentToken(HttpResponse<Charge> response)
        {
            //Check if charge details match
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Id.Should().StartWith("charge_");
        }

        #endregion

        #region VoidCharge

        [Test]
        public void VoidCharge()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var charge = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel).Model;

            var chargeVoidModel = TestHelper.GetChargeVoidModel();
            var response = CheckoutClient.ChargeService.VoidCharge(charge.Id, chargeVoidModel);

            ValidateVoidCharge(response, charge);
        }

        [Test]
        public async void VoidChargeAsync()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var chargeResponse = await CheckoutClient.ChargeService.ChargeWithCardAsync(cardCreateModel);
            var charge = chargeResponse.Model;

            var chargeVoidModel = TestHelper.GetChargeVoidModel();
            var response = await CheckoutClient.ChargeService.VoidChargeAsync(charge.Id, chargeVoidModel);

            ValidateVoidCharge(response, charge);
        }

        private static void ValidateVoidCharge(HttpResponse<Void> response, Charge charge)
        {
            //Check if charge details match
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.OriginalId.Should().Be(charge.Id);
        }

        #endregion
    }
}