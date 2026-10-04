# =========================
#  Backend - Multi-stage
# =========================

# 1) Build aÅŸamasÄ± (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Ã–nce csproj kopyalayÄ±p restore et (cache dostu)
COPY WeatherApi.csproj .
RUN dotnet restore

# Kaynak kodu kopyalayÄ±p yayÄ±nla
COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# 2) Runtime aÅŸamasÄ± (kÃ¼Ã§Ã¼k imaj)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Non-root kullanÄ±cÄ± (gÃ¼venlik)
RUN useradd -m -u 5001 appuser
USER appuser

COPY --from=build /app/publish .

# Healthcheck
HEALTHCHECK --interval=30s --timeout=5s --start-period=15s --retries=3 \
    CMD curl -fsS http://localhost:8080/api/weather/cities || exit 1

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "WeatherApi.dll"]