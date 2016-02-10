using NUnit.Framework;
using System.Linq;
using System;
using Checkout.ApiServices.SharedModels;
using System.Collections.Generic;
using Checkout.ApiServices.Reporting.ResponseModels;
using Checkout.ApiServices.Charges.RequestModels;
using Tests.Utils;
using FilterAction = Checkout.ApiServices.SharedModels.Action;
using Checkout.ApiServices.Charges.ResponseModels;

namespace Tests
{
    [TestFixture(Category = "ReportingApi")]
    public class ReportingServiceTests : BaseService
    {
        [Test]
        public void QueryTransactions_FromDateAfterTransactionCreated_NoTransactionsFound()
        {
            // create new charge
            var fromDate = DateTime.Now;
            var chargeResponse = CreateChargeWithNewTrackId();

            // query transactions starting from charge created date
            var chargeCreatedDate = DateTime.SpecifyKind(DateTime.Parse(chargeResponse.Model.Created), DateTimeKind.Utc);
            var request = TestHelper.GetQueryTransactionModel(chargeResponse.Model.Email, chargeCreatedDate.AddHours(1), null, null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(response.Model.TotalRecords, 0);
        }

        [Test]
        public void QueryTransactions_FromDateBeforeTransactionCreated_OneTransactionFound()
        {
            // create new charge
            var fromDate = DateTime.Now;
            var chargeResponse = CreateChargeWithNewTrackId();

            // query transactions starting from input date
            var request = TestHelper.GetQueryTransactionModel(chargeResponse.Model.Email, fromDate, null, null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Data.All(d => request.FromDate < d.Date));
            Assert.AreEqual(response.Model.TotalRecords, 1);
        }

        [Test]
        public void QueryTransactions_FromDateIsNull_OneTransactionFound()
        {
            // create new charge
            var chargeResponse = CreateChargeWithNewTrackId();

            // query transactions starting from input date
            var request = TestHelper.GetQueryTransactionModel(chargeResponse.Model.Email, null, null, null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(response.Model.TotalRecords, 1);
        }

        [Test]
        public void QueryTransactions_ToDateAfterTransactionCreated_NoTransactionsFound()
        {
            // create new charge
            var chargeResponse = CreateChargeWithNewTrackId();

            // query transactions starting from charge created date
            var chargeCreatedDate = DateTime.SpecifyKind(DateTime.Parse(chargeResponse.Model.Created), DateTimeKind.Utc);
            var request = TestHelper.GetQueryTransactionModel(chargeResponse.Model.Email, null, chargeCreatedDate.AddHours(1), null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Data.All(d => request.ToDate > d.Date));
            Assert.AreEqual(response.Model.TotalRecords, 1);
        }

        [Test]
        public void QueryTransactions_ToDateBeforeTransactionCreated_OneTransactionFound()
        {
            // create new charge
            var toDate = DateTime.Now;
            var chargeResponse = CreateChargeWithNewTrackId();

            // query transactions starting from input date
            var request = TestHelper.GetQueryTransactionModel(chargeResponse.Model.Email, null, toDate, null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.Data.All(d => request.ToDate > d.Date));
            Assert.AreEqual(response.Model.TotalRecords, 0);
        }

        [Test]
        public void QueryTransactions_ToDateIsNull_OneTransactionFound()
        {
            // create new charge
            var chargeResponse = CreateChargeWithNewTrackId();

            // query transactions starting from input date
            var request = TestHelper.GetQueryTransactionModel(chargeResponse.Model.Email, null, null, null, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(response.Model.TotalRecords, 1);
        }

        [TestCaseSource(typeof(TestScenarios), "QueryTransaction_PageSize")]
        public void QueryTransactions_PageSizeWithLimits(int? pageSize)
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
        public void QueryTransactions_ColumnSortingBy(SortColumn? sortColumn)
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
        public void QueryTransactions_SortingOrder(SortOrder? sortOrder)
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
        public void QueryTransactions_Pagination(string pageNumber)
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
        public void QueryTransactions_FilterBySearchString(string searchValue)
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
        public void QueryTransactions_FilterBySearchWithCardNumber()
        {
            string cardNumber;
            var chargeResponse = CreateChargeWithNewTrackId(out cardNumber);

            // query transactions containing the generated card number
            var request = TestHelper.GetQueryTransactionModel(TestHelper.MaskCardNumber(cardNumber), null, null, SortColumn.Date, null, null, null, null);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.TotalRecords > 0);
            Assert.IsTrue(response.Model.Data.Any(t => t.TrackId == chargeResponse.Model.TrackId));
        }

        [TestCaseSource(typeof(TestScenarios), "QueryTransaction_Filter_Action")]
        public void QueryTransactions_FilterWithAction(FilterAction? action)
        {
            var filters = new List<Filter> {new Filter() { Action = action, Value = "test", Field = Field.Email, Operator = Operator.Contains } };
            var request = TestHelper.GetQueryTransactionModel(filters);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.TotalRecords > 0);

            switch (request.Filters.First().Action)
            {
                case FilterAction.Exclude:
                    Assert.IsFalse(response.Model.Data.Any(t => t.Customer.Email.Contains(request.Filters.First().Value)));
                    break;
                case FilterAction.Include:
                default:
                    Assert.IsTrue(response.Model.Data.Any(t => t.Customer.Email.Contains(request.Filters.First().Value)));
                    break;
            }
        }

        [TestCaseSource(typeof(TestScenarios), "QueryTransaction_Filter_Field")]
        public void QueryTransactions_FilterByField(Field? field)
        {
            string cardNumber;
            var chargeResponse = CreateChargeWithNewTrackId(out cardNumber);

            var filter = new Filter { Field = field };
            switch (field)
            {
                case Field.Status:
                    filter.Value = chargeResponse.Model.Status;
                    break;
                case Field.Email:
                    filter.Value = chargeResponse.Model.Email;
                    break;
                case Field.ChargeId:
                    filter.Value = chargeResponse.Model.Id;
                    break;
                case Field.TrackId:
                    filter.Value = chargeResponse.Model.TrackId;
                    break;
                case Field.CardNumber:
                    filter.Value = TestHelper.MaskCardNumber(cardNumber);
                    break;
                default:
                    break;
            }

            var request = TestHelper.GetQueryTransactionModel(new List<Filter>{filter});
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            if (field.HasValue)
            {
                Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
                Assert.IsTrue(response.Model.TotalRecords > 0);
            }
            else
            {
                Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.BadRequest);
                Assert.IsTrue(response.HasError);
            }

            switch (field)
            {
                case Field.Status:
                    Assert.IsTrue(response.Model.Data.All(t => 
                                    t.Status.Equals(filter.Value, StringComparison.OrdinalIgnoreCase)));
                    break;
                case Field.Email:
                    Assert.IsTrue(response.Model.Data.All(t => 
                                    t.Customer.Email.Equals(filter.Value, StringComparison.OrdinalIgnoreCase)));
                    break;
                case Field.ChargeId:
                case Field.TrackId:
                case Field.CardNumber:
                    Assert.IsTrue(response.Model.Data.Any(t => 
                                    t.TrackId.Equals(chargeResponse.Model.TrackId, StringComparison.OrdinalIgnoreCase)));
                    break;
                default:
                    // do nothing
                    break;
            }
        }

