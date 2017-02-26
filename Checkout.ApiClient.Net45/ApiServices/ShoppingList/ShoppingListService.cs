using System;
using System.Collections;
using System.Collections.ObjectModel;

using Checkout.ApiServices.Customers.RequestModels;
using Checkout.ApiServices.Customers.ResponseModels;
using Checkout.ApiServices.SharedModels;
using Checkout.ApiServices.ShoppingList.ResponseModel;

namespace Checkout.ApiServices.ShoppingList
{
    public class ShoppingListService
    {
        private readonly BodylessResponseFriendlyPayloadDeserializer<ModelErrorCollection> bodylessResponseFriendlyPayloadDeserializer = new BodylessResponseFriendlyPayloadDeserializer<ModelErrorCollection>();

        public HttpResponse<ResponseModel.ShoppingList> GetOrderedDrinks()
        {
            return new ApiHttpClient().GetRequest<ResponseModel.ShoppingList>(ApiUrls.GetDrinks, AppSettings.SecretKey);
        }

        public HttpResponse<object> OrderDrink(DrinkOrder drinkOrder)
        {            
            return new ApiHttpClient(this.bodylessResponseFriendlyPayloadDeserializer).PostRequest<object>(ApiUrls.OrderDrink, AppSettings.SecretKey, drinkOrder);
        }

        public HttpResponse<DrinkOrder> GetDrinkDetails(string drinkName)
        {
            return new ApiHttpClient(this.bodylessResponseFriendlyPayloadDeserializer).GetRequest<DrinkOrder>(ApiUrls.GetDrink(drinkName), AppSettings.SecretKey);
        }

        public HttpResponse<object> UpdateDrink(DrinkOrder drinkOrder)
        {
            throw new NotImplementedException();
        }
    }
}