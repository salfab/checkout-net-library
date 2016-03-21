using System.Net;
using NUnit.Framework;
using Tests.Utils;

namespace Tests
{
    [TestFixture(Category = "LookupsApi")]
    public class LookupsServiceTests : BaseServiceTests
    {
        public void BinLookup_AssertResponseStatus(string bin, HttpStatusCode code)
        {
        }
    }
}