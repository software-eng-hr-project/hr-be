FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/ProjectHr.Web.Host/ProjectHr.Web.Host.csproj", "src/ProjectHr.Web.Host/"]
COPY ["src/ProjectHr.Web.Core/ProjectHr.Web.Core.csproj", "src/ProjectHr.Web.Core/"]
COPY ["src/ProjectHr.Application/ProjectHr.Application.csproj", "src/ProjectHr.Application/"]
COPY ["src/ProjectHr.Core/ProjectHr.Core.csproj", "src/ProjectHr.Core/"]
COPY ["src/ProjectHr.EntityFrameworkCore/ProjectHr.EntityFrameworkCore.csproj", "src/ProjectHr.EntityFrameworkCore/"]
WORKDIR "/src/src/ProjectHr.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/ProjectHr.Web.Host", "src/ProjectHr.Web.Host"]
COPY ["src/ProjectHr.Web.Core", "src/ProjectHr.Web.Core"]
COPY ["src/ProjectHr.Application", "src/ProjectHr.Application"]
COPY ["src/ProjectHr.Core", "src/ProjectHr.Core"]
COPY ["src/ProjectHr.EntityFrameworkCore", "src/ProjectHr.EntityFrameworkCore"]
WORKDIR "/src/src/ProjectHr.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "ProjectHr.Web.Host.dll"]
