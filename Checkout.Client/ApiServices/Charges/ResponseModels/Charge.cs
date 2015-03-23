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

        public DateTime Created { get; set; }

        public decimal Value { get; set; }

        public string Currency { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public string ResponseMessage { get; set; }

        public string ResponseAdvancedInfo { get; set; }

        public string ResponseCode { get; set; }

        public string RefundedValue { get; set; }

        public string BalanceTransaction { get; set; }

        public string Status { get; set; }

        public string AuthCode { get; set; }

        public string AutoCapture { get; set; }

        public decimal AutoCapTime { get; set; }

        public bool Paid { get; set; }

        public bool Refunded { get; set; }

        public bool Deferred { get; set; }

        public bool Expired { get; set; }

        public bool Captured { get; set; }

        public bool IsCascaded { get; set; }

        public Card Card { get; set; }

        public ShippingAddress ShippingDetails { get; set; }

        public IList<Product> Products { get; set; }

        public IList<Refund> Refunds { get; set; }

        public IDictionary<string, string> LocalPayment { get; set; }

        public IDictionary<string, string> Metadata { get; set; }
    }
}