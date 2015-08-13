using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.ApiServices.SharedModels
{
    public class BasePagination
    {
        public int? Count { get; set; }
        public int? Offset { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
