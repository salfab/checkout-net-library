using Checkout.ApiServices.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Checkout.ApiServices.Charges.RequestModels
{
    public class BaseCharge
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CustomerId { get; set; }

        public string Currency;

        /// <summary>
        /// Charge amount e.g. the value should be 100 for 1$
        /// </summary>
        public int Value;

        public string Description { get; set; }

        public string AutoCapture { get; set; }

        public decimal AutoCapTime { get; set; }

        public ShippingAddress ShippingDetails { get; set; }

        public IList<Product> Products { get; set; }

        public Dictionary<string, string> Metadata { get; set; }       
    }
}