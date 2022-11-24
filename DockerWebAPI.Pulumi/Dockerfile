FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DockerWebAPI.Pulumi/DockerWebAPI.Pulumi.csproj", "DockerWebAPI.Pulumi/"]
RUN dotnet restore "DockerWebAPI.Pulumi/DockerWebAPI.Pulumi.csproj"
COPY . .
WORKDIR "/src/DockerWebAPI.Pulumi"
RUN dotnet build "DockerWebAPI.Pulumi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerWebAPI.Pulumi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerWebAPI.Pulumi.dll"]
