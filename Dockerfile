# This docker file uses multi-stage build strategy
# to ensure optimal image build times and sizes
# End result container image requires .NET runtime,
# however creating it requires .NET SDK.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /src

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore lighthouse-api-cloud.csproj
RUN dotnet build "./lighthouse-api-cloud.csproj" -c Debug -o /out

FROM build AS publish
RUN dotnet publish lighthouse-api-cloud.csproj -c Debug -o /out

# Building final image used in running container
FROM base AS final
RUN apt-get update \
    && apt-get install -y unzip procps
WORKDIR /src
COPY --from=publish /out .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet lighthouse-api-cloud.dll
