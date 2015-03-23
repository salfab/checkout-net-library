namespace Checkout.ApiServices.Charges.RequestModels
{
    public class ChargeCapture
    {
        public string ChargeId { get; set; }
        /// <summary>
        /// Charge amount to be captured.
        /// </summary>
        public int Value { get; set; }
    }
}
