using Checkout;
using Checkout.ApiServices.Customers.RequestModels;
using Checkout.ApiServices.SharedModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture(Category = "CustomersApi")]
    public class CustomersApiTests
    {
        APIClient CheckoutClient;
        
        [SetUp]
        public void Init()
        { CheckoutClient = new APIClient(); }

        [Test]
        public void CreateCustomerWithCard()
        {
            var customerCreateModel = TestHelper.GetCustomerCreateModelWithCard();
            var response = CheckoutClient.CustomerService.CreateCustomer(customerCreateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("cust_"));
            Assert.IsTrue(ReflectionHelper.CompareProperties(customerCreateModel, response.Model, new string[] { "Card" }));
            Assert.IsTrue(ReflectionHelper.CompareProperties(customerCreateModel.Card, response.Model.Cards.Data[0], new string[] { "Number", "Cvv", "DefaultCard" }));
        }

        [Test]
        public void CreateCustomerWithNoCard()
        {
            var customerCreateModel = TestHelper.GetCustomerCreateModelWithNoCard();
            var response = CheckoutClient.CustomerService.CreateCustomer(customerCreateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("cust_"));
            Assert.IsTrue(ReflectionHelper.CompareProperties(customerCreateModel, response.Model, new string[] { "Card" }));
        }

        [Test]
        public void GetCustomer()
        {
            var customerCreateModel = TestHelper.GetCustomerCreateModelWithCard();
            var customer = CheckoutClient.CustomerService.CreateCustomer(customerCreateModel).Model;

            var response = CheckoutClient.CustomerService.GetCustomer(customer.Id);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id == customer.Id);
            Assert.IsTrue(response.Model.Id.StartsWith("cust_"));
            Assert.IsTrue(ReflectionHelper.CompareProperties(customer, response.Model));
        }

        [Test]
        public void GetCustomerList()
        {
            var startTime = DateTime.UtcNow.AddHours(-1);// records for the past hour

            var customer1 = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard());
            var customer2 = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard());
            var customer3 = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard());
            var customer4 = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard());

            var custGetListRequest = new CustomerGetList()
            {
                FromDate = startTime,
                ToDate = DateTime.UtcNow
            };

            //Get all customers created
            var response = CheckoutClient.CustomerService.GetCustomerList(custGetListRequest);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Count >= 4);

            Assert.IsTrue(response.Model.Data[0].Id == customer4.Model.Id);
            Assert.IsTrue(response.Model.Data[1].Id == customer3.Model.Id);
            Assert.IsTrue(response.Model.Data[2].Id == customer2.Model.Id);
            Assert.IsTrue(response.Model.Data[3].Id == customer1.Model.Id);
        }

        [Test]
        public void UpdateCustomer()
        {
            var customer = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            var customerUpdateModel = TestHelper.GetCustomerUpdateModel();
            var response = CheckoutClient.CustomerService.UpdateCustomer(customer.Id, customerUpdateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Message.Equals("Ok", System.StringComparison.OrdinalIgnoreCase));
        }

         [Test]
        public void DeleteCustomer()
        {
            var customer = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            var response = CheckoutClient.CustomerService.DeleteCustomer(customer.Id);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Message.Equals("Ok", System.StringComparison.OrdinalIgnoreCase));
        }
        
    }
}
