using CapitalGainsCLI.Models;
using CapitalGainsCLI.Services;

namespace CapitalGainsCLI.Tests;

[TestFixture]
public class TaxCalculatorTests
{
    private TaxCalculator _calculator;

    [SetUp]
    public void Setup() => _calculator = new TaxCalculator();

    private List<TaxResult> Calc(List<Operation> ops) => _calculator.CalculateTaxes(ops);

    [Test]
    public void Case1_BuyThenSellBelowThreshold()
    {
        var ops = new List<Operation>
        {
            new() { OperationType="buy", UnitCost=10.00m, Quantity=100 },
            new() { OperationType="sell", UnitCost=15.00m, Quantity=50 },
            new() { OperationType="sell", UnitCost=15.00m, Quantity=50 }
        };
        var result = Calc(ops);
        Assert.That(new decimal[] { 0, 0, 0 }, Is.EqualTo(result.ConvertAll(r => r.Tax).ToArray()));
    }

    [Test]
    public void Case2_SellProfitAndLoss()
    {
        var ops = new List<Operation>
        {
            new() { OperationType="buy", UnitCost=10.00m, Quantity=10000 },
            new() { OperationType="sell", UnitCost=20.00m, Quantity=5000 },
            new() { OperationType="sell", UnitCost=5.00m, Quantity=5000 }
        };
        var result = Calc(ops);
        Assert.That(new decimal[] { 0, 10000, 0 }, Is.EqualTo(result.ConvertAll(r => r.Tax).ToArray()));
    }

    [Test]
    public void Case3_ProfitAfterLoss()
    {
        var ops = new List<Operation>
        {
            new() { OperationType="buy", UnitCost=10.00m, Quantity=10000 },
            new() { OperationType="sell", UnitCost=5.00m, Quantity=5000 },
            new() { OperationType="sell", UnitCost=20.00m, Quantity=3000 }
        };
        var result = Calc(ops);
        Assert.That(new decimal[] { 0, 0, 1000 }, Is.EqualTo(result.ConvertAll(r => r.Tax).ToArray()));
    }

    [Test]
    public void Case4_BuyThenSellWithWeightedAverageNoProfit()
    {
        var ops = new List<Operation>
        {
            new() { OperationType="buy", UnitCost=10.00m, Quantity=10000 },
            new() { OperationType="buy", UnitCost=25.00m, Quantity=5000 },
            new() { OperationType="sell", UnitCost=15.00m, Quantity=10000 }
        };
        var result = Calc(ops);
        Assert.That(new decimal[] { 0, 0, 0 }, Is.EqualTo(result.ConvertAll(r => r.Tax).ToArray()));
    }

    [Test]
    public void Case5_MixedSells()
    {
        var ops = new List<Operation>
        {
            new() { OperationType="buy", UnitCost=10.00m, Quantity=10000 },
            new() { OperationType="buy", UnitCost=25.00m, Quantity=5000 },
            new() { OperationType="sell", UnitCost=15.00m, Quantity=10000 },
            new() { OperationType="sell", UnitCost=25.00m, Quantity=5000 }
        };
        var result = Calc(ops);
        Assert.That(new decimal[] { 0, 0, 0, 10000 }, Is.EqualTo(result.ConvertAll(r => r.Tax).ToArray()));
    }

    [Test]
    public void Case6_ComplexLossesAndMultipleSells()
    {
        var ops = new List<Operation>
        {
            new() { OperationType="buy", UnitCost=10.00m, Quantity=10000 },
            new() { OperationType="sell", UnitCost=2.00m, Quantity=5000 },
            new() { OperationType="sell", UnitCost=20.00m, Quantity=2000 },
            new() { OperationType="sell", UnitCost=20.00m, Quantity=2000 },
            new() { OperationType="sell", UnitCost=25.00m, Quantity=1000 }
        };
        var result = Calc(ops);
        Assert.That(new decimal[] { 0, 0, 0, 0, 3000 }, Is.EqualTo(result.ConvertAll(r => r.Tax).ToArray()));
    }

    [Test]
    public void Case7_MultipleBuySellWithLossOffset()
    {
        var ops = new List<Operation>
        {
            new() { OperationType="buy", UnitCost=20.00m, Quantity=10000 },
            new() { OperationType="sell", UnitCost=15.00m, Quantity=5000 },
            new() { OperationType="sell", UnitCost=30.00m, Quantity=4350 },
            new() { OperationType="sell", UnitCost=30.00m, Quantity=650 }
        };
        var result = Calc(ops);
        Assert.That(new decimal[] { 0, 0, 3700, 0 }, Is.EqualTo(result.ConvertAll(r => r.Tax).ToArray()));
    }

    [Test]
    public void Case8_HighVolumeBuysAndSells()
    {
        var ops = new List<Operation>
        {
            new() { OperationType="buy", UnitCost=10.00m, Quantity=10000 },
            new() { OperationType="sell", UnitCost=50.00m, Quantity=10000 },
            new() { OperationType="buy", UnitCost=20.00m, Quantity=10000 },
            new() { OperationType="sell", UnitCost=50.00m, Quantity=10000 }
        };
        var result = Calc(ops);
        Assert.That(new decimal[] { 0, 80000, 0, 60000 }, Is.EqualTo(result.ConvertAll(r => r.Tax).ToArray()));
    }

    [Test]
    public void Case9_MixedComplexOperations()
    {
        var ops = new List<Operation>
        {
            new() { OperationType="buy", UnitCost=5000.00m, Quantity=10 },
            new() { OperationType="sell", UnitCost=4000.00m, Quantity=5 },
            new() { OperationType="buy", UnitCost=15000.00m, Quantity=5 },
            new() { OperationType="buy", UnitCost=4000.00m, Quantity=2 },
            new() { OperationType="buy", UnitCost=23000.00m, Quantity=2 },
            new() { OperationType="sell", UnitCost=20000.00m, Quantity=1 },
            new() { OperationType="sell", UnitCost=12000.00m, Quantity=10 },
            new() { OperationType="sell", UnitCost=15000.00m, Quantity=3 }
        };
        var result = Calc(ops);
        Assert.That(new decimal[] { 0, 0, 0, 0, 0, 0, 1000, 2400 }, Is.EqualTo(result.ConvertAll(r => r.Tax).ToArray()));
    }
}
