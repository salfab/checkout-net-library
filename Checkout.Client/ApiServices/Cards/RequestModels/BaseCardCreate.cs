namespace Checkout.ApiServices.Cards.RequestModels
{
    public class BaseCardCreate : BaseCard
    {
        public string Number { get; set; }
        public string Cvv { get; set; }
    }
}