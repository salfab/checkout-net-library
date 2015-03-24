using Checkout;
using Checkout.ApiServices.Customers.RequestModels;
using Checkout.ApiServices.SharedModels;
using NUnit.Framework;
using System;

namespace Tests
{
    [TestFixture(Category = "CustomersApi")]
    public class CustomersApiTests
    {
        [Test]
        public void CreateCustomerWithCard()
        {
            var customerCreateModel = TestHelper.GetCustomerCreateModelWithCard();
            var response = new CheckoutClient().CustomerService.CreateCustomer(customerCreateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("cust_"));
            Assert.IsTrue(response.Model.Object.ToLower() == "customer");

            var isBillingAddressSame = ReflectionHelper.CompareProperties<Address>(response.Model.Cards.Data[0].BillingDetails, customerCreateModel.Card.BillingDetails);

            Assert.IsTrue(isBillingAddressSame);
        }

        [Test]
        public void CreateCustomerWithNoCard()
        {
            var customerCreateModel = TestHelper.GetCustomerCreateModelWithNoCard();
            var response = new CheckoutClient().CustomerService.CreateCustomer(customerCreateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("cust_"));
            Assert.IsTrue(response.Model.Object.ToLower() == "customer");
        }

        [Test]
        public void GetCustomer()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            var response = new CheckoutClient().CustomerService.GetCustomer(customer.Id);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id == customer.Id);
            Assert.IsTrue(response.Model.Object.ToLower() == "customer");
        }

        [Test]
        public void GetCustomerList()
        {
            var startTime = DateTime.UtcNow;

            var customer1 = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard());
            var customer2 = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard());
            var customer3 = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard());
            var customer4 = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard());

            var custGetListRequest = new CustomerGetList()
            {
                FromDate = startTime,
                ToDate = DateTime.UtcNow
            };

            //Get all customers created
            var response = new CheckoutClient().CustomerService.GetCustomerList(custGetListRequest);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Count == 4);

            Assert.IsTrue(response.Model.Data[0].Id == customer4.Model.Id);
            Assert.IsTrue(response.Model.Data[1].Id == customer3.Model.Id);
            Assert.IsTrue(response.Model.Data[2].Id == customer2.Model.Id);
            Assert.IsTrue(response.Model.Data[3].Id == customer1.Model.Id);
        }

        [Test]
        public void UpdateCustomer()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            var customerUpdateModel = TestHelper.GetCustomerUpdateModel(customer.Id);
            var response = new CheckoutClient().CustomerService.UpdateCustomer(customerUpdateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id == customer.Id);
            Assert.IsTrue(response.Model.Object.ToLower() == "customer");
            Assert.IsTrue(response.Model.Email == customerUpdateModel.Email);
            Assert.IsTrue(response.Model.Description == customerUpdateModel.Description);
            Assert.IsTrue(response.Model.PhoneNumber == customerUpdateModel.PhoneNumber);
        }

         [Test]
        public void DeleteCustomer()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;

            var response = new CheckoutClient().CustomerService.DeleteCustomer(customer.Id);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.Equals( customer.Id,StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Deleted);
        }
        
    }
}
