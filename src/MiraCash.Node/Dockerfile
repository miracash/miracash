FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MiraCash.Node/MiraCash.Node.csproj", "src/MiraCash.Node/"]
RUN dotnet restore "src/MiraCash.Node/MiraCash.Node.csproj"
COPY . .
WORKDIR "/src/src/MiraCash.Node"
RUN dotnet build "MiraCash.Node.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MiraCash.Node.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiraCash.Node.dll"]