        [TestCaseSource(typeof(TestScenarios), "QueryTransaction_Filter_Operator")]
        public void QueryTransactions_FilterWithOperator(string value, Operator? op)
        {
            var filters = new List<Filter> { new Filter() { Value = value, Field = Field.Email, Operator = op } };
            var request = TestHelper.GetQueryTransactionModel(filters);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);

            switch (request.Filters.First().Operator)
            {
                case Operator.Begins:
                    Assert.IsTrue(response.Model.Data.Any(t => 
                                    t.Customer.Email.StartsWith(value, StringComparison.OrdinalIgnoreCase)));
                    break;
                case Operator.Contains:
                    Assert.IsTrue(response.Model.Data.Any(t => t.Customer.Email.ContainsIgnoreCase(value)));
                    break;
                case Operator.Ends:
                    Assert.IsTrue(response.Model.Data.Any(t => 
                                    t.Customer.Email.EndsWith(value, StringComparison.OrdinalIgnoreCase)));
                    break;
                case Operator.Equals:
                default:
                    Assert.IsTrue(response.Model.Data.Any(t => 
                                    t.Customer.Email.Equals(value, StringComparison.OrdinalIgnoreCase)));
                    break;
            }
        }

        [TestCaseSource(typeof(TestScenarios), "QueryTransaction_Filter_Value")]
        public void QueryTransactions_FilterWithValue(string value)
        {
            var filters = new List<Filter> { new Filter() { Value = value, Field = Field.Email, Operator = Operator.Contains } };
            var request = TestHelper.GetQueryTransactionModel(filters);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            if (string.IsNullOrEmpty(value))
            {
                Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.BadRequest);
                Assert.IsTrue(response.HasError);
            }
            else
            {
                Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
                Assert.IsTrue(response.Model.Data.Any(t => t.Customer.Email.ContainsIgnoreCase(value)));
            }
        }

        [Test]
        public void QueryTransactions_OppositeFilters_NoResults()
        {
            var filters = new List<Filter>
            {
                new Filter() { Value = "test", Field = Field.Email, Operator = Operator.Contains },
                new Filter() { Action = FilterAction.Exclude, Value = "test", Field = Field.Email, Operator = Operator.Contains }
            };
            var request = TestHelper.GetQueryTransactionModel(filters);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.TotalRecords == 0);
        }

        [Test]
        public void QueryTransactions_MultipleFilters()
        {
            var filters = new List<Filter>
            {
                new Filter() { Value = "test", Field = Field.Email, Operator = Operator.Contains },
                new Filter() { Value = "captured", Field = Field.Status, Operator = Operator.Equals }
            };
            var request = TestHelper.GetQueryTransactionModel(filters);
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.TotalRecords > 0);
            Assert.IsTrue(response.Model.Data.All(t => t.Customer.Email.ContainsIgnoreCase(request.Filters.First().Value)));
            Assert.IsTrue(response.Model.Data.All(t => t.Status.Equals(request.Filters.Last().Value, StringComparison.OrdinalIgnoreCase)));
        }

        [TestCaseSource(typeof(TestScenarios), "QueryTransaction_Filter_Field")]
        public void QueryTransactions_CreateChargeAndCapture_BothTransactionsFoundBy(Field? field)
        {
            string cardNumber;
            var chargeResponse = CreateChargeWithNewTrackId(out cardNumber);

            var filter = new Filter { Field = field };
            switch (field)
            {
                case Field.Status:
                    filter.Value = chargeResponse.Model.Status;
                    break;
                case Field.Email:
                    filter.Value = chargeResponse.Model.Email;
                    break;
                case Field.ChargeId:
                    filter.Value = chargeResponse.Model.Id;
                    break;
                case Field.TrackId:
                    filter.Value = chargeResponse.Model.TrackId;
                    break;
                case Field.CardNumber:
                    filter.Value = TestHelper.MaskCardNumber(cardNumber);
                    break;
                default:
                    break;
            }

            var request = TestHelper.GetQueryTransactionModel(new List<Filter> { filter });
            var firstQueryResponse = CheckoutClient.ReportingService.QueryTransaction(request);

            #region Assert First Query Response

            Assert.NotNull(firstQueryResponse);
            if (field.HasValue)
            {
                Assert.IsTrue(firstQueryResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
                Assert.IsTrue(firstQueryResponse.Model.TotalRecords > 0);
            }
            else
            {
                Assert.IsTrue(firstQueryResponse.HttpStatusCode == System.Net.HttpStatusCode.BadRequest);
                Assert.IsTrue(firstQueryResponse.HasError);
            }

            switch (field)
            {
                case Field.Status:
                    Assert.IsTrue(firstQueryResponse.Model.Data.All(t =>
                                    t.Status.Equals(filter.Value, StringComparison.OrdinalIgnoreCase)));
                    break;
                case Field.Email:
                    Assert.IsTrue(firstQueryResponse.Model.Data.All(t =>
                                    t.Customer.Email.Equals(filter.Value, StringComparison.OrdinalIgnoreCase)));
                    break;
                case Field.ChargeId:
                case Field.TrackId:
                case Field.CardNumber:
                    Assert.IsTrue(firstQueryResponse.Model.Data.Any(t =>
                                    t.TrackId.Equals(chargeResponse.Model.TrackId, StringComparison.OrdinalIgnoreCase)));
                    break;
                default:
                    // do nothing
                    break;
            }

            #endregion Assert First Query Response

            // capture charge and query 2nd time
            var chargeCapture = TestHelper.GetChargeCaptureModel(chargeResponse.Model.Value);
            chargeCapture.TrackId = chargeResponse.Model.TrackId;
            var captureChargeResponse = CheckoutClient.ChargeService.CaptureCharge(chargeResponse.Model.Id, chargeCapture);
            var secondQueryResponse = CheckoutClient.ReportingService.QueryTransaction(request);

            #region Assert Second Query Response

            Assert.NotNull(secondQueryResponse);
            if (field.HasValue)
            {
                Assert.IsTrue(secondQueryResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
                Assert.IsTrue(secondQueryResponse.Model.TotalRecords > 0);
            }
            else
            {
                Assert.IsTrue(secondQueryResponse.HttpStatusCode == System.Net.HttpStatusCode.BadRequest);
                Assert.IsTrue(secondQueryResponse.HasError);
            }

            switch (field)
            {
                case Field.Status:
                    Assert.IsTrue(secondQueryResponse.Model.Data.All(t =>
                                    t.Status.Equals(filter.Value, StringComparison.OrdinalIgnoreCase)));
                    break;
                case Field.Email:
                    Assert.IsTrue(secondQueryResponse.Model.Data.All(t =>
                                    t.Customer.Email.Equals(filter.Value, StringComparison.OrdinalIgnoreCase)));
                    break;
                case Field.CardNumber:
                case Field.ChargeId:
                    Assert.IsTrue(secondQueryResponse.Model.Data.Count(t =>
                                    t.TrackId.Equals(chargeResponse.Model.TrackId, StringComparison.OrdinalIgnoreCase)) == 1);
                    break;
                case Field.TrackId:
                    Assert.IsTrue(secondQueryResponse.Model.Data.Count(t =>
                                    t.TrackId.Equals(chargeResponse.Model.TrackId, StringComparison.OrdinalIgnoreCase)) == 2);
                    break;
                default:
                    // do nothing
                    break;
            }

            #endregion Assert Second Query Response
        }

        [TestCaseSource(typeof(TestScenarios), "QueryTransaction_Charge_FilterByCardNumber")]
        public void QueryTransactions_CreateChargeFilterByCardNumber_MatchTrackId(string cardNumber, string cvv, string expirityMonth, string expirityYear)
        {
            var chargeResponse = CreateChargeWithNewTrackId(cardNumber, cvv, expirityMonth, expirityYear);

            var filter = new Filter { Field = Field.CardNumber, Value = TestHelper.MaskCardNumber(cardNumber) };
            //var request = TestHelper.GetQueryTransactionModel(null, null, null, SortColumn.Date, SortOrder.Desc, null, null, new List<Filter> { filter });
            var request = TestHelper.GetQueryTransactionModel(new List<Filter> { filter });
            var response = CheckoutClient.ReportingService.QueryTransaction(request);

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.Model.TotalRecords > 0);
            Assert.IsTrue(response.Model.Data.Any(t => t.TrackId.Equals(chargeResponse.Model.TrackId, StringComparison.OrdinalIgnoreCase)));
        }

        #region Helpers

        /// <summary>
        /// Creates a new charge with default card and new track id and asserts that is not declined
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public HttpResponse<Charge> CreateChargeWithNewTrackId()
        {
            string cardNumber;
            return CreateChargeWithNewTrackId(out cardNumber);
        }

        /// <summary>
        /// Creates a new charge with default card and new track id and asserts that is not declined
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public HttpResponse<Charge> CreateChargeWithNewTrackId(out string cardNumber)
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            cardCreateModel.TrackId = "TRF" + Guid.NewGuid();
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);

            Assert.NotNull(chargeResponse);
            Assert.IsTrue(chargeResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsFalse(chargeResponse.Model.Status.Equals("Declined", StringComparison.OrdinalIgnoreCase));

            cardNumber = cardCreateModel.Card.Number;
            return chargeResponse;
        }

        /// <summary>
        /// Creates a new charge with provided card and new track id and asserts that is not declined
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="cvv"></param>
        /// <param name="expirityMonth"></param>
        /// <param name="expirityYear"></param>
        /// <returns></returns>
        public HttpResponse<Charge> CreateChargeWithNewTrackId(string cardNumber, string cvv, string expirityMonth, string expirityYear)
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModel(TestHelper.RandomData.Email);
            cardCreateModel.TrackId = "TRF" + Guid.NewGuid();
            cardCreateModel.Card.Number = cardNumber;
            cardCreateModel.Card.Cvv = cvv;
            cardCreateModel.Card.ExpiryMonth = expirityMonth;
            cardCreateModel.Card.ExpiryYear = expirityYear;
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);

            Assert.NotNull(chargeResponse);
            Assert.IsTrue(chargeResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsFalse(chargeResponse.Model.Status.Equals("Declined", StringComparison.OrdinalIgnoreCase));

            cardNumber = cardCreateModel.Card.Number;
            return chargeResponse;
        }

        #endregion Helpers
    }
}