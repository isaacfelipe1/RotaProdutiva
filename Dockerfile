FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["RotaProdutiva.API/RotaProdutiva.API.csproj", "RotaProdutiva.API/"]
COPY ["RotaProdutiva.Application/RotaProdutiva.Application.csproj", "RotaProdutiva.Application/"]
COPY ["RotaProdutiva.Domain/RotaProdutiva.Domain.csproj", "RotaProdutiva.Domain/"]
COPY ["RotaProdutiva.Infrastructure/RotaProdutiva.Infrastructure.csproj", "RotaProdutiva.Infrastructure/"]
RUN dotnet restore "RotaProdutiva.API/RotaProdutiva.API.csproj"

COPY . .
WORKDIR /src/RotaProdutiva.API
RUN dotnet build "RotaProdutiva.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RotaProdutiva.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RotaProdutiva.API.dll"]
