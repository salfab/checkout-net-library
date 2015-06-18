using Checkout.ApiServices.Charges.RequestModels;

namespace Checkout.ApiServices.Tokens.RequestModels
{
    public class CardTokenCharge:BaseCharge
    {
        public string TransactionIndicator { get; set; }
        public string CardToken { get; set; }
    }
}
