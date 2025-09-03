namespace CapitalGainsCLI.Models;

public class Operation
{
    public string OperationType { get; set; } = string.Empty; // "buy" ou "sell"
    public decimal UnitCost { get; set; }
    public int Quantity { get; set; }
}
