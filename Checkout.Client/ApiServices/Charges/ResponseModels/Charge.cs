using Checkout.ApiServices.Cards.ResponseModels;
using Checkout.ApiServices.SharedModels;
using System;
using System.Collections.Generic;

namespace Checkout.ApiServices.Charges.ResponseModels
{
    public class Charge
    {
        public string Object { get; set; }

        public string Id { get; set; }

        public bool LiveMode { get; set; }

        public string Created { get; set; }

        public decimal Value { get; set; }

        public string Currency { get; set; }

        public string Email { get; set; }

        public bool Paid { get; set; }

        public IDictionary<string, string> LocalPayment { get; set; }

        public Card Card { get; set; }
        
        public IList<Refund> Refunds { get; set; }
        
        public string ResponseMessage { get; set; }

        public string ResponseAdvancedInfo { get; set; }

        public string ResponseCode { get; set; }

        public string RefundedValue { get; set; }
        
        public string Description { get; set; }

        public IDictionary<string, string> Metadata { get; set; }

        public string BalanceTransaction { get; set; }

        public string Status { get; set; }

        public bool IsCascaded { get; set; }

        public string AuthCode { get; set; }

        public ShippingAddress ShippingDetails { get; set; }

        public string AutoCapture { get; set; }

        public decimal AutoCapTime { get; set; }

        public IList<Product> Products { get; set; }

        public int ChargeModel { get; set; }
    }
}