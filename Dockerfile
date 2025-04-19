# Stage 1 - Build
# Use the official .NET 9 SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project files to the container
COPY *.csproj ./

# Restore the dependencies
RUN dotnet restore

# Copy the source code into the container
COPY . ./

# Build the application
RUN dotnet publish -c Release -o /app/publish

# Stage 2 - Runtime
# Use the official .NET 9 ASP.NET image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published files from the build stage
COPY --from=build /app/publish .

# Copy the .env file (optional, if you want to pass environment variables)
COPY .env /app/.env

# Expose the port the application will run on
EXPOSE 5172

# Run the application
ENTRYPOINT ["dotnet", "MedicalAppointmentApi.dll"]
