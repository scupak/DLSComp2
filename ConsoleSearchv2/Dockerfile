FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ConsoleSearchv2/ConsoleSearchv2.csproj", "ConsoleSearchv2/"]
RUN dotnet restore "ConsoleSearchv2/ConsoleSearchv2.csproj"
COPY . .
WORKDIR "/src/ConsoleSearchv2"
RUN dotnet build "ConsoleSearchv2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleSearchv2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleSearchv2.dll"]
