using Checkout.ApiServices.Cards.RequestModels;
using Checkout.ApiServices.Charges.RequestModels;
using Checkout.ApiServices.Customers.RequestModels;
using Checkout.ApiServices.SharedModels;
using Checkout.ApiServices.Tokens.RequestModels;
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

        public static PaymentTokenCreate GetPaymentTokenCreateModel(string email)
        {
            return new PaymentTokenCreate()
              {
                  Currency = "usd",
                  Value = RandomData.GetNumber(50, 500).ToString(),
                  AutoCapTime = 1,
                  AutoCapture = "N",
                  ChargeMode = 1,
                  Email = email,
                  CustomerIp = "82.23.168.254",
                  TrackId = "TRK12345", 
                  Description = RandomData.String,
                  Products = GetProducts(),
                  ShippingDetails = GetAddress(),
                  Metadata = new Dictionary<string, string>() { { "extraInformation", RandomData.CompanyName } },
                  Udf1 = RandomData.String,
                  Udf2 = RandomData.String,
                  Udf3 = RandomData.String,
                  Udf4 = RandomData.String,
                  Udf5 = RandomData.String, 
              };
        }

        #endregion

        #region Card Helpers
        public static Address GetAddressModel()
        {
            return new Address()
            {
                AddressLine1 = RandomData.StreetAddress,
                AddressLine2 = RandomData.String,
                City = RandomData.City,
                Country = RandomData.CountryISO2,
                Phone = GetPhone(),
                Postcode = RandomData.PostCode,
                State = RandomData.City
            };
        }

        public static BaseCard GetBaseCardModel(CardProvider cardProvider=CardProvider.Visa)
        {
            if( cardProvider == CardProvider.Visa )
            {
                return new BaseCard()
                  {
                      ExpiryMonth = "06",
                      ExpiryYear = "2018",
                      Name = RandomData.FullName,
                      BillingDetails = GetAddressModel()
                  };
            }else
            {
                return new BaseCard()
                  {
                      ExpiryYear = "2017",
                      ExpiryMonth = "06",
                      Name = RandomData.FullName,
                      BillingDetails = GetAddressModel()
                  };
            }
        }

        public static CardCreate GetCardCreateModel(CardProvider cardProvider=CardProvider.Visa)
        {
            CardCreate card=new CardCreate();
            var baseCard = GetBaseCardModel(cardProvider);

            card.Name = baseCard.Name;
            card.ExpiryMonth = baseCard.ExpiryMonth;
            card.ExpiryYear = baseCard.ExpiryYear;
            card.BillingDetails = baseCard.BillingDetails;

            if (cardProvider == CardProvider.Visa)
            {
                card.Cvv = "100";
                card.Number = "4242424242424242";
            }
            else
            {
                card.Cvv = "257";
                card.Number = "5313581000123430";
            }

            return card;
        }

        public static CardUpdate GetCardUpdateModel(bool setDefaultCard=false)
        {
            CardUpdate card = new CardUpdate();
            var baseCard = GetBaseCardModel();

            card.Name = baseCard.Name;
            card.ExpiryMonth = baseCard.ExpiryMonth;
            card.ExpiryYear = baseCard.ExpiryYear;
            card.BillingDetails = baseCard.BillingDetails;
            card.DefaultCard = setDefaultCard;

            return card;
        }

        #endregion

        #region Customer Helpers

        public static CustomerCreate GetCustomerCreateModelWithCard(CardProvider cardProvider = CardProvider.Visa)
        {
            return new CustomerCreate()
            {
                Name = RandomData.FullName,
                Description = RandomData.String,
                Email = RandomData.Email,
                Phone = GetPhone(),
                Metadata = new Dictionary<string, string>() { { "channelInfo", RandomData.CompanyName } },
                Card = GetCardCreateModel(cardProvider)
            };
        }
        public static CustomerCreate GetCustomerCreateModelWithNoCard()
        {
            var customerCreateModel = GetCustomerCreateModelWithCard();
            customerCreateModel.Card = null;
            return customerCreateModel;
        }
        public static CustomerUpdate GetCustomerUpdateModel()
        {
            return new CustomerUpdate()
            {
                Name = RandomData.FullName,
                Description = RandomData.String,
                Email = RandomData.Email,
                Phone = GetPhone(),
                Metadata = new Dictionary<string, string>() { { "channelInfo", RandomData.CompanyName } }
            };
        }

        private static Phone GetPhone()
        {
            return new Phone()
            {
                CountryCode = "1",
                Number = "999 999 9999"
            };
        }
        
        public static DefaultCardCharge GetCustomerDefaultCardChargeCreateModel(string customerId)
         {
             DefaultCardCharge defaultCardCharge = new DefaultCardCharge
             {
                 CustomerId = customerId,
                 AutoCapture = "Y",
                 AutoCapTime = 10,
                 Currency = "Usd",
                 TrackId = "TRK12345",
                 TransactionIndicator = "1",
                 CustomerIp = "82.23.168.254",
                 Description = RandomData.String,
                 Value = RandomData.GetNumber(50, 500).ToString(),
                 Descriptor = new BillingDescriptor { Name = "Amigo ltd.", City = "London" },
                 Products = GetProducts(),
                 ShippingDetails = GetAddress(),
                 Metadata = new Dictionary<string, string>() { { "extraInformation", RandomData.CompanyName } },
                 Udf1 = RandomData.String,
                 Udf2 = RandomData.String,
                 Udf3 = RandomData.String,
                 Udf4 = RandomData.String,
                 Udf5 = RandomData.String
             };

             return defaultCardCharge;
         }
        #endregion

        #region Charge Helpers
        public static CardCharge GetCardChargeCreateModel(string customerEmail = null,string customerId = null)
        {
            return new CardCharge()
                {
                    CustomerId = customerId,
                    Email = customerEmail,
                    AutoCapture = "N",
                    AutoCapTime = 0,
                    Currency = "Usd",
                    TrackId = "TRK12345",
                    TransactionIndicator="1",
                    CustomerIp="82.23.168.254",
                    Description = RandomData.String,
                    Value = RandomData.GetNumber(50, 500).ToString(),
                    Card = GetCardCreateModel(),
                    Descriptor =new BillingDescriptor { Name = "Amigo ltd.", City = "London" },
                    Products = GetProducts(),
                    ShippingDetails = GetAddress(),
                    Metadata = new Dictionary<string, string>() { { "extraInformation", RandomData.CompanyName } },
                    Udf1 = RandomData.String,
                    Udf2 = RandomData.String,
                    Udf3 = RandomData.String,
                    Udf4 = RandomData.String,
                    Udf5 = RandomData.String
                };
        }

        public static Address GetAddress()
        {

            return new Address
                    {
                        AddressLine1 = RandomData.StreetAddress,
                        AddressLine2 = RandomData.String,
                        City = RandomData.City,
                        Country = RandomData.CountryISO2,
                        Phone = GetPhone(),
                        Postcode = RandomData.PostCode,
                        State = RandomData.City
                    };
        }

        public static List<Product> GetProducts(int numOfProducts = 1)
        {
            var productList = new List<Product>();
            Product product;

            for (int i = 0; i <= numOfProducts; i++)
            {
                product = new Product
                {
                    Description = RandomData.String,
                    Image = "http://www.imageurl.com/me.png",
                    Name = "test product" + i,
                    Price = RandomData.GetNumber(50, 500),
                    Quantity = RandomData.GetNumber(1, 100),
                    ShippingCost = RandomData.GetDecimalNumber(),
                    Sku = RandomData.UniqueString,
                    TrackingUrl = "http://www.track.com?ref=12345"
                };
                productList.Add(product);
            }

            return productList;
        }

        public static CardIdCharge GetCardIdChargeCreateModel(string cardId, string customerEmail = null, string customerId = null)
        {
            return new CardIdCharge()
                {
                    CardId=cardId,
                    CustomerId = customerId,
                    Email = customerEmail,
                    AutoCapture = "Y",
                    AutoCapTime = 10,
                    Currency = "Usd",
                    Description = RandomData.String,
                    Value = RandomData.GetNumber(50, 500).ToString(),
                    Products = GetProducts(),
                    ShippingDetails = GetAddress(),
                    Metadata = new Dictionary<string, string>() { { "extraInformation", RandomData.CompanyName } },
                    Descriptor = new BillingDescriptor { Name = "Amigo ltd.", City = "London" }
                };
        }

        public static CardTokenCharge GetCardTokenChargeCreateModel(string cardToken, string customerEmail = null, string customerId = null)
        {
            return new CardTokenCharge()
            {
                CardToken = cardToken,
                CustomerId = customerId,
                Email = customerEmail?? RandomData.Email,
                AutoCapture = "Y",
                AutoCapTime = 10,
                Currency = "Usd",
                Description = RandomData.String,
                Value = RandomData.GetNumber(50, 500).ToString(),
                Descriptor = new BillingDescriptor { Name = "Amigo ltd.", City = "London" }
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
                Value = RandomData.GetNumber(50, 500).ToString()
            };
        }

        public static ChargeRefund GetChargeRefundModel(string amount=null)
        {
            return new ChargeRefund()
            {
                Value = amount,
                TrackId = "TRK12345",
                Description = RandomData.String,
                Products = GetProducts(),
                Metadata = new Dictionary<string, string>() { { "extraInformation", RandomData.CompanyName } },
                Udf1 = RandomData.String,
                Udf2 = RandomData.String,
                Udf3 = RandomData.String,
                Udf4 = RandomData.String,
                Udf5 = RandomData.String
            };
        }

        public static ChargeUpdate GetChargeUpdateModel()
        {
            return new ChargeUpdate()
            {
                TrackId = "TRK12345",
                Description = RandomData.String,
                Metadata = new Dictionary<string, string>() { { "extraInformation", RandomData.CompanyName }, { "extraInformation2", RandomData.String } },
                Udf1 = RandomData.String,
                Udf2 = RandomData.String,
                Udf3 = RandomData.String,
                Udf4 = RandomData.String,
                Udf5 = RandomData.String
            };
        }

         public static ChargeCapture GetChargeCaptureModel(string amount=null)
         {
             return new ChargeCapture()
             {
                 Value = amount,
                 TrackId = "TRK12345",
                 Description = RandomData.String,
                 Products = GetProducts(),
                 Metadata = new Dictionary<string, string>() { { "extraInformation", RandomData.CompanyName } },
                 Udf1 = RandomData.String,
                 Udf2 = RandomData.String,
                 Udf3 = RandomData.String,
                 Udf4 = RandomData.String,
                 Udf5 = RandomData.String
             };
         }

         public static ChargeVoid GetChargeVoidModel()
         {
             return new ChargeVoid()
             {
                 TrackId = "TRK12345",
                 Description = RandomData.String,
                 Products = GetProducts(),
                 Metadata = new Dictionary<string, string>() { { "extraInformation", RandomData.CompanyName } },
                 Udf1 = RandomData.String,
                 Udf2 = RandomData.String,
                 Udf3 = RandomData.String,
                 Udf4 = RandomData.String,
                 Udf5 = RandomData.String
             };
         }

        #endregion

    }
}
