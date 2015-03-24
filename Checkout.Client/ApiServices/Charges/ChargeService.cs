
using Checkout.ApiServices.Charges.RequestModels;
using Checkout.ApiServices.Charges.ResponseModels;
using Checkout.ApiServices.SharedModels;
using Checkout.Utilities;
namespace Checkout.ApiServices.Charges
{
    public class ChargeService 
    {

        /// <summary>
        /// Creates a charge with full card details.
        /// </summary>
        /// <param name="requestModel">CardChargeCreateModel</param>
        /// <returns>ChargeResponseModel</returns>
        public HttpResponse<Charge> ChargeWithCard(CardChargeCreate requestModel)
        {
            return new ApiHttpClient().PostRequest<Charge>(ApiUrls.CardChargesApiUri, AppSettings.SecretKey,requestModel);
        }

        /// <summary>
        /// Creates a charge with card id.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>ChargeResponseModel</returns>
        public HttpResponse<Charge> ChargeWithCardId(CardIdChargeCreate requestModel)
        {
            return new ApiHttpClient().PostRequest<Charge>(ApiUrls.CardChargesApiUri, AppSettings.SecretKey, requestModel);
        }

        ///// <summary>
        ///// Creates a charge with card token.
        ///// </summary>
        ///// <param name="requestModel"></param>
        ///// <returns>ChargeResponseModel</returns>
        //public HttpResponse<Charge> ChargeWithCardToken(CardTokenChargeCreate requestModel)
        //{
        //    return new ApiHttpClient().PostRequest<Charge>(ApiUrls.CardTokenChargesApiUri, AppSettings.SecretKey, requestModel);
        //}

        /// <summary>
        /// Creates a charge with the default card of the customer.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>ChargeResponseModel</returns>
        public HttpResponse<Charge> ChargeWithDefaultCustomerCard(BaseCharge requestModel)
        {
            return new ApiHttpClient().PostRequest<Charge>(ApiUrls.DefaultCardChargesApiUri, AppSettings.SecretKey, requestModel);
        } 


        public HttpResponse<Charge> RefundCardChargeRequest(ChargeRefund requestModel)
        {
            var chargeRefundsApiUri = string.Format(ApiUrls.ChargeRefundsApiUri, requestModel.ChargeId);
            return new ApiHttpClient().PostRequest<Charge>(chargeRefundsApiUri, AppSettings.SecretKey, requestModel);
        }


        public HttpResponse<Charge> CaptureCardCharge(ChargeCapture requestModel)
        {
            var captureChargesApiUri = string.Format(ApiUrls.CaptureChargesApiUri, requestModel.ChargeId);
            return new ApiHttpClient().PostRequest<Charge>(captureChargesApiUri, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<Charge> UpdateCardCharge(ChargeUpdate requestModel)
        {
            var updateChargesApiUri = string.Format(ApiUrls.UpdateChargesApiUri, requestModel.ChargeId);
            return new ApiHttpClient().PutRequest<Charge>(updateChargesApiUri, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<ChargeList> GetChargeList(ChargeGetList request)
        {
            var getChargeListUri = ApiUrls.ChargesApiUri;

            if (request.Count.HasValue)
            {
                getChargeListUri = UrlHelper.AddParameterToUrl(getChargeListUri, "count", request.Count.ToString());
            }

            if (request.Offset.HasValue)
            {
                getChargeListUri = UrlHelper.AddParameterToUrl(getChargeListUri, "offset", request.Offset.ToString());
            }

            if (request.FromDate.HasValue)
            {
                getChargeListUri = UrlHelper.AddParameterToUrl(getChargeListUri, "fromDate", DateTimeHelper.FormatAsUtc(request.FromDate.Value));
            }

            if (request.ToDate.HasValue)
            {
                getChargeListUri = UrlHelper.AddParameterToUrl(getChargeListUri, "fromDate", DateTimeHelper.FormatAsUtc(request.ToDate.Value));
            }

            return new ApiHttpClient().GetRequest<ChargeList>(getChargeListUri, AppSettings.SecretKey);
        }

        public HttpResponse<Charge> GetCharge(string chargeId)
        {
            var getChargeUri = string.Format("{0}/{1}", ApiUrls.ChargesApiUri, chargeId);
            return new ApiHttpClient().GetRequest<Charge>(getChargeUri, AppSettings.SecretKey);
        }
    }
}