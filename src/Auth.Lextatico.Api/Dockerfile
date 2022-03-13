FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 9000
EXPOSE 9001

ENV ASPNETCORE_URLS=http://+:9000;https://+:9001

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
# RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
# USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["src/Auth.Lextatico.Api/Auth.Lextatico.Api.csproj", "src/Auth.Lextatico.Api/"]
RUN dotnet restore "src/Auth.Lextatico.Api/Auth.Lextatico.Api.csproj"
COPY . .
WORKDIR "/src/src/Auth.Lextatico.Api"
RUN dotnet build "Auth.Lextatico.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auth.Lextatico.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.Lextatico.Api.dll"]
