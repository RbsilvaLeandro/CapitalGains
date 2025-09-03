# Use .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia csproj e restaura dependências
COPY src/CapitalGainsCLI/*.csproj ./CapitalGainsCLI/
RUN dotnet restore ./CapitalGainsCLI/CapitalGainsCLI.csproj

# Copia o restante do código e build
COPY src/CapitalGainsCLI/. ./CapitalGainsCLI/
WORKDIR /app/CapitalGainsCLI
RUN dotnet publish -c Release -o out

# Build final
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/CapitalGainsCLI/out ./
ENTRYPOINT ["dotnet", "CapitalGainsCLI.dll"]
