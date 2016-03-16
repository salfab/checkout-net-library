using Checkout.ApiServices.RecurringPayments.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.ApiServices.RecurringPayments.ResponseModels
{
    public class QueryCustomerPaymentPlanResponse : CustomerPaymentPlan
    {
        public int TotalRows { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }

        public List<PaymentPlan> Data { get; set; }
    }
}
