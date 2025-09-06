using System.Text.Json;
using CapitalGainsCLI.Models;
using CapitalGainsCLI.Interfaces;
using CapitalGainsCLI.Services;

namespace CapitalGainsCLI
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using CapitalGainsCLI.Models;

    class Program
    {
        static void Main(string[] args)
        {
            ITaxCalculator calculator = new TaxCalculator();

            try
            {
                // Lê TODO o STDIN como uma única string
                string inputJson = Console.In.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(inputJson))
                {
                    // Desserializa o JSON completo
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
