FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY reqnroll_project.csproj ./
RUN dotnet restore reqnroll_project.csproj

# Copy everything else and build
COPY . ./
RUN dotnet build reqnroll_project.csproj -c Release --no-restore

# Default command runs the tests
ENTRYPOINT ["dotnet", "test", "--no-build", "-c", "Release", "--logger:console;verbosity=normal"]