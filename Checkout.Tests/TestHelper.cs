using Checkout.ApiServices.Cards.RequestModels;
using Checkout.ApiServices.Charges.RequestModels;
using Checkout.ApiServices.Customers.RequestModels;
using Checkout.ApiServices.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests.Utils;

namespace Tests
{
    public class TestHelper
    {
        private static RandomData _randomData;

        public static RandomData RandomData { get { return _randomData ?? (_randomData = new RandomData()); } }

        #region Token Helpers
        //public static CardTokenCreate GetCardTokenCreateModel()
        //{
        //    return new CardTokenCreate()
        //       {
        //           Card = GetBaseCreateCardModel()
        //       };
        //}

        //public static PaymentTokenCreate GetPaymentTokenCreateModel()
        //{
        //    return new PaymentTokenCreate()
        //      {
        //          Currency = "usd",
        //          Value = RandomData.GetNumber(50, 500)
        //      };
        //}

        #endregion

        #region Card Helpers
        public static Address GetAddressModel()
        {
            return new Address()
            {
                AddressLine1 = RandomData.StreetAddress,
                AddressLine2 = RandomData.String,
                City = RandomData.City,
                Country = RandomData.Country,
                Phone = RandomData.PhoneNumber,
                Postcode = RandomData.PostCode,
                State = RandomData.City
            };
        }

        public static BaseCardCreate GetBaseCreateCardModel(CardProvider cardProvider=CardProvider.Visa)
        {
            if( cardProvider == CardProvider.Visa )
            {
                return new BaseCardCreate()
                  {
                      Cvv = "100",
                      ExpiryMonth = "06",
                      ExpiryYear = "2018",
                      Name = RandomData.FullName,
                      Number = "4242424242424242",
                      BillingDetails = GetAddressModel()
                  };
            }else
            {
                return new BaseCardCreate()
                  {
                      Cvv = "257",
                      ExpiryYear = "2017",
                      ExpiryMonth = "06",
                      Name = RandomData.FullName,
                      Number = "5313581000123430",
                      BillingDetails = GetAddressModel()
                  };
            }
        }

        public static CardCreate GetCardCreateModel(string customerId,CardProvider cardProvider=CardProvider.Visa)
        {
            return new CardCreate()
            {
                CustomerId = customerId,
                Card = GetBaseCreateCardModel(cardProvider)
            };
        }

        public static CardUpdate GetCardUpdateModel(string customerId, string cardId)
        {
            return new CardUpdate()
            {
                CardId = cardId,
                CustomerId = customerId,
                Card = new BaseCard()
                {
                    ExpiryMonth = "06",
                    ExpiryYear = "2018",
                    Name = RandomData.FullName,
                    BillingDetails = GetAddressModel()
                }
            };
        }

        #endregion

        #region Customer Helpers
        public static CustomerCreate GetCustomerCreateModelWithCardToken(string cardToken)
        {
            return new CustomerCreate()
            {
                Token = cardToken,
                Name = RandomData.FullName,
                Description = RandomData.String,
                Email = RandomData.Email,
                PhoneNumber = RandomData.PhoneNumber,
                Metadata = new Dictionary<string, string>() { { "channelInfo", RandomData.CompanyName } },
            };
        }
        public static CustomerCreate GetCustomerCreateModelWithCard()
        {
            return new CustomerCreate()
            {
                Name = RandomData.FullName,
                Description = RandomData.String,
                Email = RandomData.Email,
                PhoneNumber = RandomData.PhoneNumber,
                Metadata = new Dictionary<string, string>() { { "channelInfo", RandomData.CompanyName } },
                Card = GetBaseCreateCardModel()
            };
        }
        public static CustomerCreate GetCustomerCreateModelWithNoCard()
        {
            var customerCreateModel = GetCustomerCreateModelWithCard();
            customerCreateModel.Card = null;
            return customerCreateModel;
        }
        public static CustomerUpdate GetCustomerUpdateModel(string customerId)
        {
            return new CustomerUpdate()
            {
                CustomerId = customerId,
                Name = RandomData.FullName,
                Description = RandomData.String,
                Email = RandomData.Email,
                PhoneNumber = RandomData.PhoneNumber,
                Metadata = new Dictionary<string, string>() { { "channelInfo", RandomData.CompanyName } }
            };
        }
        #endregion

