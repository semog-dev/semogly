# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia o arquivo de projeto e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o restante dos arquivos e compila
COPY . ./
RUN dotnet publish -c Release -o out

# Estágio 2: Runtime (Imagem Final)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expõe a porta padrão do ASP.NET Core
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "Semogly.Core.Api.dll"]