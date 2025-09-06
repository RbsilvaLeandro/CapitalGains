FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia csproj e restaura depend�ncias
COPY ./CapitalGainsCLI/*.csproj ./CapitalGainsCLI/
RUN dotnet restore ./CapitalGainsCLI/CapitalGainsCLI.csproj

# Copia o restante do c�digo e faz o build
COPY ./CapitalGainsCLI/ ./CapitalGainsCLI/
WORKDIR /app/CapitalGainsCLI
RUN dotnet publish -c Release -o out

# Build final (imagem menor s� com runtime)
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/CapitalGainsCLI/out ./
ENTRYPOINT ["dotnet", "CapitalGainsCLI.dll"]
