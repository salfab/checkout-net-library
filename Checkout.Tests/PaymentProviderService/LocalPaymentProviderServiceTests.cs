using NUnit.Framework;
using System;
using System.Linq;


namespace Tests
{
    [TestFixture(Category="ProvidersApi")]
    public class LocalPaymentProvidersApiTests
    {
        //[Test]
        //public void GetLocalPaymentProviderList()
        //{
        //    var paymentToken = new CheckoutClient().TokensApi.CreatePaymentToken(TestHelper.GetPaymentTokenCreateModel()).Model;

        //    var response = new CheckoutClient().PaymentProviderService.GetLocaPaymentProviderList(paymentToken.Id);

        //    Assert.NotNull(response);
        //    Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
        //    Assert.IsTrue(response.Model.Object.ToLower() == "list");
        //    Assert.IsTrue(response.Model.Data.Count > 0);

        //    foreach (var provider in response.Model.Data)
        //    {
        //        Assert.IsTrue(provider.Id.ToLower().StartsWith("lpp_"));
        //        Assert.IsNotNullOrEmpty(provider.Name);
        //    }
        //}

        //[Test]
        //public void GetLocalPaymentProvider()
        //{
        //    var paymentToken = new CheckoutClient().TokensApi.CreatePaymentToken(TestHelper.GetPaymentTokenCreateModel()).Model;

        //    var locaPaymentProvider = new CheckoutClient().PaymentProviderService.GetLocaPaymentProviderList(paymentToken.Id).Model.Data.First();

        //    var response = new CheckoutClient().PaymentProviderService.GetLocaPaymentProvider(locaPaymentProvider.Id, paymentToken.Id);

        //    Assert.NotNull(response);
        //    Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
        //    Assert.IsTrue(response.Model.Id.StartsWith("lpp_"));
        //    Assert.IsTrue(!string.IsNullOrEmpty(response.Model.Name));
        //}

    }
}
