FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tests/MiraCash.Test.Blockchain/MiraCash.Test.Blockchain.csproj", "tests/MiraCash.Test.Blockchain/"]
RUN dotnet restore "tests/MiraCash.Test.Blockchain/MiraCash.Test.Blockchain.csproj"
COPY . .
WORKDIR "/src/tests/MiraCash.Test.Blockchain"
RUN dotnet build "MiraCash.Test.Blockchain.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MiraCash.Test.Blockchain.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiraCash.Test.Blockchain.dll"]
