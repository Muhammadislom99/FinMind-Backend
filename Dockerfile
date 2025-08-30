FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FinMind.API/FinMind.API.csproj", "FinMind.API/"]
COPY ["FimMind.Application/FimMind.Application.csproj", "FimMind.Application/"]
COPY ["FinMind.Application.Contract/FinMind.Application.Contract.csproj", "FinMind.Application.Contract/"]
COPY ["FinMind.Domain/FinMind.Domain.csproj", "FinMind.Domain/"]
COPY ["FinMind.Infrastructure/FinMind.Infrastructure.csproj", "FinMind.Infrastructure/"]
RUN dotnet restore "FinMind.API/FinMind.API.csproj"
COPY . .
WORKDIR "/src/FinMind.API"
RUN dotnet build "./FinMind.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FinMind.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinMind.API.dll"]
