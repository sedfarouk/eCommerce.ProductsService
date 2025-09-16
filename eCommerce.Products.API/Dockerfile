FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["eCommerce.ProductsService/eCommerce.ProductsService.csproj", "eCommerce.ProductsService/"]
RUN dotnet restore "eCommerce.ProductsService/eCommerce.ProductsService.csproj"
COPY . .
WORKDIR "/src/eCommerce.ProductsService"
RUN dotnet build "./eCommerce.ProductsService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./eCommerce.ProductsService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eCommerce.ProductsService.dll"]
