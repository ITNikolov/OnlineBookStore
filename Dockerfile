################################################################
# STAGE 1: Build and publish the app
################################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project file and restore dependencies
COPY ["OnlineBookStore.csproj", "./"]
RUN dotnet restore "OnlineBookStore.csproj"

# Copy the rest of your source code and publish to /app/publish
COPY . .
RUN dotnet publish "OnlineBookStore.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

################################################################
# STAGE 2: Create the runtime image
################################################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published outputs from the build stage
COPY --from=build /app/publish .

# Expose port 80 so Render (or any host) can route HTTP traffic
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "OnlineBookStore.dll"]
