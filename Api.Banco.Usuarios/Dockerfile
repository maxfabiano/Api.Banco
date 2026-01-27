FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080 

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- PASSO 1: INSTALAR DOTNET-EF ---
RUN dotnet tool install --global dotnet-ef
# Adiciona a ferramenta ao PATH do sistema para ser reconhecida
ENV PATH="$PATH:/root/.dotnet/tools"

# Copia os projetos
COPY ["Api.Banco.Usuarios/Api.Banco.Usuarios.csproj", "Api.Banco.Usuarios/"]
COPY ["Api.Banco.Database.Usuarios/Api.Banco.Database.Usuarios.csproj", "Api.Banco.Database.Usuarios/"]

RUN dotnet restore "Api.Banco.Usuarios/Api.Banco.Usuarios.csproj"
COPY . .

# --- PASSO 2: GERAR MIGRATION AUTOMÁTICA ---
# Entramos na pasta da API (Startup Project)
WORKDIR "/src/Api.Banco.Usuarios"

# Lógica: Se não houver pasta Migrations, ele cria a 'Initial'. Se houver, tenta criar uma nova.
RUN dotnet ef migrations add "Fix_$(date +%Y%m%d%H%M)" \
    --project ../Api.Banco.Database.Usuarios/Api.Banco.Database.Usuarios.csproj \
    --startup-project Api.Banco.Usuarios.csproj || echo "Aviso: Usando migrations existentes."


# --- PASSO 3: COMPILAR E PUBLICAR ---
# Importante buildar APÓS gerar a migration para incluir os novos arquivos .cs
RUN dotnet build "Api.Banco.Usuarios.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Api.Banco.Usuarios.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.Banco.Usuarios.dll"]