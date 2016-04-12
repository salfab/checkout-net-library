using System.Threading.Tasks;
using Checkout.ApiServices.SharedModels;
using Checkout.ApiServices.Tokens.RequestModels;
using Checkout.ApiServices.Tokens.ResponseModels;

namespace Checkout.ApiServices.Tokens
{
    public class TokenService : BaseService
    {
        public HttpResponse<PaymentToken> CreatePaymentToken(PaymentTokenCreate requestModel)
        {
            return ApiHttpClient.PostRequest<PaymentToken>(ApiUrls.PaymentToken, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<OkResponse> UpdatePaymentToken(string paymentToken, PaymentTokenUpdate requestModel)
        {
            var updatePaymentTokenUri = string.Format(ApiUrls.UpdatePaymentToken, paymentToken);
            return ApiHttpClient.PutRequest<OkResponse>(updatePaymentTokenUri, AppSettings.SecretKey, requestModel);
        }

        public async Task<HttpResponse<PaymentToken>> CreatePaymentTokenAsync(PaymentTokenCreate requestModel)
        {
            return await ApiHttpClient.PostRequestAsync<PaymentToken>(ApiUrls.PaymentToken, AppSettings.SecretKey, requestModel);
        }

        public async Task<HttpResponse<OkResponse>> UpdatePaymentTokenAsync(string paymentToken, PaymentTokenUpdate requestModel)
        {
            var updatePaymentTokenUri = string.Format(ApiUrls.UpdatePaymentToken, paymentToken);
            return await ApiHttpClient.PutRequestAsync<OkResponse>(updatePaymentTokenUri, AppSettings.SecretKey, requestModel);
        }
    }
}
