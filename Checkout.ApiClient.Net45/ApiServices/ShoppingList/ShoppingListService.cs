using System.Collections;

using Checkout.ApiServices.Customers.RequestModels;
using Checkout.ApiServices.Customers.ResponseModels;
using Checkout.ApiServices.SharedModels;

namespace Checkout.ApiServices.ShoppingList
{
    public class ShoppingListService
    {
        public HttpResponse<ResponseModel.ShoppingList> GetOrderedDrinks()
        {
            return new ApiHttpClient().GetRequest<ResponseModel.ShoppingList>(ApiUrls.GetDrinks, AppSettings.SecretKey);
        }
    }
}