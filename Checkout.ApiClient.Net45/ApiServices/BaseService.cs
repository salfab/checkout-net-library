namespace Checkout.ApiServices
{
    public abstract class BaseService
    {
        protected readonly ApiHttpClient ApiHttpClient;

        protected BaseService()
        {
            ApiHttpClient = new ApiHttpClient();
        }
    }
}
