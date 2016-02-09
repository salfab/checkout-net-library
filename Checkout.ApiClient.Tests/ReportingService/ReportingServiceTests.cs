using NUnit.Framework;
using System.Linq;
using System;
using Checkout.ApiServices.SharedModels;
using System.Collections.Generic;
using Checkout.ApiServices.Reporting.ResponseModels;
using Checkout.ApiServices.Charges.RequestModels;
using Tests.Utils;

namespace Tests
{
    [TestFixture(Category = "ReportingApi")]
    public class ReportingServiceTests : BaseService
    {
        [Test]
        public void QueryTransactions_FromDateAfterTransactionCreated_NoTransactionsFound()
        {
            var customerEmail = TestHelper.RandomData.Email;

            // create new charge
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(customerEmail);
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);
            Assert.NotNull(chargeResponse);
            Assert.IsTrue(chargeResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(!chargeResponse.HasError);

            // query transactions starting from charge created date
            var chargeCreatedDate = DateTime.SpecifyKind(DateTime.Parse(chargeResponse.Model.Created), DateTimeKind.Utc);
            var request = TestHelper.GetQueryTransactionModel(customerEmail, chargeCreatedDate.AddHours(1), null, null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(response.Model.TotalRecords, 0);
        }

        [Test]
        public void QueryTransactions_FromDateBeforeTransactionCreated_OneTransactionFound()
        {
            var customerEmail = TestHelper.RandomData.Email;
            var fromDate = DateTime.Now;

            // create new charge
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(customerEmail);
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);
            Assert.NotNull(chargeResponse);
            Assert.IsTrue(chargeResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(!chargeResponse.HasError);

            // query transactions starting from input date
            var request = TestHelper.GetQueryTransactionModel(customerEmail, fromDate, null, null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Data.All(d => request.FromDate < d.Date));
            Assert.AreEqual(response.Model.TotalRecords, 1);
        }

        [Test]
        public void QueryTransactions_FromDateIsNull_OneTransactionFound()
        {
            var customerEmail = TestHelper.RandomData.Email;

            // create new charge
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(customerEmail);
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);
            Assert.NotNull(chargeResponse);
            Assert.IsTrue(chargeResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(!chargeResponse.HasError);

            // query transactions starting from input date
            var request = TestHelper.GetQueryTransactionModel(customerEmail, null, null, null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(response.Model.TotalRecords, 1);
        }

        [Test]
        public void QueryTransactions_ToDateAfterTransactionCreated_NoTransactionsFound()
        {
            var customerEmail = TestHelper.RandomData.Email;

            // create new charge
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(customerEmail);
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);
            Assert.NotNull(chargeResponse);
            Assert.IsTrue(chargeResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(!chargeResponse.HasError);

            // query transactions starting from charge created date
            var chargeCreatedDate = DateTime.SpecifyKind(DateTime.Parse(chargeResponse.Model.Created), DateTimeKind.Utc);
            var request = TestHelper.GetQueryTransactionModel(customerEmail, null, chargeCreatedDate.AddHours(1), null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Data.All(d => request.ToDate > d.Date));
            Assert.AreEqual(response.Model.TotalRecords, 1);
        }

        [Test]
        public void QueryTransactions_ToDateBeforeTransactionCreated_OneTransactionFound()
        {
            var customerEmail = TestHelper.RandomData.Email;
            var toDate = DateTime.Now;

            // create new charge
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(customerEmail);
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);
            Assert.NotNull(chargeResponse);
            Assert.IsTrue(chargeResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(!chargeResponse.HasError);

            // query transactions starting from input date
            var request = TestHelper.GetQueryTransactionModel(customerEmail, null, toDate, null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Data.All(d => request.ToDate > d.Date));
            Assert.AreEqual(response.Model.TotalRecords, 0);
        }

        [Test]
        public void QueryTransactions_ToDateIsNull_OneTransactionFound()
        {
            var customerEmail = TestHelper.RandomData.Email;

            // create new charge
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(customerEmail);
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);
            Assert.NotNull(chargeResponse);
            Assert.IsTrue(chargeResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(!chargeResponse.HasError);

            // query transactions starting from input date
            var request = TestHelper.GetQueryTransactionModel(customerEmail, null, null, null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(response.Model.TotalRecords, 1);
        }

        [TestCaseSource(typeof(TestScenarios), "QueryTransaction_PageSize")]
        public void QueryTransactions_ShouldAllowPageSizeWithLimits(int? pageSize)
        {
            var request = TestHelper.GetQueryTransactionModel(string.Empty, null, null, null, null, pageSize, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            if (pageSize.HasValue && (pageSize.Value < 10 || pageSize.Value > 250))
            {
                Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.BadRequest);
                Assert.IsTrue(response.HasError);
            }
            else
            {
                Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
                Assert.AreEqual(response.Model.PageSize, pageSize ?? 10);
            }
        }

        [TestCaseSource(typeof(TestScenarios), "QueryTransaction_ColumnSorting")]
        public void QueryTransactions_ShouldAllowColumnSorting(SortColumn? sortColumn)
        {
            var request = TestHelper.GetQueryTransactionModel(string.Empty, null, null, sortColumn, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);

            IOrderedQueryable<Transaction> orderedData = null;
            switch (request.SortColumn)
            {
                case SortColumn.Amount:
                case SortColumn.BusinessName:
                case SortColumn.ChannelName:
                case SortColumn.Currency:
                case SortColumn.Id:
                case SortColumn.LiveMode:
                case SortColumn.OriginId:
                case SortColumn.ResponseCode:
                case SortColumn.Status:
                case SortColumn.TrackId:
                case SortColumn.Scheme:
                case SortColumn.Type:
                    orderedData = response.Model.Data
                                                .AsQueryable()
                                                .OrderBy(t => ReflectionHelper.GetPropValue(t, request.SortColumn.ToString()) as string, 
                                                            StringComparer.InvariantCultureIgnoreCase);
                    break;
                case SortColumn.Name:
                case SortColumn.Email:
                    orderedData = response.Model.Data
                                                .AsQueryable()
                                                .OrderBy(t => ReflectionHelper.GetPropValue(t, "Customer." + request.SortColumn) as string,
                                                            StringComparer.InvariantCultureIgnoreCase);
                    break;
                case SortColumn.Date:
                default:
                    orderedData = response.Model.Data.AsQueryable().OrderByDescending(t => t.Date); // when ordering by date ASC sorting is default
                    break;
            }

            CollectionAssert.AreEqual(orderedData, response.Model.Data);
        }

        [TestCaseSource(typeof(TestScenarios), "QueryTransaction_Sorting")]
        public void QueryTransactions_ShouldAllowSorting(SortOrder? sortOrder)
        {
            var request = TestHelper.GetQueryTransactionModel(string.Empty, null, null, SortColumn.Amount, sortOrder, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);

            IOrderedQueryable<Transaction> orderedData = null;
            switch (request.SortOrder)
            {
                case SortOrder.Asc:
                    orderedData = response.Model.Data.AsQueryable().OrderBy(t => ReflectionHelper.GetPropValue(t, "Amount"));
                    break;
                case SortOrder.Desc:
                default:
                    orderedData = response.Model.Data.AsQueryable().OrderByDescending(t => ReflectionHelper.GetPropValue(t, "Amount"));
                    break;
            }

            CollectionAssert.AreEqual(orderedData, response.Model.Data);
        }

        [TestCaseSource(typeof(TestScenarios), "QueryTransaction_Pagination")]
        public void QueryTransactions_ShouldAllowPagination(string pageNumber)
        {
            var request = TestHelper.GetQueryTransactionModel(string.Empty, null, null, null, null, null, pageNumber, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);

            if (string.IsNullOrEmpty(request.PageNumber))
            {
                Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
                Assert.IsTrue(response.Model.PageNumber == 1); // default page number
            }
            else
            {
                int value;
                if (int.TryParse(request.PageNumber, out value) && value > 0)
                {
                    if (response.Model.PageNumber == 9999)
                    {
                        // Result set empty if greater than the number of pages. 
                        Assert.IsTrue(response.Model.TotalRecords == 0);
                    }

                    Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
                    Assert.IsTrue(response.Model.PageNumber.ToString() == request.PageNumber);
                }
                else
                {
                    // invalid number
                    Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.BadRequest);
                    Assert.IsTrue(response.HasError);
                }
            }
        }

        [TestCaseSource(typeof(TestScenarios), "QueryTransaction_SearchString")]
        public void QueryTransactions_ShouldFilterBySearchString(string searchValue)
        {
            var request = TestHelper.GetQueryTransactionModel(searchValue, null, null, null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.TotalRecords > 0);

            // the following fields will be checked if they contain the input search value:
            var assertions = new List<bool>()
            {
                response.Model.Data.Any(d => d.Id.ContainsIgnoreCase(request.Search)),
                response.Model.Data.Any(d => d.OriginId.ContainsIgnoreCase(request.Search)),
                response.Model.Data.Any(d => d.TrackId.ContainsIgnoreCase(request.Search)),
                response.Model.Data.Any(d => d.Status.ContainsIgnoreCase(request.Search)),
                response.Model.Data.Any(d => d.Customer.Email.ContainsIgnoreCase(request.Search))
            };

            // at least one of the fields contains the search value
            Assert.IsTrue(assertions.Any(assert => assert == true));
        }

        [Test]
        public void QueryTransactions_ShouldSearchByCardNumber()
        {
            // create new charge
            var cardCreateModel = new CardCharge()
            {
                Email = TestHelper.RandomData.Email,
                Currency = "USD",
                Value = "247",
                TrackId = "TRF" + Guid.NewGuid(),
                Card = new Checkout.ApiServices.Cards.RequestModels.CardCreate()
                {
                    Number = "4242424242424242",
                    Cvv = "100",
                    ExpiryMonth = "6",
                    ExpiryYear = "2018"
                }
            };
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);
            Assert.NotNull(chargeResponse);
            Assert.IsTrue(chargeResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(chargeResponse.Model.Status != "Declined");

            // query transactions containing the generated card number
            var request = TestHelper.GetQueryTransactionModel(cardCreateModel.Card.Number, null, null, SortColumn.Date, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.TotalRecords > 0);
            Assert.IsTrue(response.Model.Data.Any(t => t.TrackId == cardCreateModel.TrackId));
        }
    }
}