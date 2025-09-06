using System.Text.Json.Serialization;

namespace CapitalGainsCLI.Models;

public class Operation
{
    [JsonPropertyName("operation")]
    public string OperationType { get; set; } = string.Empty; // "buy" ou "sell"
    
    [JsonPropertyName("unit-cost")]
    public decimal UnitCost { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}