        #region Charge Helpers
        public static CardChargeCreate GetCardChargeCreateModel(string customerEmail = null,string customerId = null)
        {
            return new CardChargeCreate()
                {
                    CustomerId = customerId,
                    Email = customerEmail,
                    AutoCapture = "Y",
                    AutoCapTime = 10,
                    Currency = "Usd",
                    Description = RandomData.String,
                    Value = RandomData.GetNumber(50, 500),
                    Card = GetBaseCreateCardModel(),
                    Products = new List<Product>(){ new Product{
                        Description = RandomData.String,
                        Image = "http://www.imageurl.com/me.png",
                        Name = "test product",
                        Price = RandomData.GetNumber(50, 500),
                        Quantity = RandomData.GetNumber(1, 100),
                        ShippingCost = RandomData.GetDecimalNumber(),
                        Sku = RandomData.UniqueString,
                        TrackingUrl = "http://www.track.com?ref=12345"
                    }
                    },
                    ShippingDetails = new ShippingAddress
                    {
                        RecipientName = RandomData.FullName,
                        AddressLine1 = RandomData.StreetAddress,
                        AddressLine2 = RandomData.String,
                        City = RandomData.City,
                        Country = RandomData.Country,
                        Phone = RandomData.PhoneNumber,
                        Postcode = RandomData.PostCode,
                        State = RandomData.City
                    },
                    Metadata = new Dictionary<string, string>() { { "extraInformation", RandomData.CompanyName } }
                };
        }

        public static CardIdChargeCreate GetCardIdChargeCreateModel(string cardId, string customerEmail = null, string customerId = null)
        {
            return new CardIdChargeCreate()
                {
                    CardId=cardId,
                    CustomerId = customerId,
                    Email = customerEmail,
                    AutoCapture = "Y",
                    AutoCapTime = 10,
                    Currency = "Usd",
                    Description = RandomData.String,
                    Value = RandomData.GetNumber(50, 500)
                };
        }

        public static CardTokenChargeCreate GetCardTokenChargeCreateModel(string cardToken, string customerEmail = null, string customerId = null)
        {
            return new CardTokenChargeCreate()
            {
                CardToken = cardToken,
                CustomerId = customerId,
                Email = customerEmail,
                AutoCapture = "Y",
                AutoCapTime = 10,
                Currency = "Usd",
                Description = RandomData.String,
                Value = RandomData.GetNumber(50, 500)
            };
        }

        public static BaseCharge GetBaseChargeModel(string customerEmail = null, string customerId = null)
        {
            return new BaseCharge()
            {
                CustomerId = customerId,
                Email = customerEmail,
                AutoCapture = "Y",
                AutoCapTime = 10,
                Currency = "Usd",
                Description = RandomData.String,
                Value = RandomData.GetNumber(50, 500)
            };
        }

        public static ChargeRefund GetChargeRefundModel(string chargeId, int amount)
        {
            return new ChargeRefund()
            {
                 ChargeId = chargeId,
                 Value = amount
            };
        }

         public static ChargeUpdate GetChargeUpdateModel(string chargeId)
        {
            return new ChargeUpdate()
            {
                 ChargeId = chargeId,
                  Description= RandomData.String,
                 Metadata = new Dictionary<string, string>() { { "extraInformation", RandomData.CompanyName }, { "extraInformation2", RandomData.String } }
            };
        }

         public static ChargeCapture GetChargeCaptureModel(string chargeId, int amount)
         {
             return new ChargeCapture()
             {
                 ChargeId = chargeId,
                 Value = amount
             };
         }
        
        #endregion
     
    }
}
