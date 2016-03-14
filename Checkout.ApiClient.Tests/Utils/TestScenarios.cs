using Checkout.ApiServices.SharedModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FilterAction = Checkout.ApiServices.SharedModels.Action;

namespace Tests.Utils
{
    public class TestScenarios
    {
        public const string Test_QueryTransaction_ColumnSorting = "QueryTransaction_ColumnSorting";
        public const string Test_QueryTransaction_Sorting = "QueryTransaction_Sorting";
        public const string Test_QueryTransaction_Pagination = "QueryTransaction_Pagination";
        public const string Test_QueryTransaction_SearchString = "QueryTransaction_SearchString";
        public const string Test_QueryTransaction_PageSize = "QueryTransaction_PageSize";
        public const string Test_QueryTransaction_Filter_Action = "QueryTransaction_Filter_Action";
        public const string Test_QueryTransaction_Filter_Field = "QueryTransaction_Filter_Field";
        public const string Test_QueryTransaction_Filter_Operator = "QueryTransaction_Filter_Operator";
        public const string Test_QueryTransaction_Filter_Value = "QueryTransaction_Filter_Value";
        public const string Test_QueryTransaction_Charge_FilterByCardNumber = "QueryTransaction_Charge_FilterByCardNumber";

        public const string Test_BinLookup_AssertResponseStatus = "BinLookup_AssertResponseStatus";

        #region Query Transaction

        public static SortColumn?[] QueryTransaction_ColumnSorting =
        {
            null,
            SortColumn.Amount,
            SortColumn.BusinessName,
            SortColumn.ChannelName,
            SortColumn.Currency,
            SortColumn.Date,
            SortColumn.Email,
            SortColumn.Id,
            SortColumn.LiveMode,
            SortColumn.Name,
            SortColumn.OriginId,
            SortColumn.ResponseCode,
            SortColumn.Scheme,
            SortColumn.Status,
            SortColumn.TrackId,
            SortColumn.Type,
        };

        public static SortOrder?[] QueryTransaction_Sorting =
        {
            null,
            SortOrder.Asc,
            SortOrder.Desc
        };

        public static string[] QueryTransaction_Pagination =
        {
            null,
            "asd",
            "-1",
            "2",
            "9999",
        };

        public static string[] QueryTransaction_SearchString =
        {
            "test",
            "captured",
            "TRK12345"
        };

        public static int?[] QueryTransaction_PageSize =
        {
            null,
            9,
            15,
            251
        };

        public static FilterAction?[] QueryTransaction_Filter_Action =
        {
            null,
            FilterAction.Exclude,
            FilterAction.Include
        };

        public static Field?[] QueryTransaction_Filter_Field =
        {
            null,
            Field.Email,
            Field.ChargeId,
            Field.CardNumber,
            Field.TrackId,
            Field.Status
        };

        public static object[] QueryTransaction_Filter_Operator =
        {
            new object[] { "test_68dbef9b-492a-49da-977d-b328c9b4dbab@checkouttest.co.uk", null },
            new object[] { "test", Operator.Begins },
            new object[] { "test", Operator.Contains },
            new object[] { "@checkouttest.co.uk", Operator.Ends },
            new object[] { "test_68dbef9b-492a-49da-977d-b328c9b4dbab@checkouttest.co.uk", Operator.Equals }
        };

        public static string[] QueryTransaction_Filter_Value = { "test", null };

        public static object[] QueryTransaction_Charge_FilterByCardNumber =
        {
            new object[] { "5313581000123430", "257", "6", "2017" },
            new object[] { "5436031030606378", "257", "6", "2017" },
            new object[] { "6759649826438453", "257", "6", "2017" },
            new object[] { "4242424242424242", "100", "6", "2018" }
        };

        #endregion

        public static object[] BinLookup_AssertResponseStatus = 
            new object[]
        {
            new object[] { "465941", HttpStatusCode.OK},
            new object[] {"45as", HttpStatusCode.BadRequest},
            new object[] {"14.255", HttpStatusCode.BadRequest},
            new object[] {"", HttpStatusCode.OK},
            new object[] {"", HttpStatusCode.OK},
        };
    }
}
