# Use the official .NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET Core SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EmployeePortal.API/EmployeePortal.API.csproj", "EmployeePortal.API/"]
RUN dotnet restore "EmployeePortal.API/EmployeePortal.API.csproj"
COPY . .
WORKDIR "/src/EmployeePortal.API"
RUN dotnet build "EmployeePortal.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmployeePortal.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeePortal.API.dll"]