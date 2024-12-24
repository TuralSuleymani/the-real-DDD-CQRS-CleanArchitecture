#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/VOEConsulting.Flame.BasketContext.Api/VOEConsulting.Flame.BasketContext.Api.csproj", "src/VOEConsulting.Flame.BasketContext.Api/"]
COPY ["src/VOEConsulting.Flame.BasketContext.Application/VOEConsulting.Flame.BasketContext.Application.csproj", "src/VOEConsulting.Flame.BasketContext.Application/"]
COPY ["src/VOEConsulting.Flame.BasketContext.Domain/VOEConsulting.Flame.BasketContext.Domain.csproj", "src/VOEConsulting.Flame.BasketContext.Domain/"]
COPY ["src/VOEConsulting.Flame.Common.Domain/VOEConsulting.Flame.Common.Domain.csproj", "src/VOEConsulting.Flame.Common.Domain/"]
COPY ["src/VOEConsulting.Flame.Common.Core/VOEConsulting.Flame.Common.Core.csproj", "src/VOEConsulting.Flame.Common.Core/"]
COPY ["src/VOEConsulting.Flame.BasketContext.Infrastructure/VOEConsulting.Flame.BasketContext.Infrastructure.csproj", "src/VOEConsulting.Flame.BasketContext.Infrastructure/"]
RUN dotnet restore "./src/VOEConsulting.Flame.BasketContext.Api/VOEConsulting.Flame.BasketContext.Api.csproj"
COPY . .
WORKDIR "/src/src/VOEConsulting.Flame.BasketContext.Api"
RUN dotnet build "./VOEConsulting.Flame.BasketContext.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VOEConsulting.Flame.BasketContext.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VOEConsulting.Flame.BasketContext.Api.dll"]