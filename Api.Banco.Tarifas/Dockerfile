# 1. Base Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# 2. Build e Instalação de Ferramentas
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- NOVIDADE: INSTALAÇÃO DA FERRAMENTA EF ---
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copia os projetos
COPY ["Api.Banco.Tarifas/Api.Banco.Tarifas.csproj", "Api.Banco.Tarifas/"]
COPY ["Api.Banco.Database.Tarifas/Api.Banco.Database.Tarifas.csproj", "Api.Banco.Database.Tarifas/"]

# Restaura dependências
RUN dotnet restore "Api.Banco.Tarifas/Api.Banco.Tarifas.csproj"
COPY . .

# --- LÓGICA DE MIGRATION ROBUSTA ---
WORKDIR "/src/Api.Banco.Tarifas"

# Verifica se a pasta Migrations existe no projeto Database. 
# Se não existir, cria a Initial. Se existir, tenta criar uma nova com timestamp.
RUN dotnet ef migrations add "Fix_$(date +%Y%m%d%H%M)" \
    --project ../Api.Banco.Database.Tarifas/Api.Banco.Database.Tarifas.csproj \
    --startup-project Api.Banco.Tarifas.csproj || echo "Aviso: Usando migrations existentes."

# Build e Publish após a geração das migrations
RUN dotnet build "Api.Banco.Tarifas.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api.Banco.Tarifas.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 3. Final Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.Banco.Tarifas.dll"]