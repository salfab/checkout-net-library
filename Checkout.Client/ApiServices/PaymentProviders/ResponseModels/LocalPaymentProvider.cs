using System;
using System.Collections.Generic;

public class LocalPaymentProvider
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public bool Iframe { get; set; }
    public IList<Region> Regions{ get; set; }
    public IList<string> CountryCodes{ get; set; }
    public IDictionary<String, String> Dimensions{ get; set; }
    public IList<CustomFields> customerFields;
}
