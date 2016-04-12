using System.Threading.Tasks;
using Checkout.ApiServices.Cards.RequestModels;
using Checkout.ApiServices.Cards.ResponseModels;
using Checkout.ApiServices.SharedModels;

namespace Checkout.ApiServices.Cards
{
    public class CardService : BaseService
    {

        public HttpResponse<Card> CreateCard(string customerId, CardCreate requestModel)
        {
            var createCardUri = string.Format(ApiUrls.Cards, customerId);
            return ApiHttpClient.PostRequest<Card>(createCardUri, AppSettings.SecretKey, requestModel);
        }
        
        public HttpResponse<Card> GetCard(string customerId, string cardId)
        {
            var getCardUri = string.Format(ApiUrls.Card, customerId, cardId);
            return ApiHttpClient.GetRequest<Card>(getCardUri, AppSettings.SecretKey);
        }

        public HttpResponse<OkResponse> UpdateCard(string customerId, string cardId, CardUpdate requestModel)
        {
            var updateCardUri = string.Format(ApiUrls.Card, customerId, cardId);
            return ApiHttpClient.PutRequest<OkResponse>(updateCardUri, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<OkResponse> DeleteCard(string customerId, string cardId)
        {
            var deleteCardUri = string.Format(ApiUrls.Card, customerId, cardId);
            return ApiHttpClient.DeleteRequest<OkResponse>(deleteCardUri, AppSettings.SecretKey);
        }

        public HttpResponse<CardList> GetCardList(string customerId)
        {
            var getCardListUri = string.Format(ApiUrls.Cards, customerId);
            return ApiHttpClient.GetRequest<CardList>(getCardListUri, AppSettings.SecretKey);
        }

        public async Task<HttpResponse<Card>> CreateCardAsync(string customerId, CardCreate requestModel)
        {
            var createCardUri = string.Format(ApiUrls.Cards, customerId);
            return await ApiHttpClient.PostRequestAsync<Card>(createCardUri, AppSettings.SecretKey, requestModel);
        }

        public async Task<HttpResponse<Card>> GetCardAsync(string customerId, string cardId)
        {
            var getCardUri = string.Format(ApiUrls.Card, customerId, cardId);
            return await ApiHttpClient.GetRequestAsync<Card>(getCardUri, AppSettings.SecretKey);
        }

        public async Task<HttpResponse<OkResponse>> UpdateCardAsync(string customerId, string cardId, CardUpdate requestModel)
        {
            var updateCardUri = string.Format(ApiUrls.Card, customerId, cardId);
            return await ApiHttpClient.PutRequestAsync<OkResponse>(updateCardUri, AppSettings.SecretKey, requestModel);
        }

        public async Task<HttpResponse<OkResponse>> DeleteCardAsync(string customerId, string cardId)
        {
            var deleteCardUri = string.Format(ApiUrls.Card, customerId, cardId);
            return await ApiHttpClient.DeleteRequestAsync<OkResponse>(deleteCardUri, AppSettings.SecretKey);
        }

        public async Task<HttpResponse<CardList>> GetCardListAsync(string customerId)
        {
            var getCardListUri = string.Format(ApiUrls.Cards, customerId);
            return await ApiHttpClient.GetRequestAsync<CardList>(getCardListUri, AppSettings.SecretKey);
        }

    }
}