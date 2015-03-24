using Checkout.ApiServices.Customers.RequestModels;
using Checkout.ApiServices.Customers.ResponseModels;
using Checkout.ApiServices.SharedModels;
using Checkout.Utilities;
using System;
namespace Checkout.ApiServices.Customers
{
public class CustomerService  {
     
    public HttpResponse<Customer> CreateCustomer(CustomerCreate requestModel)
    {
        return new ApiHttpClient().PostRequest<Customer>(ApiUrls.CustomersApiUri, AppSettings.SecretKey, requestModel);
    }

    public HttpResponse<Customer> UpdateCustomer(CustomerUpdate requestModel)
    {
        var updateCustomerUri = string.Format("{0}/{1}", ApiUrls.CustomersApiUri, requestModel.CustomerId);
        return new ApiHttpClient().PutRequest<Customer>(updateCustomerUri, AppSettings.SecretKey, requestModel);
    }

    public HttpResponse<DeleteResponse> DeleteCustomer(string customerId)
    {
        var deleteCustomerUri = string.Format("{0}/{1}", ApiUrls.CustomersApiUri, customerId);
        return new ApiHttpClient().DeleteRequest<DeleteResponse>(deleteCustomerUri, AppSettings.SecretKey);
    }

    public HttpResponse<Customer> GetCustomer(string customerId)
    {
        var getCustomerUri = string.Format("{0}/{1}", ApiUrls.CustomersApiUri, customerId);
        return new ApiHttpClient().GetRequest<Customer>(getCustomerUri, AppSettings.SecretKey);
    }

    public HttpResponse<CustomerList> GetCustomerList(CustomerGetList request)
    {
        var getCustomerListUri = ApiUrls.CustomersApiUri;

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
            getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "fromDate", DateTimeHelper.FormatAsUtc(request.FromDate.Value));
        }

        if (request.ToDate.HasValue)
        {
            getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "fromDate", DateTimeHelper.FormatAsUtc(request.ToDate.Value));
        }

        return new ApiHttpClient().GetRequest<CustomerList>(getCustomerListUri, AppSettings.SecretKey);
    }
}

}