FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5216

ENV ASPNETCORE_URLS=http://+:5216

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Importames/Importames.csproj", "Importames/"]
RUN dotnet restore "Importames/Importames.csproj"
COPY . .
WORKDIR "/src/Importames"
RUN dotnet build "Importames.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Importames.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Importames.dll"]
