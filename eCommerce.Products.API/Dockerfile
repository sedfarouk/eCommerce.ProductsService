FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["eCommerce.Products.API/eCommerce.Products.API.csproj", "eCommerce.Products.API/"]
RUN dotnet restore "eCommerce.Products.API/eCommerce.Products.API.csproj"
COPY . .
WORKDIR "/src/eCommerce.Products.API"
RUN dotnet build "./eCommerce.Products.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish 
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./eCommerce.Products.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final  
WORKDIR /app
COPY --from=publish /app/publish .
ENV POSTGRES_DATABASE=ecommerceproductsdatabase
ENV POSTGRES_HOST=postgres
ENV POSTGRES_PORT=5432 
ENV POSTGRES_USER=postgres
ENTRYPOINT ["dotnet", "eCommerce.Products.API.dll"]
