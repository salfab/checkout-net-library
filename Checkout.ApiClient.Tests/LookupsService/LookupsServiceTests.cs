using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tests.Utils;

namespace Tests
{
    [TestFixture(Category = "LookupsApi")]
    public class LookupsServiceTests : BaseServiceTests
    {
        [TestCaseSource(typeof(TestScenarios), TestScenarios.Test_BinLookup_AssertResponseStatus)]
        public void BinLookup_AssertResponseStatus(string bin, HttpStatusCode code)
        {

        }
    }
}
