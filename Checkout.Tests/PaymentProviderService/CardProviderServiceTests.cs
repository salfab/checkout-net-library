using Checkout;
using NUnit.Framework;
using System;
using System.Linq;


namespace Tests
{
    [TestFixture(Category="ProvidersApi")]
    public class CardProvidersApiTests
    {
        [Test]
        public void GetCardPaymentProviderList()
        {
            var response = new CheckoutClient().PaymentProviderService.GetCardProviderList();
          
            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Object.ToLower() == "list");
            Assert.IsTrue(response.Model.Data.Count > 0);

            foreach (var cardProvider in response.Model.Data)
            {
                Assert.IsTrue(cardProvider.Id.ToLower().StartsWith("cp_"));
                Assert.IsNotNullOrEmpty(cardProvider.Name);
            }
        }

        [Test]
        public void GetCardPaymentProvider()
        {
            var cardProvider = new CheckoutClient().PaymentProviderService.GetCardProviderList().Model.Data.First();

            var response = new CheckoutClient().PaymentProviderService.GetCardProvider(cardProvider.Id);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("cp_"));
            Assert.IsTrue(!string.IsNullOrEmpty(response.Model.Name));
        }
    }
}
