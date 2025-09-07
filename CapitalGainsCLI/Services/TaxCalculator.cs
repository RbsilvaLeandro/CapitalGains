using CapitalGainsCLI.Interfaces;
using CapitalGainsCLI.Models;

namespace CapitalGainsCLI.Services;

public class TaxCalculator : ITaxCalculator
{
    private const decimal TaxRate = 0.20m;
    private const decimal TaxFreeThreshold = 20000m;

    public List<TaxResult> CalculateTaxes(List<Operation> operations)
    {
        var results = new List<TaxResult>();
        decimal weightedAverage = 0m;
        int totalQuantity = 0;
        decimal accumulatedLoss = 0m;

        foreach (var op in operations)
        {
            if (op.OperationType == "buy")
            {
                weightedAverage = ((weightedAverage * totalQuantity) + (op.UnitCost * op.Quantity)) / (totalQuantity + op.Quantity);
                totalQuantity += op.Quantity;
                results.Add(new TaxResult { Tax = 0m });
            }
            else if (op.OperationType == "sell")
            {
                decimal totalSaleValue = op.UnitCost * op.Quantity;
                decimal profit = (op.UnitCost - weightedAverage) * op.Quantity;

                totalQuantity -= op.Quantity;

                decimal tax = 0m;

                if (totalSaleValue > TaxFreeThreshold && profit > 0)
                {
                    decimal taxableProfit = Math.Max(0, profit - accumulatedLoss);
                    accumulatedLoss = Math.Max(0, accumulatedLoss - profit);
                    tax = Math.Round(taxableProfit * TaxRate, 2);
                }
                else if (profit < 0)
                {
                    accumulatedLoss += Math.Abs(profit);
                }

                results.Add(new TaxResult { Tax = tax });
            }
            else
            {
                throw new InvalidOperationException($"Tipo de operação inválido: {op.OperationType}");
            }
        }

        return results;
    }
}