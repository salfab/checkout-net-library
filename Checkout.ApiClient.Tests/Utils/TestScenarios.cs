using Checkout.ApiServices.SharedModels;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Utils
{
    public class TestScenarios
    {
        public static SortColumn?[] QueryTransaction_ColumnSorting = new SortColumn?[]
        {
            null
            ,SortColumn.Amount
            ,SortColumn.BusinessName
            ,SortColumn.ChannelName
            ,SortColumn.Currency
            ,SortColumn.Date
            ,SortColumn.Email
            ,SortColumn.Id
            ,SortColumn.LiveMode
            ,SortColumn.Name
            ,SortColumn.OriginId
            ,SortColumn.ResponseCode
            ,SortColumn.Scheme
            ,SortColumn.Status
            ,SortColumn.TrackId
            ,SortColumn.Type
        };

        public static SortOrder?[] QueryTransaction_Sorting = new SortOrder?[]
        {
            null
            ,SortOrder.Asc
            ,SortOrder.Desc
        };

        public static string[] QueryTransaction_Pagination = new string[]
        {
            null
            ,"asd"
            ,"-1"
            ,"2"
            ,"9999"
        };

        public static string[] QueryTransaction_SearchString = new string[]
        {
            "test"
            ,"captured"
            ,"TRK12345"
        };

        public static int?[] QueryTransaction_PageSize = new int?[]
        {
            null
            ,9
            ,15
            ,251
        };
    }
}
