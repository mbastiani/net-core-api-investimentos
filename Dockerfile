FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Investimentos.Api/Investimentos.Api.csproj", "Investimentos.Api/"]
COPY ["Investimentos.Infra/Investimentos.Infra.csproj", "Investimentos.Infra/"]
COPY ["Investimentos.Domain/Investimentos.Domain.csproj", "Investimentos.Domain/"]
COPY ["Investimentos.Service/Investimentos.Service.csproj", "Investimentos.Service/"]
RUN dotnet restore "Investimentos.Api/Investimentos.Api.csproj"
COPY . .
WORKDIR "/src/Investimentos.Api"
RUN dotnet build "Investimentos.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Investimentos.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Investimentos.Api.dll"]