# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install dockerize
ENV DOCKERIZE_VERSION v0.6.1
RUN apt-get update && apt-get install -y wget && \
    wget https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz && \
    tar -C /usr/local/bin -xzvf dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz && \
    rm dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz

# SDK image for building the code
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["JobPosting.API/JobPosting.API.csproj", "JobPosting.API/"]
COPY ["JobPosting.IoC/JobPosting.Infra.IoC.csproj", "JobPosting.IoC/"]
COPY ["JobPosting.Application/JobPosting.Application.csproj", "JobPosting.Application/"]
COPY ["JobPosting.Infra.Data/JobPosting.Infra.Data.csproj", "JobPosting.Infra.Data/"]
COPY ["JobPosting.Domain/JobPosting.Domain.csproj", "JobPosting.Domain/"]

# Restore dependencies
RUN dotnet restore "JobPosting.API/JobPosting.API.csproj"
COPY . .
WORKDIR "/src/JobPosting.API"
RUN dotnet build "JobPosting.API.csproj" -c Release -o /app/build

FROM base AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dockerize", "-wait", "tcp://db:5432", "-timeout", "30s", "dotnet", "JobPosting.API.dll"]
