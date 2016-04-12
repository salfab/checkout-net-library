using System.Threading.Tasks;
using Checkout.ApiServices.RecurringPayments.RequestModels;
using Checkout.ApiServices.RecurringPayments.ResponseModels;
using Checkout.ApiServices.SharedModels;

namespace Checkout.ApiServices.RecurringPayments
{
    public class RecurringPaymentsService : BaseService
    {
        /// <summary>
        /// Creates a Payment Plan that can exist independently and act as template. 
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public HttpResponse<SinglePaymentPlanCreateResponse> CreatePaymentPlan(SinglePaymentPlanCreateRequest requestModel)
        {
            return ApiHttpClient.PostRequest<SinglePaymentPlanCreateResponse>(ApiUrls.RecurringPaymentPlans, AppSettings.SecretKey, requestModel);
        }

        /// <summary>
        /// Updates an existing Payment Plan to the amended values of the parameters passed in the request.
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public HttpResponse<OkResponse> UpdatePaymentPlan(string planId, PaymentPlanUpdate requestModel)
        {
            return ApiHttpClient.PutRequest<OkResponse>(string.Format(ApiUrls.RecurringPaymentPlan, planId), AppSettings.SecretKey, requestModel);
        }

        /// <summary>
        /// Cancels an existing Payment Plan.
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public HttpResponse<OkResponse> CancelPaymentPlan(string planId)
        {
            return ApiHttpClient.DeleteRequest<OkResponse>(string.Format(ApiUrls.RecurringPaymentPlan, planId), AppSettings.SecretKey);
        }

        /// <summary>
        ///  Retrieves an existing Payment Plan.
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public HttpResponse<PaymentPlan> GetPaymentPlan(string planId)
        {
            return ApiHttpClient.GetRequest<PaymentPlan>(string.Format(ApiUrls.RecurringPaymentPlan, planId), AppSettings.SecretKey);
        }

        /// <summary>
        /// Searches for all Payment Plans created under a business based on the parameters passed in the request.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public HttpResponse<QueryPaymentPlanResponse> QueryPaymentPlan(QueryPaymentPlanRequest requestModel)
        {
            return ApiHttpClient.PostRequest<QueryPaymentPlanResponse>(ApiUrls.RecurringPaymentPlanSearch, AppSettings.SecretKey, requestModel);
        }

        /// <summary>
        /// Searches for all customers and their associated Payment Plans created under a business based on the parameters passed in the request.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public HttpResponse<QueryCustomerPaymentPlanResponse> QueryCustomerPaymentPlan(QueryCustomerPaymentPlanRequest requestModel)
        {
            return ApiHttpClient.PostRequest<QueryCustomerPaymentPlanResponse>(ApiUrls.RecurringCustomerPaymentPlanSearch, AppSettings.SecretKey, requestModel);
        }

        /// <summary>
        /// Retrieves an existing Customer Payment Plan.
        /// </summary>
        /// <param name="customerPlanId"></param>
        /// <returns></returns>
        public HttpResponse<CustomerPaymentPlan> GetCustomerPaymentPlan(string customerPlanId)
        {
            return ApiHttpClient.GetRequest<CustomerPaymentPlan>(string.Format(ApiUrls.RecurringCustomerPaymentPlan, customerPlanId), AppSettings.SecretKey);
        }

        /// <summary>
        /// Cancels an existing Customer Payment Plan.
        /// </summary>
        /// <param name="customerPlanId"></param>
        /// <returns></returns>
        public HttpResponse<OkResponse> CancelCustomerPaymentPlan(string customerPlanId)
        {
            return ApiHttpClient.DeleteRequest<OkResponse>(string.Format(ApiUrls.RecurringCustomerPaymentPlan, customerPlanId), AppSettings.SecretKey);
        }

        /// <summary>
        /// Updates the card associated with the Customer Payment Plan and/or its status
        /// </summary>
        /// <param name="customerPlanId"></param>
        /// <returns></returns>
        public HttpResponse<OkResponse> UpdateCustomerPaymentPlan(string customerPlanId, CustomerPaymentPlanUpdate requestModel)
        {
            return ApiHttpClient.PutRequest<OkResponse>(string.Format(ApiUrls.RecurringCustomerPaymentPlan, customerPlanId), AppSettings.SecretKey, requestModel);
        }

