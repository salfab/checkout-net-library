using Checkout.ApiServices.SharedModels;
using Checkout.ApiServices.Tokens.RequestModels;
using Checkout.ApiServices.Tokens.ResponseModels;

namespace Checkout.ApiServices.Tokens
{
    public class TokenService 
    {
        public HttpResponse<PaymentToken> CreatePaymentToken(PaymentTokenCreate requestModel)
        {
            return new ApiHttpClient().PostRequest<PaymentToken>(ApiUrls.PaymentToken, AppSettings.SecretKey, requestModel);
        }
    }
}
