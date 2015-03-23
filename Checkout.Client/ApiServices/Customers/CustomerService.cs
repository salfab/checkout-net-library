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

    public HttpResponse<CustomerList> GetCustomerList(int? count, int? offset, DateTime? startDate, DateTime? endDate,bool singleDay=false)
    {
        var getCustomerListUri = ApiUrls.CustomersApiUri;

        var fromDateString = startDate.HasValue ? DateTimeHelper.FormatAsUtc(startDate.Value): string.Empty;
        var toDateString = endDate.HasValue ? DateTimeHelper.FormatAsUtc(endDate.Value) : string.Empty;

        if (count.HasValue)
        {
            getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "count", count.Value.ToString());
        }

        if (offset.HasValue)
        {
            getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "offset", offset.Value.ToString());
        }

        if (startDate.HasValue && singleDay)
        {
            getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "fromDate", fromDateString);
        }else if(startDate.HasValue)
        {
            getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "fromDate", fromDateString);
        }

         if(endDate.HasValue)
        {
            getCustomerListUri = UrlHelper.AddParameterToUrl(getCustomerListUri, "fromDate", toDateString);
        }

        return new ApiHttpClient().GetRequest<CustomerList>(getCustomerListUri, AppSettings.SecretKey);
    }
}

}