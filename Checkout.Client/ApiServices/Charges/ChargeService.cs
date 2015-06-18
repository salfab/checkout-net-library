
using Checkout.ApiServices.Charges.RequestModels;
using Checkout.ApiServices.Charges.ResponseModels;
using Checkout.ApiServices.SharedModels;
using Checkout.Utilities;
namespace Checkout.ApiServices.Charges
{
    public class ChargeService 
    {
        public HttpResponse<Charge> VerifyCharge(string paymentToken) {
		
		string chargeVerifyApiUri= string.Format(ApiUrls.Charge, paymentToken);

        return new ApiHttpClient().GetRequest<Charge>(chargeVerifyApiUri, AppSettings.SecretKey);
	}


        /// <summary>
        /// Creates a charge with full card details.
        /// </summary>
        /// <param name="requestModel">CardChargeCreateModel</param>
        /// <returns>ChargeResponseModel</returns>
        public HttpResponse<Charge> ChargeWithCard(CardCharge requestModel)
        {
            return new ApiHttpClient().PostRequest<Charge>(ApiUrls.CardCharge, AppSettings.SecretKey,requestModel);
        }

        /// <summary>
        /// Creates a charge with card id.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>ChargeResponseModel</returns>
        public HttpResponse<Charge> ChargeWithCardId(CardIdCharge requestModel)
        {
            return new ApiHttpClient().PostRequest<Charge>(ApiUrls.CardCharge, AppSettings.SecretKey, requestModel);
        }

        /// <summary>
        /// Creates a charge with card token.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>ChargeResponseModel</returns>
        public HttpResponse<Charge> ChargeWithCardToken(CardTokenCharge requestModel)
        {
            return new ApiHttpClient().PostRequest<Charge>(ApiUrls.CardTokenCharge, AppSettings.SecretKey, requestModel);
        }

        /// <summary>
        /// Creates a charge with the default card of the customer.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>ChargeResponseModel</returns>
        public HttpResponse<Charge> ChargeWithDefaultCustomerCard(DefaultCardCharge requestModel)
        {
            return new ApiHttpClient().PostRequest<Charge>(ApiUrls.DefaultCardCharge, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<Void> VoidCharge(string chargeId, ChargeVoid requestModel)
        {
            var chargeRefundsApiUri = string.Format(ApiUrls.VoidCharge, chargeId);
            return new ApiHttpClient().PostRequest<Void>(chargeRefundsApiUri, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<Refund> RefundCharge(string chargeId,ChargeRefund requestModel)
        {
            var chargeRefundsApiUri = string.Format(ApiUrls.RefundCharge, chargeId);
            return new ApiHttpClient().PostRequest<Refund>(chargeRefundsApiUri, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<Capture> CaptureCharge(string chargeId, ChargeCapture requestModel)
        {
            var captureChargesApiUri = string.Format(ApiUrls.CaptureCharge, chargeId);
            return new ApiHttpClient().PostRequest<Capture>(captureChargesApiUri, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<OkResponse> UpdateCharge(string chargeId, ChargeUpdate requestModel)
        {
            var updateChargesApiUri = string.Format(ApiUrls.Charge, chargeId);
            return new ApiHttpClient().PutRequest<OkResponse>(updateChargesApiUri, AppSettings.SecretKey, requestModel);
        }

        //public HttpResponse<ChargeList> GetChargeList(ChargeGetList request)
        //{
        //    var getChargeListUri = ApiUrls.ChargesApiUri;

        //    if (request.Count.HasValue)
        //    {
        //        getChargeListUri = UrlHelper.AddParameterToUrl(getChargeListUri, "count", request.Count.ToString());
        //    }

        //    if (request.Offset.HasValue)
        //    {
        //        getChargeListUri = UrlHelper.AddParameterToUrl(getChargeListUri, "offset", request.Offset.ToString());
        //    }

        //    if (request.FromDate.HasValue)
        //    {
        //        getChargeListUri = UrlHelper.AddParameterToUrl(getChargeListUri, "fromDate", DateTimeHelper.FormatAsUtc(request.FromDate.Value));
        //    }

        //    if (request.ToDate.HasValue)
        //    {
        //        getChargeListUri = UrlHelper.AddParameterToUrl(getChargeListUri, "fromDate", DateTimeHelper.FormatAsUtc(request.ToDate.Value));
        //    }

        //    return new ApiHttpClient().GetRequest<ChargeList>(getChargeListUri, AppSettings.SecretKey);
        //}

        public HttpResponse<Charge> GetCharge(string chargeId)
        {
            var getChargeUri = string.Format(ApiUrls.Charge, chargeId);
            return new ApiHttpClient().GetRequest<Charge>(getChargeUri, AppSettings.SecretKey);
        }
    }
}