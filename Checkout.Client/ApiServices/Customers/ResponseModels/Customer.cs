using Checkout.ApiServices.Cards.ResponseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Checkout.ApiServices.Customers.ResponseModels
{
    public class Customer
    {
        public string Object;
        public string Id;
        public string Name;
        public DateTime Created;
        public string Email;
        public string PhoneNumber;
        public string Description;
        public string Ltv;
        public string DefaultCard;
        public string ResponseCode;
        public bool LiveMode;
        public CardList Cards;
        public Dictionary<String, String> Metadata { get; set; }
        
       
    }
}
