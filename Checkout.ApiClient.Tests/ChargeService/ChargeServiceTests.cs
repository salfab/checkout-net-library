using Checkout;
using Checkout.ApiServices.Charges.RequestModels;
using Checkout.ApiServices.SharedModels;
using NUnit.Framework;
using System;

namespace Tests
{
    [TestFixture(Category = "ChargesApi")]
    public class ChargeService
    {
        APIClient CheckoutClient;

        [SetUp]
        public void Init()
        { CheckoutClient = new APIClient(); }

        [Test]
        public void CreateChargeWithCard()
       {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var response = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);

            //Check if charge details match
            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("charge_"));
            
            Assert.IsTrue(response.Model.AutoCapTime == cardCreateModel.AutoCapTime);
            Assert.IsTrue(response.Model.AutoCapture.Equals(cardCreateModel.AutoCapture, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Email.Equals(cardCreateModel.Email, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Currency.Equals(cardCreateModel.Currency, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Description.Equals(cardCreateModel.Description, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Value == cardCreateModel.Value);
            Assert.IsNotNullOrEmpty(response.Model.Status);
            Assert.IsNotNullOrEmpty(response.Model.AuthCode);
            Assert.IsNotNullOrEmpty(response.Model.ResponseCode);

            //Check if card details match
            Assert.IsTrue(response.Model.Card.Name == cardCreateModel.Card.Name);
            Assert.IsTrue(response.Model.Card.ExpiryMonth == cardCreateModel.Card.ExpiryMonth);
            Assert.IsTrue(response.Model.Card.ExpiryYear == cardCreateModel.Card.ExpiryYear);
            Assert.IsTrue(cardCreateModel.Card.Number.EndsWith(response.Model.Card.Last4));
            var isCardBillingAddressSame = ReflectionHelper.CompareProperties(response.Model.Card.BillingDetails, cardCreateModel.Card.BillingDetails);
            Assert.IsTrue(isCardBillingAddressSame);

            //Check if shipping details match
            for (int i = 0; i < cardCreateModel.Products.Count; i++)
            {
              
                    var isProductSame = ReflectionHelper.CompareProperties(response.Model.Products[i], cardCreateModel.Products[i], "ProductId","customerId" );
                    Assert.IsTrue(isProductSame);
            }

            //Check if metadatadetails match
            foreach (string key in cardCreateModel.Metadata.Keys)
            {
                Assert.IsTrue(response.Model.Metadata[key].Equals(cardCreateModel.Metadata[key], System.StringComparison.OrdinalIgnoreCase));
            
            }
        }

         [Test]
        public void CreateChargeWithCardId()
        {
            var customer = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            var cardIdChargeCreateModel = TestHelper.GetCardIdChargeCreateModel(customer.Cards.Data[0].Id, customer.Email);

            var response = CheckoutClient.ChargeService.ChargeWithCardId(cardIdChargeCreateModel);

            ////Check if charge details match
            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("charge_"));
            
            Assert.IsTrue(response.Model.AutoCapTime == cardIdChargeCreateModel.AutoCapTime);
            Assert.IsTrue(response.Model.AutoCapture.Equals(cardIdChargeCreateModel.AutoCapture, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Email.Equals(cardIdChargeCreateModel.Email, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Currency.Equals(cardIdChargeCreateModel.Currency, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Description.Equals(cardIdChargeCreateModel.Description, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Value == cardIdChargeCreateModel.Value);
            Assert.IsTrue(response.Model.Card.Id == cardIdChargeCreateModel.CardId);
            Assert.IsNotNullOrEmpty(response.Model.Status);
            Assert.IsNotNullOrEmpty(response.Model.AuthCode);
            Assert.IsNotNullOrEmpty(response.Model.ResponseCode);
   
        }

         [Test]
         public void CreateChargeWithCardToken()
         {
             string cardToken = "card_tok_34FF74EC-5E8A-41CD-A7FF-8992F54DAA9F";// card token for the JS charge

             var cardTokenChargeModel = TestHelper.GetCardTokenChargeCreateModel(cardToken, TestHelper.RandomData.Email);

             var response = CheckoutClient.ChargeService.ChargeWithCardToken(cardTokenChargeModel);

             ////Check if charge details match
             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.Id.StartsWith("charge_"));

             Assert.IsTrue(response.Model.AutoCapTime == cardTokenChargeModel.AutoCapTime);
             Assert.IsTrue(response.Model.AutoCapture.Equals(cardTokenChargeModel.AutoCapture, System.StringComparison.OrdinalIgnoreCase));
             Assert.IsTrue(response.Model.Email.Equals(cardTokenChargeModel.Email, System.StringComparison.OrdinalIgnoreCase));
             Assert.IsTrue(response.Model.Currency.Equals(cardTokenChargeModel.Currency, System.StringComparison.OrdinalIgnoreCase));
             Assert.IsTrue(response.Model.Description.Equals(cardTokenChargeModel.Description, System.StringComparison.OrdinalIgnoreCase));
             Assert.IsTrue(response.Model.Value == cardTokenChargeModel.Value);
             Assert.IsNotNullOrEmpty(response.Model.Status);
             Assert.IsNotNullOrEmpty(response.Model.AuthCode);
             Assert.IsNotNullOrEmpty(response.Model.ResponseCode);
         }

         [Test]
         public void CreateChargeWithCustomerDefaultCard()
         {
             var customer = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard(Utils.CardProvider.Mastercard)).Model;

             var baseChargeModel = TestHelper.GetCustomerDefaultCardChargeCreateModel(customer.Id);
             var response = CheckoutClient.ChargeService.ChargeWithDefaultCustomerCard(baseChargeModel);

             ////Check if charge details match
             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.Id.StartsWith("charge_"));
             
             Assert.IsTrue(response.Model.AutoCapTime == baseChargeModel.AutoCapTime);
             Assert.IsTrue(response.Model.AutoCapture.Equals(baseChargeModel.AutoCapture, System.StringComparison.OrdinalIgnoreCase));
             Assert.IsTrue(response.Model.Currency.Equals(baseChargeModel.Currency, System.StringComparison.OrdinalIgnoreCase));
             Assert.IsTrue(response.Model.Description.Equals(baseChargeModel.Description, System.StringComparison.OrdinalIgnoreCase));
             Assert.IsTrue(response.Model.Value == baseChargeModel.Value);
             Assert.IsNotNullOrEmpty(response.Model.Email);
             Assert.IsNotNullOrEmpty(response.Model.Status);
             Assert.IsNotNullOrEmpty(response.Model.AuthCode);
             Assert.IsNotNullOrEmpty(response.Model.ResponseCode);
         }

         [Test]
         public void RefundCharge()
         {
             var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);

             var charge = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel).Model;

             var chargeCaptureModel = TestHelper.GetChargeCaptureModel(charge.Value);

             var captureResponse = CheckoutClient.ChargeService.CaptureCharge(charge.Id, chargeCaptureModel);

             var chargeRefundModel = TestHelper.GetChargeRefundModel(charge.Value);
             var response = CheckoutClient.ChargeService.RefundCharge(captureResponse.Model.Id, chargeRefundModel);

             //Check if charge details match
             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.OriginalId ==captureResponse.Model.Id);

         }

