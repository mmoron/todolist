FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
# Install the dependencies for Visual Studio Remote Debugger
RUN apt-get update && \
    apt-get install -y --no-install-recommends unzip procps && \
    # Install Visual Studio Remote Debugger
    curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg  
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /webapi/src/kafka-stream-net/core
COPY ["src/kafka-stream-net/core", "."]

WORKDIR /webapi/src/GraphQLApi
COPY ["src/GraphQLApi/GraphQLApi.csproj", "."]
RUN dotnet restore "GraphQLApi.csproj"
COPY ["src/GraphQLApi", "."]
ENTRYPOINT dotnet watch run --no-restore --urls=http://+:5000
# comment line above and uncomment below to turn off hot reloading
# RUN dotnet build "GraphQLApi.csproj" --no-restore -c Debug -o /app/build

# FROM build AS publish
# RUN dotnet publish "GraphQLApi.csproj" --no-restore -c Debug -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .

# ENTRYPOINT ["dotnet", "GraphQLApi.dll"]