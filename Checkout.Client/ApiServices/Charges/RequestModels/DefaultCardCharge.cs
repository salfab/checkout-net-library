using Checkout.ApiServices.SharedModels;
using System.Collections.Generic;
namespace Checkout.ApiServices.Charges.RequestModels
{
    public class DefaultCardCharge : BaseCharge
    {
        public string TransactionIndicator { get; set; }
    }
}