using System;
using System.Collections.Generic;

public class LocalPaymentProviderModel
{
    public string ProviderId { get; set; }

    public string PaymentToken { get; set; }

    public string Ip { get; set; }

    public string CountryCode { get; set; }

    public string Limit { get; set; }

    public string Name { get; set; }

    public List<string> Region { get; set; }
}
