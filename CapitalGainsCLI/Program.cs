using CapitalGainsCLI.Interfaces;
using CapitalGainsCLI.Models;
using CapitalGainsCLI.Services;
using System.Text.Json;

namespace CapitalGainsCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ITaxCalculator calculator = new TaxCalculator();

            try
            {
                string inputJson = Console.In.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(inputJson))
                {
                    var operations = JsonSerializer.Deserialize<List<Operation>>(inputJson, new JsonSerializerOptions
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
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao processar entrada: {ex.Message}");
            }
        }
    }
}
