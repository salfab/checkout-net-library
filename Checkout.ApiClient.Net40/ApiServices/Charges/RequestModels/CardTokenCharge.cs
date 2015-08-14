using System;

namespace Checkout.ApiServices.Charges.RequestModels
{
    public class CardTokenCharge : BaseCharge
    {
        public string TransactionIndicator { get; set; }
        public string CardToken { get; set; }
    }
}
