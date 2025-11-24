# Etapa base (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Render suele usar un puerto interno (por ejemplo 10000)
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar el proyecto y restaurar
COPY ["DiarioMagna.csproj", "."]
RUN dotnet restore "./DiarioMagna.csproj"

# Copiar el resto del código
COPY . .
WORKDIR "/src/."

# Compilar
RUN dotnet build "./DiarioMagna.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DiarioMagna.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "DiarioMagna.dll"]
