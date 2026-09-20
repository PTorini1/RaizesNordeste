FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RaizesNordeste.Domain/RaizesNordeste.Domain.csproj", "RaizesNordeste.Domain/"]
COPY ["RaizesNordeste.Application/RaizesNordeste.Application.csproj", "RaizesNordeste.Application/"]
COPY ["RaizesNordeste.Infrastructure/RaizesNordeste.Infrastructure.csproj", "RaizesNordeste.Infrastructure/"]
COPY ["RaizesNordesteApi/RaizesNordesteApi.csproj", "RaizesNordesteApi/"]
RUN dotnet restore "./RaizesNordesteApi/RaizesNordesteApi.csproj"
COPY . .
WORKDIR "/src/RaizesNordesteApi"
RUN dotnet build "./RaizesNordesteApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RaizesNordesteApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaizesNordesteApi.dll"]
