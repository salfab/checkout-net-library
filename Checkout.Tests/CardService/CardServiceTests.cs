using Checkout;
using Checkout.ApiServices.SharedModels;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    [TestFixture(Category = "CardsApi")]
    public class CardServiceTests
    {
       
        [Test]
        public void CreateCard()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithNoCard()).Model;

            var cardCreateModel = TestHelper.GetCardCreateModel(customer.Id);
            var response = new CheckoutClient().CardService.CreateCard(cardCreateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("card_"));
            Assert.IsTrue(response.Model.Object.ToLower() == "card");
            Assert.IsTrue(response.Model.CustomerId.Equals(cardCreateModel.CustomerId, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Name == cardCreateModel.Card.Name);
            Assert.IsTrue(response.Model.ExpiryMonth == cardCreateModel.Card.ExpiryMonth);
            Assert.IsTrue(response.Model.ExpiryYear == cardCreateModel.Card.ExpiryYear);
            Assert.IsTrue(cardCreateModel.Card.Number.EndsWith(response.Model.Last4));

            var isBillingAddressSame = ReflectionHelper.CompareProperties<Address>(response.Model.BillingDetails, cardCreateModel.Card.BillingDetails);
            Assert.IsTrue(isBillingAddressSame);
        }

        [Test]
        public void GetCard()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var customerCardId = customer.Cards.Data.First().Id;

            var response = new CheckoutClient().CardService.GetCard(customer.Id, customerCardId);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id == customerCardId);
            Assert.IsTrue(response.Model.Object.ToLower() == "card");
        }

        [Test]
        public void GetCardList()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithNoCard()).Model;
            var customerCard1 = new CheckoutClient().CardService.CreateCard(TestHelper.GetCardCreateModel(customer.Id,Utils.CardProvider.Visa)).Model;
            var customerCard2 = new CheckoutClient().CardService.CreateCard(TestHelper.GetCardCreateModel(customer.Id, Utils.CardProvider.Mastercard)).Model;

            var response = new CheckoutClient().CardService.GetCardList(customer.Id);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Count == 2);
        }

        [Test]
        public void UpdateCard()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var customerCardId = customer.Cards.Data.First().Id;

            var cardUpdateModel = TestHelper.GetCardUpdateModel(customer.Id, customerCardId);
            var response = new CheckoutClient().CardService.UpdateCard(cardUpdateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Name == cardUpdateModel.Card.Name);

            var isBillingAddressSame = ReflectionHelper.CompareProperties<Address>(response.Model.BillingDetails, cardUpdateModel.Card.BillingDetails);
            Assert.IsTrue(isBillingAddressSame);
        }

        [Test]
        public void DeleteCard()
        {
            var customer = new CheckoutClient().CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var customerCardId = customer.Cards.Data.First().Id;

            var response = new CheckoutClient().CardService.DeleteCard(customer.Id, customerCardId);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.Equals(customerCardId, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Deleted);
        }
        
    }
}
