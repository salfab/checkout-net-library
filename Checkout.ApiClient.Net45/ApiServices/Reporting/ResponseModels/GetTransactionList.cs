using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.ApiServices.Reporting.ResponseModels
{
    public class GetTransactionList
    {
        /// <summary>
        /// The total number of records returned
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// The number of records to return per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The page number of the result set to return
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// An Array of Query Object
        /// </summary>
        public List<Transaction> Data { get; set; }
    }
}
