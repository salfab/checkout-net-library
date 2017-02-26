using System;
using System.Collections;
using System.Collections.ObjectModel;

using Checkout.ApiServices.Customers.RequestModels;
using Checkout.ApiServices.Customers.ResponseModels;
using Checkout.ApiServices.SharedModels;
using Checkout.ApiServices.ShoppingList.Model;

namespace Checkout.ApiServices.ShoppingList
{
    /// <summary>
    /// The .net wrapper for the ShoppingList API.
    /// </summary>
    /// <remarks>
    /// Paging was not implemented for time reasons. The approach that I would have taken however would have been different from the existing services.
    /// I probably would have created a Paginable<T> and make it implement IEnumerable<T>. That way, the api wrapper would have abstracted the pagination for the consumer.
    /// 
    /// In the current implementation, the consumer needs to be aware of the offsets to provide. Using an IEnumerable would allow us to transparently query the next page when elements
    /// can no longer be yielded from the payload that we already have retrieved.
    /// </remarks>
    public class ShoppingListService
    {
        private readonly BodylessResponseFriendlyPayloadDeserializer<ModelErrorCollection> bodylessResponseFriendlyPayloadDeserializer = new BodylessResponseFriendlyPayloadDeserializer<ModelErrorCollection>();

        public HttpResponse<Model.ShoppingList> GetOrderedDrinks()
        {
            return new ApiHttpClient(this.bodylessResponseFriendlyPayloadDeserializer).GetRequest<Model.ShoppingList>(ApiUrls.GetDrinks, AppSettings.SecretKey);
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