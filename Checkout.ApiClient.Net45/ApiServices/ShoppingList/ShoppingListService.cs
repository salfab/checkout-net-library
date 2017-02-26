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
            return new ApiHttpClient(this.bodylessResponseFriendlyPayloadDeserializer).GetRequest<ResponseModel.ShoppingList>(ApiUrls.GetDrinks, AppSettings.SecretKey);
        }

        public HttpResponse<object> OrderDrink(DrinkOrder drinkOrder)
        {
            if (drinkOrder == null)
            {
                throw new ArgumentNullException(nameof(drinkOrder));
            }

            return new ApiHttpClient(this.bodylessResponseFriendlyPayloadDeserializer).PostRequest<object>(ApiUrls.OrderDrink, AppSettings.SecretKey, drinkOrder);
        }

        public HttpResponse<DrinkOrder> GetDrinkDetails(string drinkName)
        {
            if (drinkName == null)
            {
                throw new ArgumentNullException(nameof(drinkName));
            }

            return new ApiHttpClient(this.bodylessResponseFriendlyPayloadDeserializer).GetRequest<DrinkOrder>(ApiUrls.DrinkResourceLocation(drinkName), AppSettings.SecretKey);
        }

        public HttpResponse<DrinkOrderBase> UpdateDrink(DrinkOrder drinkOrder)
        {
            if (drinkOrder == null)
            {
                throw new ArgumentNullException(nameof(drinkOrder));
            }

            return new ApiHttpClient(this.bodylessResponseFriendlyPayloadDeserializer).PutRequest<DrinkOrderBase>(ApiUrls.DrinkResourceLocation(drinkOrder.Name), AppSettings.SecretKey, drinkOrder);
        }

        public HttpResponse<object> DeleteDrink(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new ApiHttpClient(this.bodylessResponseFriendlyPayloadDeserializer).DeleteRequest<object>(ApiUrls.DrinkResourceLocation(name), AppSettings.SecretKey);
        }
    }
}