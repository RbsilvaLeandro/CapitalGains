FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./CapitalGainsCLI/*.csproj ./CapitalGainsCLI/
RUN dotnet restore ./CapitalGainsCLI/CapitalGainsCLI.csproj

COPY ./CapitalGainsCLI/ ./CapitalGainsCLI/
WORKDIR /app/CapitalGainsCLI
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/CapitalGainsCLI/out ./
ENTRYPOINT ["dotnet", "CapitalGainsCLI.dll"]
