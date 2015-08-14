using Checkout.ApiServices.Cards.RequestModels;

namespace Checkout.ApiServices.Charges.RequestModels
{
    public class CardCharge : BaseCharge
    {
        public string TransactionIndicator { get; set; }
        public CardCreate Card { get; set; }
    }
}

