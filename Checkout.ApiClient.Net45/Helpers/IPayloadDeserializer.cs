using System.Net;

using Checkout.ApiServices.SharedModels;
using Checkout.Helpers;

namespace Checkout
{
    public interface IPayloadDeserializer
    {
        HttpResponse<T> Deserialize<T>(string responseAsString, HttpStatusCode httpStatusCode);
    }

    public class BodylessResponseFriendlyPayloadDeserializer<TError> : PayloadDeserializerBase, IPayloadDeserializer
    {
        public HttpResponse<T> Deserialize<T>(string responseAsString, HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode == HttpStatusCode.OK && responseAsString != null)
            {
                return new HttpResponse<T>(GetResponseAsObject<T>(responseAsString))
                {
                    HttpStatusCode = httpStatusCode
                };
            }
            else if (responseAsString != null)
            {
                var httpResponse = new HttpResponse<T>(default(T))
                                       {                                           
                                           HttpStatusCode = httpStatusCode
                                       };
                httpResponse.SetError(this.GetResponseAsObject<TError>(responseAsString));
                return httpResponse;
            }
            else
            {
                var httpResponse = new HttpResponse<T>(default(T))
                                       {                                           
                                           HttpStatusCode = httpStatusCode
                                       };                
                return httpResponse;
            }
        }
    }

    public class PayloadDeserializerBase
    {
        protected T GetResponseAsObject<T>(string responseAsString)
        {
            return ContentAdaptor.JsonStringToObject<T>(responseAsString);
        }
    }

    public class CheckoutPayloadDeserializer : PayloadDeserializerBase, IPayloadDeserializer
    {
        public HttpResponse<T> Deserialize<T>(string responseAsString, HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode == HttpStatusCode.OK && responseAsString != null)
            {
                return new HttpResponse<T>(this.GetResponseAsObject<T>(responseAsString))
                {
                    HttpStatusCode = httpStatusCode
                };
            }
            else if (responseAsString != null)
            {
                var httpResponse = new HttpResponse<T>(default(T))
                                       {                    
                                           HttpStatusCode = httpStatusCode
                                       };
                httpResponse.SetError(this.GetResponseAsObject<ResponseError>(responseAsString));
                return httpResponse;
            }

            return null;
        }
    }
}