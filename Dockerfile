# 1. Çalıştırma ortamı (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. Derleme ortamı (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Proje dosyalarını kopyala (Domain katmanı eklendi!)
COPY ["src/ProjectGym.API/ProjectGym.API.csproj", "src/ProjectGym.API/"]
COPY ["src/ProjectGym.Application/ProjectGym.Application.csproj", "src/ProjectGym.Application/"]
COPY ["src/ProjectGym.Infrastructure/ProjectGym.Infrastructure.csproj", "src/ProjectGym.Infrastructure/"]
COPY ["src/ProjectGym.Domain/ProjectGym.Domain.csproj", "src/ProjectGym.Domain/"]

RUN dotnet restore "src/ProjectGym.API/ProjectGym.API.csproj"

# Tüm kodları kopyala ve derle
COPY . .
WORKDIR "/src/src/ProjectGym.API"
RUN dotnet build "ProjectGym.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProjectGym.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 3. Canlıya Alma
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectGym.API.dll"]