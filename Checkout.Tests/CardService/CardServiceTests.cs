using Checkout;
using Checkout.ApiServices.SharedModels;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    [TestFixture(Category = "CardsApi")]
    public class CardServiceTests
    {
        APIClient CheckoutClient;

        [SetUp]
        public void Init()
        { CheckoutClient = new APIClient(); }

        [Test]
        public void CreateCard()
        {
            var customer = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithNoCard()).Model;

            var cardCreateModel = TestHelper.GetCardCreateModel();
            var response = CheckoutClient.CardService.CreateCard(customer.Id,cardCreateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("card_"));
            Assert.IsTrue(response.Model.CustomerId.Equals(customer.Id, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Name == cardCreateModel.Name);
            Assert.IsTrue(response.Model.ExpiryMonth == cardCreateModel.ExpiryMonth);
            Assert.IsTrue(response.Model.ExpiryYear == cardCreateModel.ExpiryYear);
            Assert.IsTrue(cardCreateModel.Number.EndsWith(response.Model.Last4));
            Assert.IsTrue(ReflectionHelper.CompareProperties(cardCreateModel.BillingDetails,response.Model.BillingDetails));
        }

        [Test]
        public void GetCard()
        {
            var customerCreateModel = TestHelper.GetCustomerCreateModelWithCard();
            var customer = CheckoutClient.CustomerService.CreateCustomer(customerCreateModel).Model;
            var customerCard = customer.Cards.Data.First();

            var response = CheckoutClient.CardService.GetCard(customer.Id, customerCard.Id);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id == customerCard.Id);
            Assert.IsTrue(response.Model.CustomerId.Equals(customer.Id, System.StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(response.Model.Name == customerCard.Name);
            Assert.IsTrue(response.Model.ExpiryMonth == customerCard.ExpiryMonth);
            Assert.IsTrue(response.Model.ExpiryYear == customerCard.ExpiryYear);
            Assert.IsTrue(customerCreateModel.Card.Number.EndsWith(response.Model.Last4));
            Assert.IsTrue(ReflectionHelper.CompareProperties(customerCard.BillingDetails, response.Model.BillingDetails));
        }

        [Test]
        public void GetCardList()
        {
            var customer = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithNoCard()).Model;
            var customerCard1 = CheckoutClient.CardService.CreateCard(customer.Id,TestHelper.GetCardCreateModel(Utils.CardProvider.Visa)).Model;
            var customerCard2 = CheckoutClient.CardService.CreateCard(customer.Id,TestHelper.GetCardCreateModel(Utils.CardProvider.Mastercard)).Model;

            var response = CheckoutClient.CardService.GetCardList(customer.Id);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Count == 2);
        }

        [Test]
        public void UpdateCard()
        {
            var customer = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var customerCardId = customer.Cards.Data.First().Id;

            var response = CheckoutClient.CardService.UpdateCard(customer.Id, customerCardId, TestHelper.GetCardUpdateModel());

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Message.Equals("Ok", System.StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void DeleteCard()
        {
            var customer = CheckoutClient.CustomerService.CreateCustomer(TestHelper.GetCustomerCreateModelWithCard()).Model;
            var customerCardId = customer.Cards.Data.First().Id;

            var response = CheckoutClient.CardService.DeleteCard(customer.Id, customerCardId);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Message.Equals("Ok", System.StringComparison.OrdinalIgnoreCase));
        }
        
    }
}
