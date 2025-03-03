# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /app

# Copy the project files
COPY ZPNWebAPIProject/ZPNWebAPIProject/ *.csproj ./

# Restore dependencies
RUN dotnet restore

# Build the backend
RUN dotnet publish -c Release -o out

# Use the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# Set working directory
WORKDIR /app

# Copy the build output
COPY --from=build /app/out .

# Expose port 5000
EXPOSE 5000

# Run the backend
ENTRYPOINT ["dotnet", "ZPNWebAPIProject.dll"]