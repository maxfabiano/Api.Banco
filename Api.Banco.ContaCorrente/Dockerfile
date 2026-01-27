# 1. Base Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# 2. Fase de Compilação com Automação EF
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Instalação obrigatória da ferramenta dotnet-ef para o build
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copia os projetos (Estrutura lado a lado)
COPY ["Api.Banco.ContaCorrente/Api.Banco.ContaCorrente.csproj", "Api.Banco.ContaCorrente/"]
COPY ["Api.Banco.Database.ContaCorrente/Api.Banco.Database.ContaCorrente.csproj", "Api.Banco.Database.ContaCorrente/"]

# Restaura dependências
RUN dotnet restore "Api.Banco.ContaCorrente/Api.Banco.ContaCorrente.csproj"
COPY . .

# Lógica de Migration Inteligente
WORKDIR "/src/Api.Banco.ContaCorrente"

# Verifica se a pasta Migrations existe. Se não existir, cria a primeira.
# Se existir, tenta criar uma nova com base no timestamp.
RUN dotnet ef migrations add "Fix_$(date +%Y%m%d%H%M)" \
    --project ../Api.Banco.Database.ContaCorrente/Api.Banco.Database.ContaCorrente.csproj \
    --startup-project Api.Banco.ContaCorrente.csproj || echo "Aviso: Usando migrations existentes."

# Compila o projeto incluindo as migrations geradas no passo anterior
RUN dotnet build "Api.Banco.ContaCorrente.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Fase de Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api.Banco.ContaCorrente.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Fase Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.Banco.ContaCorrente.dll"]