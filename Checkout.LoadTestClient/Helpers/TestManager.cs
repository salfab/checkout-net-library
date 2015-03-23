using Checkout.ApiServices.SharedModels;
using Checkout.ApiServices.Tokens.ResponseModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tests;

namespace Checkout.LoadTestClient.Helpers
{
    public class TestManager
    {

        #region Tokens Tests
        public void CreateCardToken()
        {
            var cardTokenCreateModel = TestHelper.GetCardTokenCreateModel();
           new CheckoutClient().TokenService.CreateCardToken(cardTokenCreateModel);
        }

        public void CreatePaymentToken()
        {
            var paymentTokenCreateModel = TestHelper.GetPaymentTokenCreateModel();
            new CheckoutClient().TokenService.CreatePaymentToken(paymentTokenCreateModel);
        }
        #endregion

        #region Payment Providers Tests
        public void GetCardProviders()
        {
            new CheckoutClient().PaymentProviderService.GetCardProviderList();
        }
        public void GetCardPaymentProvider()
        {
            var cardProvider = new CheckoutClient().PaymentProviderService.GetCardProviderList().Model.Data.First();

           new CheckoutClient().PaymentProviderService.GetCardProvider(cardProvider.Id);
        }
        #endregion

        #region Customers Tests
        public void CreateCustomerWithCard()
        {
            var customerCreateModel = TestHelper.GetCustomerCreateModelWithCard();
            new CheckoutClient().CustomerService.CreateCustomer(customerCreateModel);
        }

        public void CreateCustomerWithNoCard()
        {
            var customerCreateModel = TestHelper.GetCustomerCreateModelWithNoCard();
            new CheckoutClient().CustomerService.CreateCustomer(customerCreateModel);
        }

        public void GetCustomerList()
        {
            //Get all customers created since yesterday
            new CheckoutClient().CustomerService.GetCustomerList(10, 0, DateTime.Now.AddMinutes(-1), DateTime.Now);
        }

        public void GetCustomer()
        {
            var customer = new CheckoutClient().
                                CustomerService.
                                CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            new CheckoutClient().CustomerService.GetCustomer(customer.Id);
        }

        public void UpdateCustomer()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            var customerUpdateModel = TestHelper.GetCustomerUpdateModel(customer.Id);

            new CheckoutClient().CustomerService.UpdateCustomer(customerUpdateModel);
        }

        public void DeleteCustomer()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            new CheckoutClient().CustomerService.DeleteCustomer(customer.Id);
        }

        #endregion

        #region Cards Tests
        public void CreateCard()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithNoCard()).Model;

            var cardCreateModel = TestHelper.GetCardCreateModel(customer.Id);
            new CheckoutClient().CardService.CreateCard(cardCreateModel);
        }

        public void GetCard()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var customerCardId = customer.Cards.Data.First().Id;

            new CheckoutClient().CardService.GetCard(customer.Id, customerCardId);
        }

        public void GetCardList()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithNoCard()).Model;
            var customerCard1 = new CheckoutClient().CardService.CreateCard(TestHelper.GetCardCreateModel(customer.Id, Tests.Utils.CardProvider.Visa)).Model;
            var customerCard2 = new CheckoutClient().CardService.CreateCard(TestHelper.GetCardCreateModel(customer.Id, Tests.Utils.CardProvider.Mastercard)).Model;

            new CheckoutClient().CardService.GetCardList(customer.Id);
        }

        public void UpdateCard()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var customerCardId = customer.Cards.Data.First().Id;

            var cardUpdateModel = TestHelper.GetCardUpdateModel(customer.Id, customerCardId);
            new CheckoutClient().CardService.UpdateCard(cardUpdateModel);
        }

        public void DeleteCard()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var customerCardId = customer.Cards.Data.First().Id;

            new CheckoutClient().CardService.DeleteCard(customer.Id, customerCardId);
        }
        #endregion

        #region Charge Tests
        public void CreateChargeWithCard()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);

           new CheckoutClient().ChargeService.ChargeWithCard(cardCreateModel);
        }

        public void CreateChargeWithCardId()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            var cardIdChargeCreateModel = TestHelper.GetCardIdChargeCreateModel(customer.Cards.Data[0].Id, customer.Email);

            new CheckoutClient().ChargeService.ChargeWithCardId(cardIdChargeCreateModel);
        }

        public void CreateChargeWithCardToken()
        {
            var cardToken = new CheckoutClient().TokenService.CreateCardToken(TestHelper.GetCardTokenCreateModel()).Model;
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            var cardTokenChargeCreateModel = TestHelper.GetCardTokenChargeCreateModel(cardToken.Id, customer.Email);

            new CheckoutClient().ChargeService.ChargeWithCardToken(cardTokenChargeCreateModel);
        }

        public void CreateChargeWithCustomerDefaultCard()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            var baseChargeModel = TestHelper.GetBaseChargeModel(customerId: customer.Id);
            new CheckoutClient().ChargeService.ChargeWithDefaultCustomerCard(baseChargeModel);
        }

        public void RefundCharge()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var charge = new CheckoutClient().ChargeService.ChargeWithCard(cardCreateModel).Model;

            var chargeRefundModel = TestHelper.GetChargeRefundModel(charge.Id, (int)charge.Value);
            new CheckoutClient().ChargeService.RefundCardChargeRequest(chargeRefundModel);
        }

        public void CaptureCharge()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var charge = new CheckoutClient().ChargeService.ChargeWithCard(cardCreateModel).Model;

            var chargeCaptureModel = TestHelper.GetChargeCaptureModel(charge.Id, (int)charge.Value);
            new CheckoutClient().ChargeService.CaptureCardCharge(chargeCaptureModel);
        }

        public void UpdateCharge()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var charge = new CheckoutClient().ChargeService.ChargeWithCard(cardCreateModel).Model;

            var chargeUpdateModel = TestHelper.GetChargeUpdateModel(charge.Id);

            new CheckoutClient().ChargeService.UpdateCardCharge(chargeUpdateModel);
        }
        #endregion
    }
}
