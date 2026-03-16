# 🏨 HotelWeb – Otel Yönetim Sistemi

**HotelWeb**, otel operasyonlarını yönetmek amacıyla geliştirilmiş modern bir **.NET ve Blazor tabanlı otel yönetim sistemidir**.

Bu uygulama; müşteri yönetimi, rezervasyon işlemleri, çalışan yönetimi ve housekeeping (temizlik görevleri) gibi otel süreçlerini tek bir platform üzerinden yönetmeyi amaçlamaktadır.

Proje, katmanlı mimari (Layered Architecture) yaklaşımı kullanılarak geliştirilmiş olup servis katmanı, rol tabanlı yetkilendirme ve modern Blazor arayüzü içermektedir.

---

# ✨ Özellikler

## 🔐 Kimlik Doğrulama ve Yetkilendirme
- Rol tabanlı erişim kontrolü
- Admin ve Receptionist rolleri
- Güvenli giriş sistemi

## 👤 Müşteri Yönetimi
- Yeni müşteri ekleme
- Müşteri bilgilerini güncelleme
- Müşteri silme
- Müşteri profili görüntüleme
- Arama ve filtreleme

## 📅 Rezervasyon Yönetimi
- Rezervasyon oluşturma
- Rezervasyon bilgilerini güncelleme
- Rezervasyon durum takibi

## 🧹 Housekeeping Yönetimi
- Temizlik görevleri oluşturma
- Görevleri çalışanlara atama
- Görev durumlarını takip etme
- Oda temizlik süreçlerini yönetme

## 👨‍💼 Çalışan Yönetimi
- Çalışan ekleme, düzenleme ve silme
- Çalışan rolleri atama
- Sistem erişim yetkilerini belirleme

## 📊 Dashboard
- Otel operasyonlarının genel görünümü
- Yönetim modüllerine hızlı erişim

---

# 🛠️ Kullanılan Teknolojiler

- .NET
- Blazor
- C#
- Entity Framework Core
- PostgreSQL
- Radzen Blazor Components
- JavaScript (Toastr bildirimleri)

---

# ⚙️ Kurulum

## 1️⃣ Projeyi klonla

git clone https://github.com/kbravrl/HotelWeb.git 

---

## 2️⃣ Veritabanını yapılandır

appsettings.json dosyasında connection string ayarlayın:

"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=hotel_db;Username=postgres;Password=yourpassword"
}

---

## 3️⃣ Migration çalıştır

dotnet ef database update

---

## 4️⃣ Uygulamayı çalıştır

dotnet run
