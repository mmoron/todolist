FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["GraphQLApi.csproj", "."]
RUN dotnet restore "GraphQLApi.csproj"
COPY . .
RUN dotnet build "GraphQLApi.csproj" --no-restore -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "GraphQLApi.csproj" --no-restore -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# TODO: should this be added before code to cache it in docker layer?
# Install the dependencies for Visual Studio Remote Debugger
RUN apt-get update && apt-get install -y --no-install-recommends unzip procps

# Install Visual Studio Remote Debugger
RUN curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg  

ENTRYPOINT ["dotnet", "GraphQLApi.dll"]