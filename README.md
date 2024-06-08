# Banking Software

# [Requirement Documentation](https://drive.google.com/drive/folders/1yjGdrrZCC8MbEl-sCysheDCFVfe0Fe8_?usp=sharing)

# [ER Diagram](https://github.com/Banking-Software/Backend/blob/main/README/E-commerce%20ER-Diagram.pdf)

# Docker

> Create Docker file of backend project

```docker
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
```

> Run SQL Server in docker container
```bash
$ docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=banking@123' -p 1433:1433 --name sql_server_container -d mcr.microsoft.com/mssql/server:2022-latest
```