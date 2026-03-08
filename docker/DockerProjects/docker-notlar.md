# Docker - ASP.NET Core Port Notları

> 📅 Tarih: 8 Mart 2026

---

## ❌ Sorun

`docker run -p 5000:80 aspcoremvc:v1` komutu şu hatayı verdi:

```
ports are not available: address already in use
```

- **5000 portu** başka bir işlem tarafından kullanılıyordu.
- Port çakışması nedeniyle container hiç başlamadı.

---

## ✅ Çözüm

- **.NET 8+** ile birlikte ASP.NET Core uygulamaları varsayılan olarak **8080** portunu dinliyor.
- `Dockerfile`'da `EXPOSE 80` yazsa bile uygulama container içinde **8080**'de çalışıyor.
- Doğru port mapping: `-p 5001:8080`

---

## 🚀 Doğru Komut

```sh
docker run -p 5001:8080 aspcoremvc:v1
```

---

## 🌐 Tarayıcıdan Erişim

```
http://localhost:5001
```

---

## 📝 Özet

| Kavram | Açıklama |
|---|---|
| Format | `-p <host_port>:<container_port>` |
| container_port (.NET 10) | `8080` |
| host_port | İstediğiniz boş port (5001, 8081 vb.) |

---

## 🛠 Yardımcı Komutlar

```sh
# Çalışan container'ları listele
docker ps

# Container'ı durdur
docker stop <container_id>

# Belirli bir portu kullanan işlemi bul
lsof -i :5000
```
