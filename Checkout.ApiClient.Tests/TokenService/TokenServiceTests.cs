using NUnit.Framework;

namespace Tests
{
    [TestFixture(Category = "TokensApi")]
    public class TokenServiceTests : BaseService
    {
        [Test]
        public void CreatePaymentToken()
        {
            var paymentTokenCreateModel = TestHelper.GetPaymentTokenCreateModel(TestHelper.RandomData.Email);
            var response = CheckoutClient.TokenService.CreatePaymentToken(paymentTokenCreateModel);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Id.StartsWith("pay_tok_"));
        }

        [Test]
        public void UpdatePaymentToken()
        {
            var paymentTokenCreateModel = TestHelper.GetPaymentTokenCreateModel(TestHelper.RandomData.Email);
            var createResponse = CheckoutClient.TokenService.CreatePaymentToken(paymentTokenCreateModel);

            var paymentTokenUpdateModel = TestHelper.GetPaymentTokenUpdateModel();
            var updateResponse = CheckoutClient.TokenService.UpdatePaymentToken(createResponse.Model.Id, paymentTokenUpdateModel);

            Assert.NotNull(updateResponse);
            Assert.IsTrue(updateResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(updateResponse.Model.Message.Equals("Ok", System.StringComparison.OrdinalIgnoreCase));
        }
    }
}