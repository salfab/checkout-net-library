using System.Linq;
using System.Net;
using Checkout.ApiServices.RecurringPayments.RequestModels;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.RecurringPaymentsService
{
    [TestFixture(Category = "RecurringPaymentsApi")]
    public class RecurringPaymentsServiceTests : BaseServiceTests
    {
        [Test]
        public void CancelCustomerPaymentPlan()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModelWithNewPaymentPlan(TestHelper.RandomData.Email);
            var createResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);
            var cancelResponse =
                CheckoutClient.RecurringPaymentsService.CancelCustomerPaymentPlan(
                    createResponse.Model.CustomerPaymentPlans.Single().CustomerPlanId);

            cancelResponse.Should().NotBeNull();
            cancelResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            cancelResponse.Model.Message.Should().BeEquivalentTo("OK");
        }

        [Test]
        public void CancelPaymentPlan()
        {
            var paymentPlanModel = TestHelper.GetSinglePaymentPlanCreateModel();
            var createResponseModel =
                CheckoutClient.RecurringPaymentsService.CreatePaymentPlan(paymentPlanModel).Model.PaymentPlans.Single();
            var cancelResponse = CheckoutClient.RecurringPaymentsService.CancelPaymentPlan(createResponseModel.PlanId);

            cancelResponse.Should().NotBeNull();
            cancelResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            cancelResponse.Model.Message.Should().BeEquivalentTo("OK");
        }

        [Test]
        public void CreateFromExistingCustomerPaymentPlanWithCharge()
        {
            var paymentPlanModel = TestHelper.GetSinglePaymentPlanCreateModel();
            var createResponseModel =
                CheckoutClient.RecurringPaymentsService.CreatePaymentPlan(paymentPlanModel).Model.PaymentPlans.Single();
            var cardCreateModel = TestHelper.GetCardChargeCreateModelWithExistingPaymentPlan(
                createResponseModel.PlanId, null, TestHelper.RandomData.Email);
            var chargeResponse = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);

            chargeResponse.Should().NotBeNull();
            chargeResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            var chargeResponseModel = chargeResponse.Model.CustomerPaymentPlans.Single();
            chargeResponseModel.ShouldBeEquivalentTo(createResponseModel,
                options =>
                    options.Excluding(o => o.CustomerPlanId)
                        .Excluding(o => o.CustomerId)
                        .Excluding(o => o.CardId)
                        .Excluding(o => o.RecurringCountLeft)
                        .Excluding(o => o.TotalCollectedCount)
                        .Excluding(o => o.TotalCollectedValue)
                        .Excluding(o => o.PreviousRecurringDate)
                        .Excluding(o => o.NextRecurringDate)
                        .Excluding(o => o.StartDate));
        }

        [Test]
        public void CreateNewCustomerPaymentPlanWithCharge()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModelWithNewPaymentPlan(TestHelper.RandomData.Email);
            var response = CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            var customerPaymentPlanModel = cardCreateModel.PaymentPlans.Single();
            var responseModel = response.Model.CustomerPaymentPlans.Single();

            responseModel.PlanId.Should().NotBeNullOrEmpty();
            responseModel.Status.Should().HaveValue();
            responseModel.Name.Should().Be(customerPaymentPlanModel.Name);
            responseModel.PlanTrackId.Should().Be(customerPaymentPlanModel.PlanTrackId);
            responseModel.RecurringCount.Should().Be(customerPaymentPlanModel.RecurringCount);
            responseModel.Value.Should().Be(customerPaymentPlanModel.Value);
            responseModel.AutoCapTime.Should().Be(customerPaymentPlanModel.AutoCapTime);
            responseModel.Cycle.Should().Be(customerPaymentPlanModel.Cycle);
        }

        [Test]
        public void CreatePaymentPlan()
        {
            var paymentPlanCreateModel = TestHelper.GetSinglePaymentPlanCreateModel();
            var response = CheckoutClient.RecurringPaymentsService.CreatePaymentPlan(paymentPlanCreateModel);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            var singlePlanModel = paymentPlanCreateModel.PaymentPlans.Single();
            var responseModel = response.Model.PaymentPlans.Single();

            responseModel.ShouldBeEquivalentTo(singlePlanModel,
                options => options.Excluding(o => o.PlanId).Excluding(o => o.Status));
            responseModel.PlanId.Should().NotBeNullOrEmpty();
            responseModel.Status.Should().HaveValue();
        }

        [Test]
        public void GetCustomerPaymentPlan()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModelWithNewPaymentPlan(TestHelper.RandomData.Email);
            var customerPlanModel =
                CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel).Model.CustomerPaymentPlans.Single();
            var getResponse =
                CheckoutClient.RecurringPaymentsService.GetCustomerPaymentPlan(customerPlanModel.CustomerPlanId);

            getResponse.Should().NotBeNull();
            getResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            customerPlanModel.ShouldBeEquivalentTo(customerPlanModel,
                options => options.Excluding(o => o.CustomerId).Excluding(o => o.CardId));
        }

        [Test]
        public void GetPaymentPlan()
        {
            var paymentPlanModel = TestHelper.GetSinglePaymentPlanCreateModel();
            var createResponseModel =
                CheckoutClient.RecurringPaymentsService.CreatePaymentPlan(paymentPlanModel).Model.PaymentPlans.Single();
            var getResponse = CheckoutClient.RecurringPaymentsService.GetPaymentPlan(createResponseModel.PlanId);

            getResponse.Should().NotBeNull();
            getResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            var getResponseModel = getResponse.Model;
            getResponseModel.ShouldBeEquivalentTo(createResponseModel,
                options => options.Excluding(o => o.PlanId).Excluding(o => o.Status));
            getResponseModel.PlanId.Should().NotBeNullOrEmpty();
            getResponseModel.Status.Should().HaveValue();
        }

        [Test]
        public void QueryPaymentPlan()
        {
            var paymentPlanModel = TestHelper.GetSinglePaymentPlanCreateModel();
            var createResponse = CheckoutClient.RecurringPaymentsService.CreatePaymentPlan(paymentPlanModel);
            var queryResponse = CheckoutClient.RecurringPaymentsService.QueryPaymentPlan(
                new QueryPaymentPlanRequest
                {
                    PlanTrackId = createResponse.Model.PaymentPlans.Single().PlanTrackId
                });

            queryResponse.Should().NotBeNull();
            queryResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            Assert.IsTrue(queryResponse.Model.TotalRows == 1);
            Assert.IsTrue(queryResponse.Model.Data.Single().Name == createResponse.Model.PaymentPlans.Single().Name);
            Assert.IsTrue(queryResponse.Model.Data.Single().PlanTrackId ==
                          createResponse.Model.PaymentPlans.Single().PlanTrackId);
            Assert.IsTrue(queryResponse.Model.Data.Single().RecurringCount ==
                          createResponse.Model.PaymentPlans.Single().RecurringCount);
            Assert.IsTrue(queryResponse.Model.Data.Single().PlanId == createResponse.Model.PaymentPlans.Single().PlanId);
            Assert.IsTrue(queryResponse.Model.Data.Single().Value == createResponse.Model.PaymentPlans.Single().Value);
            Assert.IsTrue(queryResponse.Model.Data.Single().AutoCapTime ==
                          createResponse.Model.PaymentPlans.Single().AutoCapTime);
            Assert.IsTrue(queryResponse.Model.Data.Single().Currency ==
                          createResponse.Model.PaymentPlans.Single().Currency);
            Assert.IsTrue(queryResponse.Model.Data.Single().Cycle == createResponse.Model.PaymentPlans.Single().Cycle);
        }

        [Test]
        public void UpdateCustomerPaymentPlan()
        {
            var cardCreateModel = TestHelper.GetCardChargeCreateModelWithNewPaymentPlan(TestHelper.RandomData.Email);
            var createResponseModel =
                CheckoutClient.ChargeService.ChargeWithCard(cardCreateModel).Model.CustomerPaymentPlans.Single();
            var customerPlanUpdateModel = TestHelper.GetCustomerPaymentPlanUpdateModel(createResponseModel.CardId, 4);
            var updateResponse =
                CheckoutClient.RecurringPaymentsService.UpdateCustomerPaymentPlan(createResponseModel.CustomerPlanId,
                    customerPlanUpdateModel);

            updateResponse.Should().NotBeNull();
            updateResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            updateResponse.Model.Message.Should().BeEquivalentTo("OK");
        }

        [Test]
        public void UpdatePaymentPlan()
        {
            var paymentPlanModel = TestHelper.GetSinglePaymentPlanCreateModel();
            var createResponseModel =
                CheckoutClient.RecurringPaymentsService.CreatePaymentPlan(paymentPlanModel).Model.PaymentPlans.Single();
            var updateResponse = CheckoutClient.RecurringPaymentsService.UpdatePaymentPlan(createResponseModel.PlanId,
                TestHelper.GetPaymentPlanUpdateModel());

            updateResponse.Should().NotBeNull();
            updateResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            updateResponse.Model.Message.Should().BeEquivalentTo("OK");
        }
    }
}