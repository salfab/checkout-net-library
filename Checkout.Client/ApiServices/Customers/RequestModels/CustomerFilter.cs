using System;

namespace Checkout.ApiServices.Customers.RequestModels
{
    public class CustomerFilter
    {
        public int Count { get; set; }
        public int Offset { get; set; }

        /// <summary>
        /// Holds created start and end dates
        /// </summary>
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
