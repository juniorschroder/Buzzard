FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Buzzard.DemoApi/Buzzard.DemoApi.csproj", "Buzzard.DemoApi/"]
RUN dotnet restore "Buzzard.DemoApi/Buzzard.DemoApi.csproj"
COPY . .
WORKDIR "/src/Buzzard.DemoApi"
RUN dotnet build "Buzzard.DemoApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Buzzard.DemoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Buzzard.DemoApi.dll"]
