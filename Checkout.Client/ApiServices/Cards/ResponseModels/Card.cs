using Checkout.ApiServices.SharedModels;
using System;
namespace Checkout.ApiServices.Cards.ResponseModels
{
    public class Card
    {
        public string Object { get; set; }
        public string Id { get; set; }
        public string Last4 { get; set; }
        public string PaymentMethod { get; set; }
        public string FingerPrint { get; set; }
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public Address BillingDetails { get; set; }
        public string CvcCheck { get; set; }
        public string AvsCheck { get; set; }
        public string ResponseCode { get; set; }
        public string AuthCode { get; set; }
        public bool DefaultCard { get; set; }
        public bool LiveMode { get; set; }
    }
}