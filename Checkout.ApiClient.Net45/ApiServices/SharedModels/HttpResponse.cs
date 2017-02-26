using System.Net;

namespace Checkout.ApiServices.SharedModels
{
    /// <summary>
    /// Holds the response model
    /// </summary>
    /// <typeparam name="T">generic model returned from the api</typeparam>
    public class HttpResponse<T>
    {
        private object error;

        public bool HasError => this.error != null;

        public HttpStatusCode HttpStatusCode { get; set; }


        public T Model { get; set; }

        public HttpResponse(T model)
        {
            this.Model = model;
        }

        public void SetError<TError>(TError value)
        {
            this.error = value;
        }

        public TError GetError<TError>()
        {
            return (TError)this.error;
        }
    }    
}