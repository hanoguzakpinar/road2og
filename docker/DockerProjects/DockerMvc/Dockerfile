# Base image olarak .NET 10 SDK'yı kullan (derleme ve yayınlama araçlarını içerir)
FROM mcr.microsoft.com/dotnet/sdk:10.0

# Container içinde çalışma dizinini /app olarak ayarla (yoksa oluşturur)
WORKDIR /app

# Host makinedeki tüm proje dosyalarını container'ın /app dizinine kopyala
COPY . .

# Proje bağımlılıklarını (NuGet paketleri) indir ve yükle
RUN dotnet restore

# Uygulamayı Release modunda derleyip /app/out klasörüne yayınla
RUN dotnet publish DockerMvc.csproj -c Release -o out

# Çalışma dizinini yayınlanan çıktı klasörüne (/app/out) geçir
WORKDIR /app/out

# Uygulamanın dinleyeceği URL ve portu ortam değişkeni ile belirt
ENV ASPNETCORE_URLS="http://*:4500"

# Container başladığında çalıştırılacak komut: uygulamayı dotnet ile başlat
ENTRYPOINT ["dotnet", "DockerMvc.dll"]
