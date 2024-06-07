# Use the official .NET 6 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet publish -c Release -o out

# Use the official .NET 6 runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the output from the build image
COPY --from=build /app/out ./

# Expose the port the application will listen on
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "MicroFinance.dll"]