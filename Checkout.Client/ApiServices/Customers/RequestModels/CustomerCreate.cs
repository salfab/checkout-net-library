using Checkout.ApiServices.Cards.RequestModels;

namespace Checkout.ApiServices.Customers.RequestModels
{
    public class CustomerCreate : BaseCustomer
    {
        public BaseCardCreate Card { get; set; }

        /// <summary>
        /// Card Token
        /// </summary>
        public string Token { get; set; }
    }
}