         [Test]
         public void VoidCharge()
         {
             var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);

             var charge = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel).Model;

             var chargeVoidModel = TestHelper.GetChargeVoidModel();

             var response = CheckoutClient.ChargeService.VoidCharge(charge.Id, chargeVoidModel);

             //Check if charge details match
             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.OriginalId == charge.Id);
         }

         [Test]
         public void CaptureCharge()
         {
             var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);

             var charge = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel).Model;

             var chargeCaptureModel = TestHelper.GetChargeCaptureModel(charge.Value);

             var response = CheckoutClient.ChargeService.CaptureCharge(charge.Id, chargeCaptureModel);

             ////Check if charge details match
             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.OriginalId.Equals(charge.Id, System.StringComparison.OrdinalIgnoreCase));
         }

         [Test]
         public void UpdateCharge()
         {
             var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
             var charge = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel).Model;

             var chargeUpdateModel = TestHelper.GetChargeUpdateModel();

             var response = CheckoutClient.ChargeService.UpdateCharge(charge.Id, chargeUpdateModel);

             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.Message.Equals("Ok", System.StringComparison.OrdinalIgnoreCase));
         }

         [Test]
         public void GetCharge()
         {
             var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);

             var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);

             var response = CheckoutClient.ChargeService.GetCharge(chargeResponse.Model.Id);

             //Check if charge details match
             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.Id.StartsWith("charge_"));

             Assert.IsTrue(chargeResponse.Model.Id == response.Model.Id);
         }

         [Test]
         public void GetChargeHistory()
         {
             #region setup charge history
             var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);

             var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);

             var chargeVoidModel = TestHelper.GetChargeVoidModel();

             var voidResponse = CheckoutClient.ChargeService.VoidCharge(chargeResponse.Model.Id, chargeVoidModel);
             #endregion

             var response = CheckoutClient.ChargeService.GetChargeHistory(voidResponse.Model.Id);

             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.Charges.Count ==2);

             Assert.IsTrue(response.Model.Charges[0].Id == voidResponse.Model.Id);
             Assert.IsTrue(response.Model.Charges[1].Id == chargeResponse.Model.Id);
         }

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

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Charges.Count == 3);

            Assert.IsTrue(response.Model.Charges[0].Id == refundResponse.Model.Id);
            Assert.IsTrue(response.Model.Charges[1].Id == captureResponse.Model.Id);
            Assert.IsTrue(response.Model.Charges[2].Id == chargeResponse.Model.Id);

            Assert.IsTrue(chargeResponse.Model.Id == captureResponse.Model.OriginalId);
            Assert.IsTrue(refundResponse.Model.OriginalId == captureResponse.Model.Id);
        }

        [Test]
         public void VerifyChargeByPaymentToken()
         {
             string paymentToken = "pay_tok_cacdc3d0-f912-4ebb-9f84-8fde65e05fbd";// payment token for the JS charge

             var response = CheckoutClient.ChargeService.VerifyCharge(paymentToken);

             //Check if charge details match
             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.Id.StartsWith("charge_"));
         }
    }
}
