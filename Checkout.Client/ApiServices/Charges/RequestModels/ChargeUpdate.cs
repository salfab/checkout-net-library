using System.Collections.Generic;

namespace Checkout.ApiServices.Charges.RequestModels
{
    public class ChargeUpdate
    {
        public string ChargeId { get; set; }

        public string Description { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }
}
