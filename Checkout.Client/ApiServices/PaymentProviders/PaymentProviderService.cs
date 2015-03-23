using Checkout.ApiServices.PaymentProviders.ResponseModels;
using Checkout.ApiServices.SharedModels;
namespace Checkout.ApiServices.PaymentProviders
{
    public class PaymentProviderService
    {
        public HttpResponse<CardProviderList> GetCardProviderList()
        {
            return new ApiHttpClient().GetRequest<CardProviderList>(ApiUrls.CardProvidersUri, AppSettings.PublicKey);
        }

        public HttpResponse<CardProvider> GetCardProvider(string id)
        {
            var cardProviderByIdUri = string.Format("{0}/{1}", ApiUrls.CardProvidersUri, id);

            return new ApiHttpClient().GetRequest<CardProvider>(cardProviderByIdUri, AppSettings.PublicKey);
        }

        #region Local Payment Providers
        //public HttpResponse<LocalPaymentProviderList> GetLocaPaymentProviderList(string paymentToken)
        //{
        //    var localPaymentProviderUri = UrlHelper.AddParameterToUrl(ApiUrls.LocalPaymentProvidersUri,"paymentToken",paymentToken);
        //    return ApiHttpClient.GetRequest<LocalPaymentProviderList>(localPaymentProviderUri, AppSettings.PublicKey);
        //}

        //public HttpResponse<LocalPaymentProvider> GetLocaPaymentProvider(string paymentProviderId, string paymentToken)
        //{
        //    string cardProviderUri = UrlHelper.AddParameterToUrl(string.Format("{0}/{1}", ApiUrls.LocalPaymentProvidersUri, paymentProviderId), "paymentToken", paymentToken);

        //    return ApiHttpClient.GetRequest<LocalPaymentProvider>(cardProviderUri, AppSettings.PublicKey);
        //}
        #endregion
    }
}