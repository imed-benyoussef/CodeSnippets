#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Aiglusoft.IAM.Server/Aiglusoft.IAM.Server.csproj", "src/Aiglusoft.IAM.Server/"]
COPY ["src/Aiglusoft.IAM.Application/Aiglusoft.IAM.Application.csproj", "src/Aiglusoft.IAM.Application/"]
COPY ["src/Aiglusoft.IAM.Domain/Aiglusoft.IAM.Domain.csproj", "src/Aiglusoft.IAM.Domain/"]
COPY ["src/Aiglusoft.IAM.Shared/Aiglusoft.IAM.Shared.csproj", "src/Aiglusoft.IAM.Shared/"]
COPY ["src/Aiglusoft.IAM.Infrastructure/Aiglusoft.IAM.Infrastructure.csproj", "src/Aiglusoft.IAM.Infrastructure/"]
RUN dotnet restore "./src/Aiglusoft.IAM.Server/Aiglusoft.IAM.Server.csproj"
COPY . .
WORKDIR "/src/src/Aiglusoft.IAM.Server"
RUN dotnet build "./Aiglusoft.IAM.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Aiglusoft.IAM.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aiglusoft.IAM.Server.dll"]