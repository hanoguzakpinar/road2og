# ── AŞAMA 1: BUILD ──────────────────────────────────────────────
# .NET 10 SDK imajını "build" adlı aşama olarak başlat
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /app

# Önce yalnızca .csproj dosyasını kopyala; bağımlılıklar değişmediğinde
# Docker bu katmanı önbellekten kullanır, restore tekrar çalışmaz
COPY *.csproj .
RUN dotnet restore

# Kaynak kodun tamamını container'a kopyala
COPY . .

# Uygulamayı Release modunda derle, çıktıyı /app/out'a yayınla
RUN dotnet publish DockerMvc.csproj -c Release -o out


# ── AŞAMA 2: RUNTIME ────────────────────────────────────────────
# ASP.NET Core runtime imajını başlat (Razor Pages için zorunlu)
# dotnet/runtime değil dotnet/aspnet kullanılmalı;
# dotnet/runtime yalnızca temel .NET çalışma zamanını içerir,
# Microsoft.AspNetCore.App framework'ünü içermez.
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

# Build aşamasında üretilen dosyaları bu aşamaya kopyala
COPY --from=build /app/out .

# Tüm ağ arayüzlerinde 4500 portunu dinle
ENV ASPNETCORE_URLS="http://*:4500"

# Container başlatıldığında uygulamayı çalıştır
ENTRYPOINT ["dotnet", "DockerMvc.dll"]