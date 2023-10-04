FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# Copy Solution and Projects to restore as distinct layers
COPY *.sln .
COPY src/Example.API/*.csproj ./src/Example.API/

RUN sleep 600

RUN dotnet restore

# Copy everything else
COPY . .

# Build
WORKDIR /source/src/Example.API
RUN dotnet build -c Release -o /app/build

# Publish
RUN dotnet publish -c release -o /app/publish --no-restore

# Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copy published app
COPY --from=build /app/publish ./

# Configure Entrypoint
ENTRYPOINT [ "dotnet", "Example.API.dll" ]
