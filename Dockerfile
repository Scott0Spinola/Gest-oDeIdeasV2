FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY "GestãoDeIdeasV2.csproj" ./
RUN dotnet restore "GestãoDeIdeasV2.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "GestãoDeIdeasV2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GestãoDeIdeasV2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GestãoDeIdeasV2.dll"]

