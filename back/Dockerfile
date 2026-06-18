FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY av360web.slnx .
COPY Directory.Packages.props .

COPY API/API.csproj API/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Persistence/Persistence.csproj Persistence/

RUN dotnet restore API/API.csproj

COPY . .

RUN dotnet publish API/API.csproj \
    -c Release \
    -o /app/publish 

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]
