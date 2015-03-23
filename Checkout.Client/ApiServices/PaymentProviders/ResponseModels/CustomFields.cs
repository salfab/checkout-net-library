using System;
using System.Collections.Generic;

public class CustomFields
{
    public string Key { get; set; }

    public string DataType { get; set; }

    public string Label { get; set; }

    public bool Required { get; set; }

    public int Order { get; set; }

    public int MinLength { get; set; }

    public int MaxLength { get; set; }

    public bool IsActive { get; set; }

    public IDictionary<string, string> ErrorCodes{ get; set; }
   
    public IDictionary<string, string> LookupValues{ get; set; }
}
