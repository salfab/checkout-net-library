using System;
using System.Collections;

using Checkout.ApiServices.Customers.RequestModels;
using Checkout.ApiServices.Customers.ResponseModels;
using Checkout.ApiServices.SharedModels;
using Checkout.ApiServices.ShoppingList.ResponseModel;

namespace Checkout.ApiServices.ShoppingList
{
    public class ShoppingListService
    {
        public HttpResponse<ResponseModel.ShoppingList> GetOrderedDrinks()
        {
            return new ApiHttpClient().GetRequest<ResponseModel.ShoppingList>(ApiUrls.GetDrinks, AppSettings.SecretKey);
        }

        public HttpResponse<object> OrderDrink(DrinkOrder drinkOrder)
        {            
            return new ApiHttpClient(new BodylessResponseFriendlyPayloadDeserializer<object>()).PostRequest<object>(ApiUrls.OrderDrink, AppSettings.SecretKey, drinkOrder);
        }
    }
}