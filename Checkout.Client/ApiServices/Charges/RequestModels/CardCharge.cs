using Checkout.ApiServices.Cards.RequestModels;

namespace Checkout.ApiServices.Charges.RequestModels
{
    public class CardChargeCreate : BaseCharge
    {
        public BaseCardCreate Card { get; set; }
    }
}

