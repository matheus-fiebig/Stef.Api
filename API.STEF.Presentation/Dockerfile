# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API.STEF.Presentation/API.STEF.Presentation.csproj", "API.STEF.Presentation/"]
COPY ["API.STEF.Application/API.STEF.Application.csproj", "API.STEF.Application/"]
COPY ["API.STEF.Domain/API.STEF.Domain.csproj", "API.STEF.Domain/"]
COPY ["API.STEF.Data/API.STEF.Data.csproj", "API.STEF.Data/"]
COPY ["API.STEF.Ioc/API.STEF.Ioc.csproj", "API.STEF.Ioc/"]
RUN dotnet restore "./API.STEF.Presentation/API.STEF.Presentation.csproj"
COPY . .
WORKDIR "/src/API.STEF.Presentation"
RUN dotnet build "./API.STEF.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.STEF.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.STEF.Presentation.dll"]