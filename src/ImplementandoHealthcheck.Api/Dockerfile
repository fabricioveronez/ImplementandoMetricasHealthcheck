FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
CMD apt-get update && apt-get install ps 
CMD apt-get update && apt-get install kill 
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ImplementandoHealthcheck.Api/ImplementandoHealthcheck.Api.csproj ImplementandoHealthcheck.Api/
RUN dotnet restore ImplementandoHealthcheck.Api/ImplementandoHealthcheck.Api.csproj
COPY . .
WORKDIR /src/ImplementandoHealthcheck.Api
RUN dotnet build ImplementandoHealthcheck.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ImplementandoHealthcheck.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
HEALTHCHECK --interval=2m --timeout=3s CMD curl -f http://localhost/health || exit 1
ENTRYPOINT ["dotnet", "ImplementandoHealthcheck.Api.dll"]
