# Kiosk xavfsizligi — tekshiruv ro'yxati

> **8-bosqich faqat shu ro'yxatdagi hamma band belgilangandan keyin tugagan hisoblanadi.**
>
> Maqsad: mijoz uchun kioskdan chiqish yo'li **nol** bo'lsin. Windows'ga o'tishni faqat admin
> qila olsin (`CLAUDE.md` 10.6 — texnik rejim).
>
> Tekshiruv **faqat test client kompyuterda** o'tkaziladi, admin kompyuterda emas.
> Boshlashdan oldin `scripts/dev/rollback-kiosk.ps1` yozilgan va sinalgan bo'lishi shart.

**Qanday ishlatiladi:** har bir qatordagi amalni `Mijoz` foydalanuvchisi sifatida bajaring va
natijani "Kutilgan natija" bilan solishtiring. Mos kelsa — `[x]`, kelmasa — muammoni yozib qo'ying.

---

## 1. Klaviatura va oyna

| # | Nima qilinadi | Kutilgan natija | Holat |
|---|---|---|---|
| 1.1 | `Alt+F4` (Bo'sh ekranda) | Hech narsa bo'lmaydi | [ ] |
| 1.2 | `Alt+F4` (Faol ekranda) | Hech narsa bo'lmaydi | [ ] |
| 1.3 | `Alt+Tab` | Boshqa oynaga o'tib bo'lmaydi | [ ] |
| 1.4 | `Win`, `Win+R`, `Win+E`, `Win+D` | Hech narsa ochilmaydi | [ ] |
| 1.5 | `Ctrl+Esc` | Start menu ochilmaydi | [ ] |
| 1.6 | `Ctrl+Shift+Esc` | Task Manager ochilmaydi | [ ] |
| 1.7 | `Ctrl+Alt+Del` → menyu | Faqat "Bekor qilish" foydali; Task Manager, Qulflash, Chiqish yo'q | [ ] |
| 1.8 | Kiosk oynasini sichqoncha bilan surish / o'lchamini o'zgartirish | Mumkin emas, oyna to'liq ekranda qotgan | [ ] |

## 2. Jarayonni o'ldirish va qo'riqchi

| # | Nima qilinadi | Kutilgan natija | Holat |
|---|---|---|---|
| 2.1 | Administrator hisobidan `ClientApp` UI jarayonini o'ldirish | Shell-qo'riqchi uni bir necha soniyada qayta ochadi | [ ] |
| 2.2 | Qo'riqchi (`--watchdog`) jarayonini o'ldirish | Task Scheduler vazifasi 1 daqiqa ichida qaytaradi | [ ] |
| 2.3 | Ikkalasini birdan o'ldirish | 1 daqiqa ichida kiosk butunlay tiklanadi, qora ekran qolmaydi | [ ] |
| 2.4 | `Mijoz` nomidan Task Scheduler vazifasini o'chirishga urinish | Ruxsat berilmaydi (administrator huquqi so'raladi) | [ ] |
| 2.5 | Kompyuterni qayta yuklash | Avtologin ishlaydi, kiosk o'zi ochiladi, desktop ko'rinmaydi | [ ] |

## 3. Ruxsat etilgan dasturlar orqali qochish

| # | Nima qilinadi | Kutilgan natija | Holat |
|---|---|---|---|
| 3.1 | Brauzerda `Ctrl+O` | Fayl oynasi ochilmaydi yoki `file://` bloklangani haqida xabar | [ ] |
| 3.2 | Manzil qatoriga `file:///C:/` yozish | Bloklangan | [ ] |
| 3.3 | Brauzerda `F12` / Developer Tools | Ochilmaydi | [ ] |
| 3.4 | Biror faylni yuklab olish | Yuklab olish o'chirilgan | [ ] |
| 3.5 | Brauzer sozlamalaridan kengaytma o'rnatish | Taqiqlangan | [ ] |
| 3.6 | Fayl oynasi orqali `cmd.exe` yoki `powershell.exe` ishga tushirishga urinish | SRP bloklaydi | [ ] |
| 3.7 | Steam ichidagi brauzer orqali fayl tizimiga chiqishga urinish | Fayl tizimiga chiqib bo'lmaydi | [ ] |
| 3.8 | Ruxsat etilmagan dasturni biror yo'l bilan ochishga urinish | Ishga tushmaydi | [ ] |

## 4. Texnik rejim (faqat admin)

| # | Nima qilinadi | Kutilgan natija | Holat |
|---|---|---|---|
| 4.1 | Maxfiy kombinatsiya (`Ctrl+Shift+Alt+F12`) → **noto'g'ri** parol | Chiqmaydi, xato xabari | [ ] |
| 4.2 | Maxfiy kombinatsiya → **to'g'ri** admin paroli | Kiosk yopiladi, `explorer.exe` ochiladi | [ ] |
| 4.3 | Texnik rejimda 2 daqiqa kutish | Qo'riqchi kioskni **qayta ochmaydi** (belgi amal qilmoqda) | [ ] |
| 4.4 | "Kioskka qaytish" tugmasi | Kiosk darhol qaytadi | [ ] |
| 4.5 | Texnik rejimda muddatni kutish (`MaintenanceMinutes`) | Kiosk o'zi qaytadi | [ ] |
| 4.6 | Texnik rejimda kompyuterni qayta yuklash | Kiosk qaytadi (belgi qayta yuklashdan keyin kuchga ega emas) | [ ] |
| 4.7 | Admin paneldagi "Windows'ga chiqarish" tugmasi | Masofadan texnik rejimga o'tadi | [ ] |
| 4.8 | Admin paneldagi "Kioskka qaytarish" tugmasi | Masofadan kiosk qaytadi | [ ] |
| 4.9 | AuditLog'ni tekshirish | Texnik rejimga kirish va chiqish yozilgan | [ ] |

## 5. Tarmoq va server

| # | Nima qilinadi | Kutilgan natija | Holat |
|---|---|---|---|
| 5.1 | Kiosk brauzeridan `http://<admin-IP>:5000` ni ochish | 403 — admin panel ochilmaydi | [ ] |
| 5.2 | Faol sessiya paytida tarmoq kabelini sug'urish | Kiosk Faol holatda qoladi, vaqtni o'zi sanaydi, "aloqa yo'q" belgisi chiqadi | [ ] |
| 5.3 | Kabelni qayta ulash | Serverdagi haqiqiy vaqt bilan sinxronlanadi | [ ] |
| 5.4 | AdminApp'ni yopib, 2 daqiqadan keyin qayta ochish | Sessiya yo'qolmaydi, qolgan vaqt to'g'ri | [ ] |

## 6. Rollback

| # | Nima qilinadi | Kutilgan natija | Holat |
|---|---|---|---|
| 6.1 | `scripts/dev/rollback-kiosk.ps1` ni ishga tushirish | Shell `explorer.exe` ga qaytadi, cheklovlar olib tashlanadi | [ ] |
| 6.2 | Qayta yuklash | Oddiy Windows desktop ochiladi | [ ] |

---

## Natija

- **Sana:**
- **Test kompyuter:**
- **Windows versiyasi va nashri:**
- **Yopilmagan yo'llar (bo'lsa):**
