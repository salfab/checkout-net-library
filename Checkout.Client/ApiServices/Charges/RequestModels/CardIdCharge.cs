using System;
namespace Checkout.ApiServices.Charges.RequestModels
{
    public class CardIdCharge : BaseCharge
    {
        public string TransactionIndicator { get; set; }
        public string CardId { get; set; }
    }
}