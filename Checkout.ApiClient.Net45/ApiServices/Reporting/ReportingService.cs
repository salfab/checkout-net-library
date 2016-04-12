using System.Threading.Tasks;
using Checkout.ApiServices.Reporting.RequestModels;
using Checkout.ApiServices.Reporting.ResponseModels;
using Checkout.ApiServices.SharedModels;

namespace Checkout.ApiServices.Reporting
{
    public class ReportingService : BaseService
    {
        /// <summary>
        /// Search for a customer’s transaction by a date range and then drill down using further filters.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public HttpResponse<GetTransactionList> QueryTransaction(QueryTransaction requestModel)
        {
            return ApiHttpClient.PostRequest<GetTransactionList>(ApiUrls.Reporting, AppSettings.SecretKey, requestModel);
        }

        /// <summary>
        /// Search for a customer’s transaction by a date range and then drill down using further filters.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<HttpResponse<GetTransactionList>> QueryTransactionAsync(QueryTransaction requestModel)
        {
            return await ApiHttpClient.PostRequestAsync<GetTransactionList>(ApiUrls.Reporting, AppSettings.SecretKey, requestModel);
        }
    }
}
