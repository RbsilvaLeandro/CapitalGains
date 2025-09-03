using System.Text.Json;
using CapitalGainsCLI.Models;
using CapitalGainsCLI.Interfaces;
using CapitalGainsCLI.Services;     

namespace CapitalGainsCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ITaxCalculator calculator = new TaxCalculator();          

            string? line;

            while ((line = Console.ReadLine()) != null && line != "")
            {
                try
                {
                    var operations = JsonSerializer.Deserialize<List<Operation>>(line, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (operations != null)
                    {
                        var taxes = calculator.CalculateTaxes(operations);
                        string output = JsonSerializer.Serialize(taxes);
                        Console.WriteLine(output);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Erro ao processar linha: {ex.Message}");
                }
            }
        }
    }

    
}
