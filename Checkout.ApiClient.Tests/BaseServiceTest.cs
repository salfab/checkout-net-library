using Checkout;
using NUnit.Framework;

namespace Tests
{
    public abstract class BaseServiceTest
    {
        protected APIClient CheckoutClient;

        [SetUp]
        public virtual void Init()
        {
            CheckoutClient = new APIClient();
        }
    }
}
