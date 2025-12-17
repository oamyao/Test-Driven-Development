FROM mcr.microsoft.com/dotnet/sdk:9.0.302 AS build
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release

RUN dotnet test ApiProjectTests/ApiProjectTests.csproj

RUN dotnet publish -o out ApiProject/ApiProject.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "ApiProject.dll"]