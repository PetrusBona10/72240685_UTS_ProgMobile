# Sistem Informasi Akademik Sederhana

## Deskripsi Aplikasi
Sistem Informasi Akademik Sederhana (Academic System) adalah aplikasi sederhana untuk mengelola data akademik berupa **Dosen** dan **Mahasiswa**. Aplikasi ini terdiri dari:
- **Backend API**: ASP.NET Core Web API 10.0 dengan Entity Framework Core dan SQLite
- **Frontend Mobile/Desktop Hybrid**: .NET MAUI Blazor Hybrid (net10.0-android, net10.0-windows) dengan desain tema **Kampus Senja**

## Arsitektur
```
Blazor Hybrid (MAUI) -> HTTP/JSON -> ASP.NET Core Web API -> EF Core -> SQLite (academic.db)
```

## Struktur Folder
```
PROJECT_UTS_MOBILE/
├── AcademicApi/           # ASP.NET Core Web API
│   ├── Controllers/       # DosenController, MahasiswaController
│   ├── Data/             # AcademicDbContext, DAL, SeedData
│   ├── Migrations/       # EF Core migrations
│   ├── Models/           # Entitas Dosen, Mahasiswa
│   └── Program.cs        # Konfigurasi API, CORS, global error handler, migrasi otomatis
├── AcademicHybridApp/     # .NET MAUI Blazor Hybrid
│   ├── Components/       # Razor components (Pages, Layout, Shared)
│   ├ models/            # DTO/Model & ServiceResult
│   ├── Services/         # ApiConfig, ApiServiceBase, ApiErrorReader, ServiceMessages, Services
│   ├── Platforms/        # Android/Windows platform config
│   └── wwwroot/          # CSS (site.css), index.html
├── docs/                 # Dokumentasi & screenshot
│   ├── Dokumentasi.md
│   ├── screenshots/
│   └── README.md
└── AcademicSystem.slnx   # Solution file
```

