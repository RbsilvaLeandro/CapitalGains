using CapitalGainsCLI.Models;

namespace CapitalGainsCLI.Interfaces;

public interface ITaxCalculator
{        List<TaxResult> CalculateTaxes(List<Operation> operations);
}