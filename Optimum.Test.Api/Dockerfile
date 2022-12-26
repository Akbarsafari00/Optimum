FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Optimum.Test.Api/Optimum.Test.Api.csproj", "Optimum.Test.Api/"]
RUN dotnet restore "Optimum.Test.Api/Optimum.Test.Api.csproj"
COPY . .
WORKDIR "/src/Optimum.Test.Api"
RUN dotnet build "Optimum.Test.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Optimum.Test.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Optimum.Test.Api.dll"]
