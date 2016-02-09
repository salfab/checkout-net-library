using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public static class Extensions
    {
        /// <summary>
        /// Returns a value indicating whether a specified substring occurs within this string using OrdinalIgnoreCase.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(this string source, string match)
        {
            if (source == null) return false;
            return source.IndexOf(match, StringComparison.OrdinalIgnoreCase) != -1;
        }
    }
}
