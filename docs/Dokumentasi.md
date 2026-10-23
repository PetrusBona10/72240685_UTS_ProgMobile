# Dokumentasi Sistem Informasi Akademik Sederhana

> Dokumen ini dapat diekspor ke PDF atau DOCX (gunakan Microsoft Word, LibreOffice Writer, atau browser "Save as PDF").

## 1. Deskripsi Aplikasi
Sistem Informasi Akademik Sederhana untuk mengelola data Dosen dan Mahasiswa. Terdiri dari ASP.NET Core Web API dan .NET MAUI Blazor Hybrid.

## 2. Arsitektur
```
Blazor Hybrid (MAUI) -> HTTP/JSON -> ASP.NET Core Web API -> EF Core -> SQLite (academic.db)
```

## 3. Model Data dan Relasi
### Tabel Dosen
- DosenId (PK, int, Identity)
- Nidn (string, unique)
- NamaDosen (string)
- Email (string)
- Prodi (string)

### Tabel Mahasiswa
- MahasiswaId (PK, int, Identity)
- Nim (string, unique)
- NamaMahasiswa (string)
- Email (string)
- Angkatan (int)
- DosenId (FK -> Dosen.DosenId)
- Relasi: Mahasiswa * : 1 Dosen (Dosen Wali)

## 4. Tabel Endpoint
Lihat `README.md` bagian "Tabel Endpoint" untuk daftar lengkap method, endpoint, deskripsi, dan kode respons.

## 5. Screenshot Swagger
Letakkan screenshot berikut di `docs/screenshots/`:
- [ ] swagger-dosen-list.png
- [ ] swagger-mahasiswa-list.png
- [ ] swagger-post-validation.png
- [ ] swagger-delete-conflict.png

## 6. Screenshot Aplikasi
Letakkan screenshot berikut di `docs/screenshots/`:
- [ ] app-beranda.png
- [ ] app-dosen-list.png
- [ ] app-dosen-form.png
- [ ] app-mahasiswa-list.png
- [ ] app-mahasiswa-form.png
- [ ] app-konfirmasi-hapus.png
- [ ] app-toast-success.png
- [ ] app-error-offline.png
- [ ] app-error-online-retry.png

## 7. Hasil Pengujian
| No | Skenario | Hasil |
|---|---|---|
| T01 | Tampilkan daftar dosen/mahasiswa dari API | Berhasil |
| T02 | Tambah dosen/mahasiswa (valid) | Berhasil |
| T03 | Validasi form kosong/salah format | Berhasil |
| T04 | Edit dosen/mahasiswa | Berhasil |
| T05 | Hapus dengan konfirmasi (hapus berhasil) | Berhasil |
| T06 | Error 404/409/400 dari API ditampilkan dengan pesan Indonesia | Berhasil |
| T07 | Navigasi bottom nav & routing halaman | Berhasil |
| T08 | Tampilan responsif & desain aplikasi | Berhasil |
| T09 | Offline/online retry | Berhasil |

## 8. Catatan
- Swagger hanya aktif di Development
- Migration diterapkan otomatis saat API pertama dijalankan
- Android emulator menggunakan `10.0.2.2`, Windows menggunakan `localhost`
- Tidak ada password atau secret di repo
- AI assistant: opencode, repo referensi: https://github.com/ekurniawan/PizzaMobileApp
