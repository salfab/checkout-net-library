using System.Threading.Tasks;
using Checkout.ApiServices.Customers.RequestModels;
using Checkout.ApiServices.Customers.ResponseModels;
using Checkout.ApiServices.SharedModels;
using Checkout.Utilities;

namespace Checkout.ApiServices.Customers
{
    public class CustomerService :BaseService
    {
        public HttpResponse<Customer> CreateCustomer(CustomerCreate requestModel)
        {
            return ApiHttpClient.PostRequest<Customer>(ApiUrls.Customers, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<OkResponse> UpdateCustomer(string customerId, CustomerUpdate requestModel)
        {
            var updateCustomerUri = string.Format(ApiUrls.Customer, customerId);
            return ApiHttpClient.PutRequest<OkResponse>(updateCustomerUri, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<OkResponse> DeleteCustomer(string customerId)
        {
            var deleteCustomerUri = string.Format(ApiUrls.Customer, customerId);
            return ApiHttpClient.DeleteRequest<OkResponse>(deleteCustomerUri, AppSettings.SecretKey);
        }

        public HttpResponse<Customer> GetCustomer(string customerId)
        {
            var getCustomerUri = string.Format(ApiUrls.Customer, customerId);
            return ApiHttpClient.GetRequest<Customer>(getCustomerUri, AppSettings.SecretKey);
        }

        public HttpResponse<CustomerList> GetCustomerList(CustomerGetList request)
        {
            var getCustomerListUri = ApiUrls.Customers;

            if (request.Count.HasValue)
            {
                getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "count", request.Count.ToString());
            }

            if (request.Offset.HasValue)
            {
                getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "offset", request.Offset.ToString());
            }

            if (request.FromDate.HasValue)
            {
                getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "fromDate",
                    DateTimeHelper.FormatAsUtc(request.FromDate.Value));
            }

            if (request.ToDate.HasValue)
            {
                getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "toDate",
                    DateTimeHelper.FormatAsUtc(request.ToDate.Value));
            }

            return ApiHttpClient.GetRequest<CustomerList>(getCustomerListUri, AppSettings.SecretKey);
        }

        public async Task<HttpResponse<Customer>> CreateCustomerAsync(CustomerCreate requestModel)
        {
            return await ApiHttpClient.PostRequestAsync<Customer>(ApiUrls.Customers, AppSettings.SecretKey, requestModel);
        }

        public async Task<HttpResponse<OkResponse>> UpdateCustomerAsync(string customerId, CustomerUpdate requestModel)
        {
            var updateCustomerUri = string.Format(ApiUrls.Customer, customerId);
            return await ApiHttpClient.PutRequestAsync<OkResponse>(updateCustomerUri, AppSettings.SecretKey, requestModel);
        }

        public async Task<HttpResponse<OkResponse>> DeleteCustomerAsync(string customerId)
        {
            var deleteCustomerUri = string.Format(ApiUrls.Customer, customerId);
            return await ApiHttpClient.DeleteRequestAsync<OkResponse>(deleteCustomerUri, AppSettings.SecretKey);
        }

        public async Task<HttpResponse<Customer>> GetCustomerAsync(string customerId)
        {
            var getCustomerUri = string.Format(ApiUrls.Customer, customerId);
            return await ApiHttpClient.GetRequestAsync<Customer>(getCustomerUri, AppSettings.SecretKey);
        }

        public async Task<HttpResponse<CustomerList>> GetCustomerListAsync(CustomerGetList request)
        {
            var getCustomerListUri = ApiUrls.Customers;

            if (request.Count.HasValue)
            {
                getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "count", request.Count.ToString());
            }

            if (request.Offset.HasValue)
            {
                getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "offset", request.Offset.ToString());
            }

            if (request.FromDate.HasValue)
            {
                getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "fromDate",
                    DateTimeHelper.FormatAsUtc(request.FromDate.Value));
            }

            if (request.ToDate.HasValue)
            {
                getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "toDate",
                    DateTimeHelper.FormatAsUtc(request.ToDate.Value));
            }

            return await ApiHttpClient.GetRequestAsync<CustomerList>(getCustomerListUri, AppSettings.SecretKey);
        }
    }
}