using Checkout.ApiServices.Customers.RequestModels;
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
        //public void CreateCardToken()
        //{
        //    var cardTokenCreateModel = TestHelper.GetCardTokenCreateModel();
        //   new APIClient().TokenService.CreateCardToken(cardTokenCreateModel);
        //}

        //public void CreatePaymentToken()
        //{
        //    var paymentTokenCreateModel = TestHelper.GetPaymentTokenCreateModel();
        //    new APIClient().TokenService.CreatePaymentToken(paymentTokenCreateModel);
        //}
        #endregion

        //#region Payment Providers Tests
        //public void GetCardProviders()
        //{
        //    new APIClient().PaymentProviderService.GetCardProviderList();
        //}
        //public void GetCardPaymentProvider()
        //{
        //    var cardProvider = new APIClient().PaymentProviderService.GetCardProviderList().Model.Data.First();

        //   new APIClient().PaymentProviderService.GetCardProvider(cardProvider.Id);
        //}
        //#endregion

        #region Customers Tests
        public void CreateCustomerWithCard()
        {
            var customerCreateModel = TestHelper.GetCustomerCreateModelWithCard();
            new APIClient().CustomerService.CreateCustomer(customerCreateModel);
        }

        public void CreateCustomerWithNoCard()
        {
            var customerCreateModel = TestHelper.GetCustomerCreateModelWithNoCard();
            new APIClient().CustomerService.CreateCustomer(customerCreateModel);
        }

        public void GetCustomerList()
        {
            var customerGetList = new CustomerGetList()
            {
                Count = 10,
                Offset = 0,
                FromDate = DateTime.Now.AddMinutes(-1),
                ToDate = DateTime.Now
            };

            //Get all customers created since yesterday
            new APIClient().CustomerService.GetCustomerList(customerGetList);
        }

        public void GetCustomer()
        {
            var customer = new APIClient().
                                CustomerService.
                                CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            new APIClient().CustomerService.GetCustomer(customer.Id);
        }

        public void UpdateCustomer()
        {
            var customer = new APIClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            new APIClient().CustomerService.UpdateCustomer(customer.Id, TestHelper.GetCustomerUpdateModel());
        }

        public void DeleteCustomer()
        {
            var customer = new APIClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            new APIClient().CustomerService.DeleteCustomer(customer.Id);
        }

        #endregion

        #region Cards Tests
        public void CreateCard()
        {
            var customer = new APIClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithNoCard()).Model;

            var cardCreateModel = TestHelper.GetCardCreateModel();
            new APIClient().CardService.CreateCard(customer.Id,cardCreateModel);
        }

        public void GetCard()
        {
            var customer = new APIClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var customerCardId = customer.Cards.Data.First().Id;

            new APIClient().CardService.GetCard(customer.Id, customerCardId);
        }

        public void GetCardList()
        {
            var customer = new APIClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithNoCard()).Model;
            var customerCard1 = new APIClient().CardService.CreateCard(customer.Id, TestHelper.GetCardCreateModel()).Model;
            var customerCard2 = new APIClient().CardService.CreateCard(customer.Id, TestHelper.GetCardCreateModel()).Model;

            new APIClient().CardService.GetCardList(customer.Id);
        }

        public void UpdateCard()
        {
            var customer = new APIClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var customerCardId = customer.Cards.Data.First().Id;

            var cardUpdateModel = TestHelper.GetCardUpdateModel();
            new APIClient().CardService.UpdateCard(customer.Id, customerCardId,cardUpdateModel);
        }

        public void DeleteCard()
        {
            var customer = new APIClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var customerCardId = customer.Cards.Data.First().Id;

            new APIClient().CardService.DeleteCard(customer.Id, customerCardId);
        }
        #endregion

        #region Charge Tests
        public void CreateChargeWithCard()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);

           new APIClient().ChargeService.ChargeWithCard(cardCreateModel);
        }

        public void CreateChargeWithCardId()
        {
            var customer = new APIClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            var cardIdChargeCreateModel = TestHelper.GetCardIdChargeCreateModel(customer.Cards.Data[0].Id, customer.Email);

            new APIClient().ChargeService.ChargeWithCardId(cardIdChargeCreateModel);
        }

        //public void CreateChargeWithCardToken()
        //{
        //    var cardToken = new APIClient().TokenService.CreateCardToken(TestHelper.GetCardTokenCreateModel()).Model;
        //    var customer = new APIClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

        //    var cardTokenChargeCreateModel = TestHelper.GetCardTokenChargeCreateModel(cardToken.Id, customer.Email);

        //    new APIClient().ChargeService.ChargeWithCardToken(cardTokenChargeCreateModel);
        //}

        public void CreateChargeWithCustomerDefaultCard()
        {
            var customer = new APIClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            new APIClient().ChargeService.ChargeWithDefaultCustomerCard(TestHelper.GetCustomerDefaultCardChargeCreateModel(customer.Id));
        }

        public void RefundCharge()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var charge = new APIClient().ChargeService.ChargeWithCard(cardCreateModel).Model;

            var chargeRefundModel = TestHelper.GetChargeRefundModel(charge.Value);
            new APIClient().ChargeService.RefundCharge(charge.Id,chargeRefundModel);
        }

        public void CaptureCharge()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var charge = new APIClient().ChargeService.ChargeWithCard(cardCreateModel).Model;

            var chargeCaptureModel = TestHelper.GetChargeCaptureModel(charge.Value);
            new APIClient().ChargeService.CaptureCharge(charge.Id, chargeCaptureModel);
        }

        public void UpdateCharge()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            var charge = new APIClient().ChargeService.ChargeWithCard(cardCreateModel).Model;

            var chargeUpdateModel = TestHelper.GetChargeUpdateModel();

            new APIClient().ChargeService.UpdateCharge(charge.Id, chargeUpdateModel);
        }
        #endregion
    }
}
