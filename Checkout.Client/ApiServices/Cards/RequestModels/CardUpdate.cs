namespace Checkout.ApiServices.Cards.RequestModels
{
    public class CardUpdate
    {
        public string CardId { get; set; }
        public string CustomerId { get; set; }
        public BaseCard Card { get; set; }
    }
}