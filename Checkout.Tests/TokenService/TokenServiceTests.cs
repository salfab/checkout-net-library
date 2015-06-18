using Checkout;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.TokenService
{
  
    namespace Tests
    {
        [TestFixture(Category = "TokensApi")]
        public class TokenServiceTests
        {
            APIClient CheckoutClient;

            [SetUp]
            public void Init()
            { CheckoutClient = new APIClient(); }

            [Test]
            public void CreatePaymentToken()
            {
                //var customerCreateModel = TestHelper.GetCustomerCreateModelWithCard();
                //var response = CheckoutClient.CustomerService.CreateCustomer(customerCreateModel);

                //Assert.NotNull(response);
                //Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
                //Assert.IsTrue(response.Model.Id.StartsWith("cust_"));
                //Assert.IsTrue(ReflectionHelper.CompareProperties(customerCreateModel, response.Model, new string[] { "Card" }));
                //Assert.IsTrue(ReflectionHelper.CompareProperties(customerCreateModel.Card, response.Model.Cards.Data[0], new string[] { "Number", "Cvv", "DefaultCard" }));
            }
        }
    }

}
