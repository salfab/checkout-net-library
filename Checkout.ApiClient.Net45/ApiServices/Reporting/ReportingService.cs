using Checkout.ApiServices.Reporting.RequestModels;
using Checkout.ApiServices.Reporting.ResponseModels;
using Checkout.ApiServices.SharedModels;

namespace Checkout.ApiServices.Reporting
{
    public class ReportingService
    {
        /// <summary>
        /// Search for a customer’s transaction by a date range and then drill down using further filters.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public HttpResponse<GetTransactionList> QueryTransaction(QueryTransaction requestModel)
        {
            return new ApiHttpClient().PostRequest<GetTransactionList>(ApiUrls.Reporting, AppSettings.SecretKey, requestModel);
        }
    }
}
