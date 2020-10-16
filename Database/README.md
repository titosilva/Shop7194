# How to deploy msssql server?
To deploy MSSql server, an option is to use docker. There is a docker image that Microsoft provides and is easy to use. To use it, download the image and use the file mssql-docker-setting as --env-file. Forward the port 1433 of your computer to port 1433 of the container using -p 1433:1433. 

$ docker run --env-file mssql-docker-settings -p 1433:1433 -d mcr.microsoft.com/mssql/server

# How to build the database tables from my code?
## Connection Strings
First of all, remember to adjust your connection strings (in appsettings) to your needs. For a localhost deployment, we may use 

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "connectionString": "server=localhost,1433;database=shop;User ID=SA;Password=abcdef123_"
  }
}

as our appsettings.json. Notice the ConnectionStrings in this json. These connection strings are used by the Startup.cs code to define how to connect to the DB.

## Installing tools and packages
For build our DB, we need to use the tool dotnet-ef. This tool can be installed using

$ dotnet tool install --global dotnet-ef

This tool is used to do Migrations. Every time we change our model, we just need to create a new migration. To use the model we've created on code to build our DB, we need the package EF Design too. It can be installed using

$ dotnet add package Microsoft.EntityFrameworkCore.Design

## Create a migration
$ dotnet ef migrations add InitialCreate
## Install migrations on DB
$ dotnet ef database update