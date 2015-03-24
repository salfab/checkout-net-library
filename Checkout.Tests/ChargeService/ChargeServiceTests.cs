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

        [Test]
        public void CreateChargeWithCard()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);

            var response = new CheckoutClient().ChargeService.ChargeWithCard(cardCreateModel);

            //Check if charge details match
            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("charge_"));
            Assert.IsTrue(response.Model.Object.ToLower() == "charge");
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
            var isCardBillingAddressSame = ReflectionHelper.CompareProperties<Address>(response.Model.Card.BillingDetails, cardCreateModel.Card.BillingDetails);
            Assert.IsTrue(isCardBillingAddressSame);

            //Check if shipping details match
            for (int i = 0; i < cardCreateModel.Products.Count; i++)
            {
                {
                    var isProductSame = ReflectionHelper.CompareProperties<Product>(response.Model.Products[i], cardCreateModel.Products[i], "ProductId","customerId" );
                    Assert.IsTrue(isProductSame);
                }
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
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            var cardIdChargeCreateModel = TestHelper.GetCardIdChargeCreateModel(customer.Cards.Data[0].Id, customer.Email);

            var response = new CheckoutClient().ChargeService.ChargeWithCardId(cardIdChargeCreateModel);

            ////Check if charge details match
            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("charge_"));
            Assert.IsTrue(response.Model.Object.ToLower() == "charge");
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
         public void CreateChargeWithCustomerDefaultCard()
         {
             var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

             var baseChargeModel = TestHelper.GetBaseChargeModel(customerId: customer.Id);
             var response = new CheckoutClient().ChargeService.ChargeWithDefaultCustomerCard(baseChargeModel);

             ////Check if charge details match
             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.Id.StartsWith("charge_"));
             Assert.IsTrue(response.Model.Object.ToLower() == "charge");
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
             var charge = new CheckoutClient().ChargeService.ChargeWithCard(cardCreateModel).Model;

             var chargeRefundModel = TestHelper.GetChargeRefundModel(charge.Id,(int)charge.Value);
             var response = new CheckoutClient().ChargeService.RefundCardChargeRequest(chargeRefundModel);

             //Check if charge details match
             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.Id.Equals(chargeRefundModel.ChargeId, System.StringComparison.OrdinalIgnoreCase));
             Assert.IsTrue(response.Model.Object.ToLower() == "charge");
             Assert.IsTrue(response.Model.Value == chargeRefundModel.Value);
             Assert.IsTrue(int.Parse(response.Model.RefundedValue) == chargeRefundModel.Value);
         }

         [Test]
         public void CaptureCharge()
         {
             var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
             cardCreateModel.AutoCapture = "N";

             var charge = new CheckoutClient().ChargeService.ChargeWithCard(cardCreateModel).Model;

             var chargeCaptureModel = TestHelper.GetChargeCaptureModel(charge.Id, (int)charge.Value);

             var response = new CheckoutClient().ChargeService.CaptureCardCharge(chargeCaptureModel);

             ////Check if charge details match
             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.Id.Equals(chargeCaptureModel.ChargeId, System.StringComparison.OrdinalIgnoreCase));
             Assert.IsTrue(response.Model.Object.ToLower() == "charge");
             Assert.IsTrue(response.Model.Value == chargeCaptureModel.Value);
         }

         [Test]
         public void UpdateCharge()
         {
             var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
             var charge = new CheckoutClient().ChargeService.ChargeWithCard(cardCreateModel).Model;

             var chargeUpdateModel = TestHelper.GetChargeUpdateModel(charge.Id);

             var response = new CheckoutClient().ChargeService.UpdateCardCharge(chargeUpdateModel);

             ////Check if charge details match
             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.Id.StartsWith("charge_"));
             Assert.IsTrue(response.Model.Object.ToLower() == "charge");
             Assert.IsTrue(response.Model.Description.Equals(chargeUpdateModel.Description, System.StringComparison.OrdinalIgnoreCase));

             //Check if metadatadetails match
             foreach (string key in cardCreateModel.Metadata.Keys)
             {
                 Assert.IsTrue(response.Model.Metadata[key].Equals(chargeUpdateModel.Metadata[key], System.StringComparison.OrdinalIgnoreCase));

             }
            
         }

         [Test]
         public void GetCharge()
         {
             var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);

             var chargeResponse = new CheckoutClient().ChargeService.ChargeWithCard(cardCreateModel);

             var response = new CheckoutClient().ChargeService.GetCharge(chargeResponse.Model.Id);

             //Check if charge details match
             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.Id.StartsWith("charge_"));
             Assert.IsTrue(response.Model.Object.ToLower() == "charge");
             Assert.IsTrue(response.Model.Id == response.Model.Id);
         }

         [Test]
         public void GetChargeList()
         {
             var startTime = DateTime.UtcNow;

             var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
             var baseChargeModel = TestHelper.GetBaseChargeModel(customerId: customer.Id);

             var charge1 = new CheckoutClient().ChargeService.ChargeWithDefaultCustomerCard(baseChargeModel);
             var charge2 = new CheckoutClient().ChargeService.ChargeWithDefaultCustomerCard(baseChargeModel);
             var charge3 = new CheckoutClient().ChargeService.ChargeWithDefaultCustomerCard(baseChargeModel);

             var chargeGetListRequest = new ChargeGetList()
             {
                 FromDate = startTime,
                 ToDate = DateTime.UtcNow
             };

             //Get all charges created
             var response = new CheckoutClient().ChargeService.GetChargeList(chargeGetListRequest);

             Assert.NotNull(response);
             Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
             Assert.IsTrue(response.Model.Count == 3);

             Assert.IsTrue(response.Model.Data[0].Id == charge3.Model.Id);
             Assert.IsTrue(response.Model.Data[1].Id == charge2.Model.Id);
             Assert.IsTrue(response.Model.Data[2].Id == charge1.Model.Id);
         }
    }
}
