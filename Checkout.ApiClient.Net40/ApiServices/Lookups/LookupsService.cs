using Checkout.ApiServices.Lookups.ResponseModels;
using Checkout.ApiServices.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.ApiServices.Lookups
{
    public class LookupsService
    {
        public HttpResponse<CountryInfo> GetBinLookup(string bin)
        {
            var uri = string.Format(ApiUrls.BinLookup, bin);
            return new ApiHttpClient().GetRequest<CountryInfo>(uri, AppSettings.SecretKey);
        }
    }
}
