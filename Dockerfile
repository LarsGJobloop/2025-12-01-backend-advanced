# =======================================
# = Common Dockerfile for .NET services =
# =======================================

# Example Usage (replace SERVICE_NAME with the name of the service/directory you want to build):
# docker build --build-arg SERVICE_NAME=AssetManagementService --tag asset-management-service .
# docker run -p 8080:8080 asset-management-service



# ===============
# = Build stage =
# ===============

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Build argument for service name
ARG SERVICE_NAME

# Copy solution file
COPY *.slnx ./

# Copy all project files for dependency resolution
COPY src/ ./src/

# Restore dependencies
RUN dotnet restore src/${SERVICE_NAME}/*.csproj

# Build and publish the specific service
WORKDIR /app/src/${SERVICE_NAME}
RUN dotnet publish --configuration Release --output /app/publish /p:UseAppHost=false



# =================
# = Runtime stage =
# =================

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Set Ports kestrel listens on
ENV HTTP_PORTS=8080
EXPOSE 8080

# Build argument for service name (needed for entrypoint)
ARG SERVICE_NAME
# Convert ARG to ENV so it's available at runtime
ENV SERVICE_NAME=${SERVICE_NAME}

COPY --from=build /app/publish .

# When the container starts, run this command
ENTRYPOINT ["sh", "-c", "dotnet ${SERVICE_NAME}.dll"]