## Prasyarat
- [.NET SDK 10](https://dotnet.microsoft.com/download) (versi yang digunakan: 10.0.400)
- .NET MAUI workload: `dotnet workload install maui`
- Android Emulator atau Windows untuk menjalankan aplikasi
- (Opsional) [dotnet-ef](https://learn.microsoft.com/ef/core/cli/dotnet) sebagai global tool:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Konfigurasi

### Connection String
Di `AcademicApi/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "AcademicDb": "Data Source=academic.db"
  }
}
```

File database SQLite `academic.db` berada di root project `AcademicApi/`.

### API Base Address
File `AcademicHybridApp/Services/ApiConfig.cs`:
```csharp
public static string BaseAddress => DeviceInfo.Platform == DevicePlatform.Android
    ? "http://10.0.2.2:5080/"
    : "http://localhost:5080/";
```
- **Android Emulator**: gunakan `http://10.0.2.2:5080/` (alias ke localhost host machine)
- **Windows/Desktop**: gunakan `http://localhost:5080/`
- Untuk mengubah alamat API, ubah nilai di `ApiConfig.cs` sesuai target platform.

## Langkah Menjalankan Berurutan

1. **Jalankan API**
   - Buka solution `AcademicSystem.slnx` di Visual Studio 2022
   - Set `AcademicApi` sebagai startup project atau gunakan **Multiple Startup Projects** (Start untuk `AcademicApi` dan `AcademicHybridApp`)
   - Jalankan dengan profil **http** (mendengarkan di `http://localhost:5080`)
   - API akan menerapkan migration otomatis saat pertama dijalankan (`db.Database.Migrate()` di `Program.cs`)

2. **Buka Swagger**
   - Swagger UI hanya aktif di **environment Development** (`app.Environment.IsDevelopment()`)
   - Akses: [http://localhost:5080/swagger](http://localhost:5080/swagger)
   - OpenAPI tersedia di `/openapi/v1.json`

3. **Jalankan Aplikasi**
   - Jalankan `AcademicHybridApp` (Windows atau Android Emulator)
   - Aplikasi akan terhubung ke API sesuai konfigurasi `ApiConfig`

## Data Uji dari Seed
Saat API dijalankan, `SeedData.Initialize(db)` akan mengisi data awal:
- **Dosen**: 3 data (Dr. Budi Santoso, M.Kom., Rina Wijaya, S.Kom., M.Cs., Agus Prasetyo, S.T., M.T.)
- **Mahasiswa**: 5 data dengan relasi dosen wali

## Tabel Endpoint

| Method | Endpoint | Deskripsi | Kode Respons |
|---|---|---|---|
| GET | `/api/dosen` | Daftar semua dosen | `200 OK` |
| GET | `/api/dosen/{id}` | Detail dosen | `200 OK`, `404 Not Found` `"Dosen tidak ditemukan."` |
| POST | `/api/dosen` | Tambah dosen | `201 Created`, `400 Bad Request`, `409 Conflict` `"NIDN sudah digunakan oleh dosen lain."` |
| PUT | `/api/dosen/{id}` | Ubah dosen | `204 NoContent`, `400 Bad Request`, `404 Not Found` `"Dosen tidak ditemukan."`, `409 Conflict` `"NIDN sudah digunakan oleh dosen lain."` |
| DELETE | `/api/dosen/{id}` | Hapus dosen | `204 NoContent`, `404 Not Found` `"Dosen tidak ditemukan."`, `409 Conflict` `"Dosen tidak dapat dihapus karena masih membimbing mahasiswa."` |
| GET | `/api/mahasiswa` | Daftar semua mahasiswa | `200 OK` |
| GET | `/api/mahasiswa/{id}` | Detail mahasiswa | `200 OK`, `404 Not Found` `"Mahasiswa tidak ditemukan."` |
| POST | `/api/mahasiswa` | Tambah mahasiswa | `201 Created`, `400 Bad Request`, `404 Not Found` `"Dosen wali tidak ditemukan."`, `409 Conflict` `"NIM sudah digunakan oleh mahasiswa lain."` |
| PUT | `/api/mahasiswa/{id}` | Ubah mahasiswa | `204 NoContent`, `400 Bad Request`, `404 Not Found` `"Mahasiswa tidak ditemukan."` / `"Dosen wali tidak ditemukan."`, `409 Conflict` `"NIM sudah digunakan oleh mahasiswa lain."` |
| DELETE | `/api/mahasiswa/{id}` | Hapus mahasiswa | `204 NoContent`, `404 Not Found` `"Mahasiswa tidak ditemukan."` |
| Global 500 | - | Error tidak terduga | `500 Internal Server Error` `{"message": "Terjadi kesalahan pada server. Silakan coba lagi."}` |

## Fitur Bonus yang Dikerjakan
- **Tema "Kampus Senja"** - Desain konsisten dengan CSS variables, tipografi sistem, bottom navigation
- **Shared Components** - `ToastService`/`ToastHost`, `ConfirmDialog`, `LoadingState`, `EmptyState`, `ErrorState`, `InitialsAvatar`
- **Dashboard Beranda** - Statistik dosen/mahasiswa, mahasiswa terbaru, pintasan tambah
- **Penanganan Error Terpusat** - `ApiServiceBase`, `ApiErrorReader`, pesan error Indonesia
- **Validasi Berbasis DataAnnotations** - Validasi client & server dengan pesan Bahasa Indonesia
- **Offline/Online Retry** - `ErrorState` dengan tombol "Coba Lagi" di Beranda/Dosen/Mahasiswa
- **Refactor Duplikasi** - `EntityListPageBase` dan `EntityFormPageBase` untuk list/form CRUD
- **Polish UI/UX** - Hapus template Bootstrap, OpenSans, asset tidak terpakai; perbaiki ikon splash/app icon

## Tabel Skenario Uji T01 sampai T09

| No | Skenario | Hasil |
|---|---|---|
| T01 | Tampilkan daftar dosen/mahasiswa dari API | Berhasil |
| T02 | Tambah dosen/mahasiswa (valid) | Berhasil |
| T03 | Validasi form kosong/salah format | Berhasil |
| T04 | Edit dosen/mahasiswa | Berhasil |
| T05 | Hapus dengan konfirmasi (hapus berhasil) | Berhasil |
| T06 | Error 404/409/400 dari API ditampilkan dengan pesan Indonesia | Berhasil |
| T07 | Navigasi bottom nav & routing halaman | Berhasil |
| T08 | Tampilan responsif & desain Kampus Senja | Berhasil |
| T09 | Offline/online retry - matikan API, buka app, tampil ErrorState + tombol Coba Lagi; hidupkan API, klik Coba Lagi, data muncul | Berhasil |

## Catatan Keamanan
- **Tidak ada password atau secret** yang disimpan di repository ini
- File `.db`, `bin/`, `obj/` tidak di-commit (lihat `.gitignore`)
- Hanya konfigurasi koneksi SQLite lokal dan alamat API publik lokal yang digunakan

## Sumber dan Alat Bantu yang Digunakan
- **AI Assistant**: [opencode](https://github.com/sst/opencode) - digunakan untuk membantu perbaikan kode, refactor, validasi, dan dokumentasi
- **Repo Referensi**: [https://github.com/ekurniawan/PizzaMobileApp](https://github.com/ekurniawan/PizzaMobileApp) - digunakan sebagai acuan struktur proyek MAUI Blazor Hybrid
- **Teknologi**: ASP.NET Core 10, Entity Framework Core 10, SQLite, .NET MAUI 10, Blazor Hybrid
