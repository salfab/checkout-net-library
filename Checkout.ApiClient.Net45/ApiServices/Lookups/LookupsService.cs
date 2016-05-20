using System.Threading.Tasks;
using Checkout.ApiServices.Lookups.ResponseModels;
using Checkout.ApiServices.SharedModels;

namespace Checkout.ApiServices.Lookups
{
    public class LookupsService
    {
        public HttpResponse<CountryInfo> GetBinLookup(string bin)
        {
            var uri = string.Format(ApiUrls.BinLookup, bin);
            return new ApiHttpClient().GetRequest<CountryInfo>(uri, AppSettings.SecretKey);
        }
        public async Task<HttpResponse<CountryInfo>> GetBinLookupAsync(string bin)
        {
            var uri = string.Format(ApiUrls.BinLookup, bin);
            return await new ApiHttpClient().GetRequestAsync<CountryInfo>(uri, AppSettings.SecretKey);
        }
    }
}
