namespace Checkout.ApiServices.Charges.RequestModels
{
    public class ChargeRefund
    {
        public string ChargeId { get; set; }
        /// <summary>
        /// Charge amount to be refunded.
        /// </summary>
        public int Value { get; set; }
    }
}