using Checkout.ApiServices.Cards.RequestModels;
using Checkout.ApiServices.Cards.ResponseModels;
using Checkout.ApiServices.SharedModels;
namespace Checkout.ApiServices.Cards
{
    public class CardService 
    {

        public HttpResponse<Card> CreateCard(CardCreate requestModel)
        {
            var createCardUri = string.Format(ApiUrls.CardsApiUri, requestModel.CustomerId);
            return new ApiHttpClient().PostRequest<Card>(createCardUri, AppSettings.SecretKey, requestModel);
        }
        
        public HttpResponse<Card> GetCard(string customerId, string cardId)
        {
            var getCardUri = string.Format(string.Concat(ApiUrls.CardsApiUri, "/{1}"), customerId, cardId);
            return new ApiHttpClient().GetRequest<Card>(getCardUri, AppSettings.SecretKey);
        }

        public HttpResponse<Card> UpdateCard(CardUpdate requestModel)
        {
            var updateCardUri = string.Format(string.Concat(ApiUrls.CardsApiUri, "/{1}"), requestModel.CustomerId, requestModel.CardId);
            return new ApiHttpClient().PutRequest<Card>(updateCardUri, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<DeleteResponse> DeleteCard(string customerId, string cardId)
        {
            var deleteCardUri = string.Format(string.Concat(ApiUrls.CardsApiUri, "/{1}"), customerId, cardId);
            return new ApiHttpClient().DeleteRequest<DeleteResponse>(deleteCardUri, AppSettings.SecretKey);
        }

        public HttpResponse<CardList> GetCardList(string customerId)
        {
            var getCardListUri = string.Format(ApiUrls.CardsApiUri, customerId);
            return new ApiHttpClient().GetRequest<CardList>(getCardListUri, AppSettings.SecretKey);
        }

    }
}