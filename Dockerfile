# Build the application
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
RUN mkdir GraphQL
COPY OrderApi.sln ./
COPY GraphQL/GraphQL.csproj ./GraphQL/GraphQL.csproj
COPY GraphQL.Tests/GraphQL.Tests.csproj ./GraphQL.Tests/GraphQL.Tests.csproj
COPY Migration/Migration.csproj ./Migration/Migration.csproj
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish GraphQL -c Release -r linux-x64 -o out
RUN dotnet publish Migration -c Release -r linux-x64 -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

RUN apt-get update && apt-get install -y wait-for-it

COPY --from=build-env /app/out .

CMD ["dotnet", "GraphQL.dll"]

EXPOSE 80 