        /// <summary>
        /// Creates a Payment Plan asynchronously that can exist independently and act as template. 
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<HttpResponse<SinglePaymentPlanCreateResponse>> CreatePaymentPlanAsync(SinglePaymentPlanCreateRequest requestModel)
        {
            return await ApiHttpClient.PostRequestAsync<SinglePaymentPlanCreateResponse>(ApiUrls.RecurringPaymentPlans, AppSettings.SecretKey, requestModel);
        }

        /// <summary>
        /// Updates an existing Payment Plan asynchronously to the amended values of the parameters passed in the request.
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public async Task<HttpResponse<OkResponse>> UpdatePaymentPlanAsync(string planId, PaymentPlanUpdate requestModel)
        {
            return await ApiHttpClient.PutRequestAsync<OkResponse>(string.Format(ApiUrls.RecurringPaymentPlan, planId), AppSettings.SecretKey, requestModel);
        }

        /// <summary>
        /// Cancels an existing Payment Plan asynchronously.
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public async Task<HttpResponse<OkResponse>> CancelPaymentPlanAsync(string planId)
        {
            return await ApiHttpClient.DeleteRequestAsync<OkResponse>(string.Format(ApiUrls.RecurringPaymentPlan, planId), AppSettings.SecretKey);
        }

        /// <summary>
        ///  Retrieves an existing Payment Plan asynchronously.
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public async Task<HttpResponse<PaymentPlan>> GetPaymentPlanAsync(string planId)
        {
            return await ApiHttpClient.GetRequestAsync<PaymentPlan>(string.Format(ApiUrls.RecurringPaymentPlan, planId), AppSettings.SecretKey);
        }

        /// <summary>
        /// Searches for all Payment Plans asynchronously created under a business based on the parameters passed in the request.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<HttpResponse<QueryPaymentPlanResponse>> QueryPaymentPlanAsync(QueryPaymentPlanRequest requestModel)
        {
            return await ApiHttpClient.PostRequestAsync<QueryPaymentPlanResponse>(ApiUrls.RecurringPaymentPlanSearch, AppSettings.SecretKey, requestModel);
        }

        /// <summary>
        /// Searches for all customers and their associated Payment Plans asynchronously created under a business based on the parameters passed in the request.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<HttpResponse<QueryCustomerPaymentPlanResponse>> QueryCustomerPaymentPlanAsync(QueryCustomerPaymentPlanRequest requestModel)
        {
            return await ApiHttpClient.PostRequestAsync<QueryCustomerPaymentPlanResponse>(ApiUrls.RecurringCustomerPaymentPlanSearch, AppSettings.SecretKey, requestModel);
        }

        /// <summary>
        /// Retrieves an existing Customer Payment Plan asynchronously.
        /// </summary>
        /// <param name="customerPlanId"></param>
        /// <returns></returns>
        public async Task<HttpResponse<CustomerPaymentPlan>> GetCustomerPaymentPlanAsync(string customerPlanId)
        {
            return await ApiHttpClient.GetRequestAsync<CustomerPaymentPlan>(string.Format(ApiUrls.RecurringCustomerPaymentPlan, customerPlanId), AppSettings.SecretKey);
        }

        /// <summary>
        /// Cancels an existing Customer Payment Plan asynchronously.
        /// </summary>
        /// <param name="customerPlanId"></param>
        /// <returns></returns>
        public async Task<HttpResponse<OkResponse>> CancelCustomerPaymentPlanAsync(string customerPlanId)
        {
            return await ApiHttpClient.DeleteRequestAsync<OkResponse>(string.Format(ApiUrls.RecurringCustomerPaymentPlan, customerPlanId), AppSettings.SecretKey);
        }

        /// <summary>
        /// Updates the card associated with the Customer Payment Plan asynchronously and/or its status
        /// </summary>
        /// <param name="customerPlanId"></param>
        /// <returns></returns>
        public async Task<HttpResponse<OkResponse>> UpdateCustomerPaymentPlanAsync(string customerPlanId, CustomerPaymentPlanUpdate requestModel)
        {
            return await ApiHttpClient.PutRequestAsync<OkResponse>(string.Format(ApiUrls.RecurringCustomerPaymentPlan, customerPlanId), AppSettings.SecretKey, requestModel);
        }
    }
}
