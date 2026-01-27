# 1. Base Runtime (ASP.NET para suporte a Web APIs)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# 2. Fase de Compilação e Automação EF
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- PASSO CRUCIAL: Instalação do Global Tool do Entity Framework ---
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copia os arquivos de projeto (Estrutura lado a lado)
COPY ["Api.Banco.Transferencia/Api.Banco.Transferencia.csproj", "Api.Banco.Transferencia/"]
COPY ["Api.Banco.Database.Transferencia/Api.Banco.Database.Transferencia.csproj", "Api.Banco.Database.Transferencia/"]

# Restaura as dependências
RUN dotnet restore "Api.Banco.Transferencia/Api.Banco.Transferencia.csproj"

# Copia todo o código-fonte
COPY . .

# --- LÓGICA DE MIGRATION ROBUSTA ---
WORKDIR "/src/Api.Banco.Transferencia"

# Se não existir a pasta Migrations no projeto Database, cria a Initial.
# Se já existir, tenta criar uma nova com base no horário (timestamp).
RUN dotnet ef migrations add "Fix_$(date +%Y%m%d%H%M)" \
    --project ../Api.Banco.Database.Transferencia/Api.Banco.Database.Transferencia.csproj \
    --startup-project Api.Banco.Transferencia.csproj || echo "Aviso: Usando migrations existentes."


# Build focado no projeto de Transferência (incluindo as migrations geradas acima)
RUN dotnet build "Api.Banco.Transferencia.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Fase de publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api.Banco.Transferencia.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Fase Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.Banco.Transferencia.dll"]