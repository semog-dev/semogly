# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copia os arquivos de projeto (.csproj) individualmente para aproveitar o cache do Docker
COPY ["Semogly.Core.Api/Semogly.Core.Api.csproj", "Semogly.Core.Api/"]
COPY ["Semogly.Core.Application/Semogly.Core.Application.csproj", "Semogly.Core.Application/"]
COPY ["Semogly.Core.Domain/Semogly.Core.Domain.csproj", "Semogly.Core.Domain/"]
COPY ["Semogly.Core.Infrastructure/Semogly.Core.Infrastructure.csproj", "Semogly.Core.Infrastructure/"]

# 2. Restaura as dependências baseando-se no projeto da API
RUN dotnet restore "Semogly.Core.Api/Semogly.Core.Api.csproj"

# 3. Copia todo o restante dos arquivos
COPY . .

# 4. Compila a API
WORKDIR "/src/Semogly.Core.Api"
RUN dotnet publish "Semogly.Core.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Porta que o Render usa por padrão ou a que você definir
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "Semogly.Core.Api.dll"]