Projeto separado em Models, Interfaces e Services para evitar acoplamento

Models (Operation, TaxResult) → Representam o domínio (os dados básicos: operações de compra/venda e resultado de imposto).
Interfaces (ITaxCalculator) → Define o contrato que o serviço deve seguir, permitindo trocar implementações no futuro sem quebrar o código.
Services (TaxCalculator) → Implementa a regra de negócio (cálculo da média ponderada, prejuízo acumulado e imposto).

Separação que permite escalar para múltiplas regras fiscais ou até substituir a lógica de cálculo sem mexer no Program.cs
A idéia e usar uma imagem pequena e otimizada (runtime sem SDK) e evitar expor código-fonte no container.

Separação de responsabilidades (dados, regra, interface, Saplicação, testes).
Pronto para extensão → dá para criar outra implementação de ITaxCalculator (ex: imposto em outro país).

Boas práticas aplicadas

SOLID: TaxCalculator isolado por interface ITaxCalculator. Fácil de testar e trocar a implementação.
Testes: NUnit cobrindo casos de lucro, prejuízo, offset, taxa zero, média ponderada.
CLI: Recebe stdin, não depende de estado externo.
Docker: Imagem mínima com SDK para build e Runtime para execução.

Instruções para o build 
Na raiz do projeto execute os comandos
docker build -t capital-gains .
Get-Content input.json -Raw| docker run -i capital-gains
