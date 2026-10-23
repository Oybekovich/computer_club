# Kompyuterxona boshqaruv tizimi

> **Claude Code uchun:** bu loyihaning asosiy hujjati. Har bir yangi sessiyada shu faylni to'liq o'qi va shu yerda yozilgan qarorlar hamda qoidalarga amal qil. Agar biror qarorni o'zgartirish kerak bo'lsa, avval men bilan kelish, keyin shu faylni yangila. Har bir bosqich tugagach, eng oxiridagi **"Loyiha holati"** bo'limini yangila.

## Mundarija

1. Loyiha haqida
2. Men haqimda va muloqot qoidalari
3. Umumiy arxitektura
4. Texnologiyalar
5. Papka tuzilishi
6. Ma'lumotlar bazasi
7. Pul va vaqt hisoblash qoidalari
8. Aloqa protokoli
9. Admin dasturi
10. Client (kiosk) dasturi
11. Xavfsizlik
12. Test muhiti (SSH)
13. Ishlash bosqichlari
14. Kod yozish qoidalari
15. Build va publish
16. Deploy (o'rnatish) — keyingi bosqich
17. Ochiq savollar
18. Lug'at
19. Loyiha holati

---

## 1. Loyiha haqida

**Maqsad:** kompyuterxona (o'yin klubi) uchun yengil boshqaruv tizimi. Admin bitta kompyuterdan barcha mijoz kompyuterlarini boshqaradi: pul olib sessiya ochadi (pulni vaqtga aylantiradi), qolgan vaqtni kuzatadi, kompyuterlarni masofadan yoqadi va o'chiradi, har bir kompyuterda mijoz qaysi dasturlardan foydalana olishini tanlaydi.

**Sharoit:**

- Taxminan 15 ta client kompyuter va 1 ta admin kompyuter. Hammasi Windows 10/11 da ishlaydi.
- Hammasi bitta mahalliy tarmoqda (LAN). **Internet yo'q deb hisobla.** Tizim internetga hech qachon murojaat qilmasligi kerak: CDN, tashqi API, onlayn litsenziya tekshiruvi va shunga o'xshash narsalar yo'q.
- Bir martalik tijorat loyihasi, bitta mijozga sotiladi. Kod toza, tushunarli va keyinchalik men o'zim qo'llab-quvvatlay oladigan bo'lishi kerak.
- Loyiha 0 dan yoziladi. Tayyor open-source loyihalardan kod nusxalanmaydi (g'oya sifatida o'rganish mumkin).

**Asosiy talablar:**

- Client kompyuterda oddiy Windows desktop, taskbar va Start menu ko'rinmaydi. Faqat bizning kiosk ekranimiz va admin ruxsat bergan dasturlar ko'rinadi.
- Admin pul summasini kiritadi, tizim uni soatlik narx bo'yicha vaqtga aylantiradi va orqaga sanoq boshlanadi. Sessiya davomida vaqt (pul) qo'shish mumkin.
- Soatlik narxni admin o'zi sozlaydi.
- Admin har bir kompyuter uchun qaysi dasturlar ko'rinishini tanlaydi va istalgan dasturni masofadan ochib bera oladi.
- Wake-on-LAN orqali yoqish, masofadan o'chirish va qayta yuklash.
- Sessiyalar tarixi va hisobotlar.
- Admin panel admin kompyuterida alohida oyna sifatida ochiladi. Shu LAN/Wi-Fi'dagi telefon brauzeridan ham ochish mumkin (login va parol bilan).
- Ikkala dastur ham juda yengil bo'lishi kerak: o'yin o'ynayotgan mijoz ularning borligini sezmasligi kerak.

**Doiradan tashqari (1-versiyada qilinmaydi):** internet orqali boshqarish, mijozlarning shaxsiy hisoblari va balans kartalari, tungi/kunduzgi turli tariflar, bar yoki do'kon savdosi, bir nechta filial. Bularni keyin qo'shish mumkin bo'lishi uchun arxitekturani yopib qo'yma, lekin hozir yozma.

---

## 2. Men haqimda va muloqot qoidalari

- Men frontend (HTML/CSS/JavaScript) o'rganayotgan boshlang'ich dasturchiman. C# va .NET bilan tajribam cheklangan.
- Men bilan **o'zbek tilida** gaplash. Texnik atamani birinchi marta ishlatganingda uni oddiy tilda, iloji bo'lsa hayotiy o'xshatish bilan tushuntir.
- Har bir qadamdan keyin nima qilinganini va **men uni qanday sinab ko'rishimni** yoz: qaysi oynani ochish, qaysi buyruqni yozish, ekranda nimani ko'rishim kerak.
- Xavfli yoki qaytarish qiyin amallardan oldin (Registry, Windows shell, Firewall, Windows foydalanuvchilari, fayllarni o'chirish, tizim sozlamalari) **avval nima qilmoqchi ekaningni tushuntir va ruxsat so'ra.**
- Mendan **hech qachon parol so'rama.** Parolni buyruqlarga, fayllarga yoki loglarga yozma. Parol kerak bo'lsa, men uni o'zim terminalda kiritaman.
- Bir vaqtda juda ko'p narsani o'zgartirma. Kichik qadam, keyin sinov, keyin keyingi qadam.
- Yangi NuGet paket qo'shishdan oldin u nima uchun kerakligini qisqa tushuntir.
- Har bir bosqich tugagach: build, testlar, git commit (o'zbekcha qisqa izoh bilan) va 19-bo'limni yangilash.

---

## 3. Umumiy arxitektura

```
┌──────────────────────────────────────────────┐
│  ADMIN KOMPYUTER (masalan 192.168.1.100)     │
│                                              │
│  AdminApp.exe                                │
│   ├─ Ichki server (ASP.NET Core, port 5000)  │
│   │   ├─ REST API (admin panel)              │
│   │   ├─ SignalR (jonli aloqa)               │
│   │   └─ SQLite baza (data.db)               │
│   └─ Oyna: WebView2 (admin panel)            │
└───────────────┬──────────────────────────────┘
                │  LAN (kabel / Wi-Fi)
    ┌───────────┼───────────┬────────────────────┐
    ▼           ▼           ▼                    ▼
┌───────┐   ┌───────┐   ┌───────┐     ┌────────────────────┐
│ PC-01 │   │ PC-02 │...│ PC-15 │     │ Admin telefoni     │
│ Client│   │ Client│   │ Client│     │ (brauzer + login)  │
└───────┘   └───────┘   └───────┘     └────────────────────┘
```

**Uchta qism:**

| Qism | Vazifasi |
|---|---|
| `Kompyuterxona.Admin` (AdminApp.exe) | Server, baza va admin oynasi — hammasi bitta exe ichida |
| `Kompyuterxona.Client` (ClientApp.exe) | Har bir mijoz kompyuteridagi kiosk ekran |
| `Kompyuterxona.Shared` | Ikkala dastur umumiy ishlatadigan kod: buyruq turlari, DTO'lar, konstantalar |

**Asosiy qoida — "haqiqat manbai" server:**

- Barcha ma'lumotlar (sessiyalar, pul, vaqt, sozlamalar) faqat serverdagi SQLite bazada saqlanadi.
- Client serverdan kelgan buyruqlarni bajaradi va o'z holatini xabar qiladi.
- **Istisno — aloqa uzilsa:** client faol sessiyaning qolgan vaqtini o'zi sanashda davom etadi va vaqt tugaganda o'zi qulflanadi. Aloqa tiklanganda server qiymati ustun bo'ladi.
- Ulanishni **har doim client boshlaydi** (client serverga ulanadi). Shuning uchun client kompyuterlarda kiruvchi port ochish shart emas.

---

## 4. Texnologiyalar

| Soha | Tanlov | Nega |
|---|---|---|
| Til va platforma | C#, eng so'nggi LTS .NET (yozilish paytida .NET 10). Boshlashdan oldin `dotnet --list-sdks` bilan tekshir | Windows bilan chuqur integratsiya, ikkala dasturda bitta texnologiya |
| Admin server | ASP.NET Core (Kestrel), Minimal API | Yengil, exe ichida ichki server sifatida ishlaydi |
| Jonli aloqa | ASP.NET Core SignalR | Server clientga buyruqni darhol yubora oladi; .NET va JS client kutubxonalari bor; uzilsa o'zi qayta ulanadi |
| Baza | SQLite + EF Core (`Microsoft.EntityFrameworkCore.Sqlite`) | Bitta fayl, alohida baza serveri yo'q; sxema migratsiyalar orqali boshqariladi |
| Admin oynasi | WinForms + WebView2 | WinForms faqat oyna va tray uchun; ichida WebView2 admin panelni ko'rsatadi |
| Admin UI | Oddiy HTML + CSS + vanilla JavaScript (ES modullar, `fetch`) | Framework va build bosqichi yo'q; men JS bilaman, tushunish va o'zgartirish oson |
| Client UI | WPF | To'liq ekranli, chiroyli kiosk interfeysi |
| Parol xeshlash | `PasswordHasher<T>` (ASP.NET Core Identity'dan, PBKDF2) | Tayyor va ishonchli |
| Loglar | Serilog (fayl, kunlik aylanish) | Xatolarni keyin o'qib tahlil qilish uchun |
| Testlar | xUnit | Pul va vaqt hisoblash mantiqini tekshirish |

**Muhim:**

- **CDN ishlatma.** SignalR JS kutubxonasi (`signalr.min.js`) va boshqa barcha frontend fayllar `wwwroot/lib/` ichida mahalliy saqlanadi.
- Tashqi NuGet paketlarni minimal saqla.
- Admin loyihasi bitta loyihada ham web server, ham WinForms bo'ladi: `Sdk="Microsoft.NET.Sdk.Web"`, `TargetFramework` = `net10.0-windows`, `UseWindowsForms` = `true`, `OutputType` = `WinExe`.
- WebView2 Runtime Windows 11 da bor, Windows 10 da odatda Edge bilan birga keladi. AdminApp ishga tushganda WebView2 topilmasa, tushunarli xabar chiqarsin. Deploy paketiga WebView2 offline o'rnatuvchisi (Evergreen Standalone) qo'shiladi.

---

## 5. Papka tuzilishi

```
KompyuterxonaTizimi/
├── CLAUDE.md                        ← shu fayl
├── KompyuterxonaTizimi.sln
├── .gitignore
├── src/
│   ├── Kompyuterxona.Shared/        ← umumiy kod
│   │   ├── Commands/                (server → client buyruqlari)
│   │   ├── Dtos/                    (ma'lumot almashish modellari)
│   │   └── Constants.cs             (hub manzillari, standart port)
│   ├── Kompyuterxona.Admin/         ← AdminApp.exe
│   │   ├── Program.cs               (server + oynani ishga tushirish)
│   │   ├── Data/                    (DbContext, Entity'lar, Migrations)
│   │   ├── Services/                (Billing, Session, WakeOnLan, Backup, Settings, ConnectionRegistry)
│   │   ├── Hubs/                    (ClientHub, AdminHub)
│   │   ├── Endpoints/               (REST API guruhlari)
│   │   ├── Auth/                    (login, cookie, birinchi sozlash)
│   │   ├── Desktop/                 (WinForms oyna, WebView2, tray)
│   │   └── wwwroot/                 (index.html, css/, js/, lib/)
│   └── Kompyuterxona.Client/        ← ClientApp.exe
│       ├── App.xaml
│       ├── Views/                   (IdleView, ActiveView, WarningWindow, TechExitDialog)
│       ├── Services/                (ServerConnection, SessionTimer, AppLauncher,
│       │                             ProcessTracker, PowerControl, Watchdog)
│       └── client.config.example.json
├── tests/
│   └── Kompyuterxona.Tests/         (xUnit)
├── scripts/
│   ├── dev/                         (test uchun: SSH orqali client'ga yuborish, rollback)
│   └── deploy/                      (keyinroq: install-admin.ps1, install-client.ps1)
└── docs/
    └── admin-qollanma.md            (keyinroq: admin uchun o'zbekcha qo'llanma)
```

**Ishlash paytidagi ma'lumotlar (repo'da emas):**

| Kompyuter | Dastur fayllari | Ma'lumotlar |
|---|---|---|
| Admin | `C:\Program Files\Kompyuterxona\Admin\` | `C:\ProgramData\Kompyuterxona\` → `data.db`, `logs\`, `backups\` |
| Client | `C:\Program Files\Kompyuterxona\Client\` | `C:\ProgramData\Kompyuterxona\Client\` → `client.config.json`, `logs\` |

Dastur fayllari `Program Files` ichida turadi, shuning uchun oddiy (administrator bo'lmagan) mijoz foydalanuvchisi ularni o'zgartira olmaydi.

---

## 6. Ma'lumotlar bazasi

Fayl: `C:\ProgramData\Kompyuterxona\data.db`. Sxema EF Core migratsiyalari bilan boshqariladi; dastur ishga tushganda `Database.Migrate()` avtomatik chaqiriladi.

### Computers — kompyuterlar

| Ustun | Turi | Izoh |
|---|---|---|
| Id | int, PK | |
| Number | int, unique | 1..15; ekranda "PC-01" |
| Name | text, null | ixtiyoriy nom, masalan "VIP-1" |
| MacAddress | text | Wake-on-LAN uchun; `AA-BB-CC-DD-EE-FF` formatiga keltiriladi |
| IpAddress | text, null | client ulanganda avtomatik yangilanadi |
| IsEnabled | bool | o'chirilgan kompyuter panelda kulrang |
| LastSeenAt | datetime (UTC), null | oxirgi heartbeat |
| Notes | text, null | |

Holat (Oflayn / Bo'sh / Band) bazada saqlanmaydi. U xotirada (`ConnectionRegistry`) hisoblanadi: client ulanganmi va faol sessiya bormi.

### Applications — ruxsat etiladigan dasturlar katalogi

| Ustun | Turi | Izoh |
|---|---|---|
| Id | int, PK | |
| Name | text | "Google Chrome" |
| ExePath | text | Client kompyuterdagi yo'l, masalan `C:\Program Files\Google\Chrome\Application\chrome.exe` |
| Arguments | text, null | |
| WorkingDirectory | text, null | |
| Category | text | "Brauzer", "O'yin", "Ofis", "Boshqa" |
| SortOrder | int | |
| IsActive | bool | |

Ikonka bazada saqlanmaydi: client uni exe faylning o'zidan oladi.

### ComputerApplications — qaysi kompyuterda qaysi dastur ko'rinadi

| Ustun | Turi |
|---|---|
| ComputerId | FK → Computers |
| ApplicationId | FK → Applications |

PK: (ComputerId, ApplicationId).

### Sessions — sessiyalar

| Ustun | Turi | Izoh |
|---|---|---|
| Id | int, PK | |
| ComputerId | FK | |
| StartedAt | datetime (UTC) | |
| EndedAt | datetime (UTC), null | null bo'lsa sessiya faol |
| GrantedSeconds | int | barcha to'lovlar bo'yicha berilgan jami vaqt |
| UsedSeconds | int, null | sessiya yopilganda yoziladi |
| TotalAmount | long | jami to'langan pul, so'mda |
| EndReason | text, null | `TimeUp`, `StoppedByAdmin`, ... |

Bir kompyuterda bir vaqtda faqat bitta faol sessiya bo'lishi mumkin.

### Payments — to'lovlar

| Ustun | Turi | Izoh |
|---|---|---|
| Id | int, PK | |
| SessionId | FK | |
| Amount | long | so'mda |
| SecondsAdded | int | |
| HourlyRateUsed | long | to'lov paytidagi soatlik narx |
| Type | text | `Initial` yoki `TopUp` |
| CreatedAt | datetime (UTC) | |

### Settings — sozlamalar (kalit-qiymat)

| Kalit | Standart qiymat | Izoh |
|---|---|---|
| HourlyRate | 10000 | so'm/soat (vaqtinchalik qiymat, 17-bo'limga qarang) |
| WarningMinutes | 5 | necha daqiqa qolganda ogohlantirish |
| TimeUpAction | Lock | `Lock` (Bo'sh ekranga qaytish) yoki `Shutdown` |
| BackupFolder | `C:\ProgramData\Kompyuterxona\backups` | |
| BackupDailyTime | 03:00 | |
| BackupKeepDays | 30 | |
| ServerPort | 5000 | |
| ClientKeyHash | (avtomatik) | client kaliti xeshi |

### AdminUsers — admin hisoblari

| Ustun | Turi |
|---|---|
| Id | int, PK |
| Username | text, unique |
| PasswordHash | text |
| CreatedAt | datetime (UTC) |

### AuditLog — muhim amallar jurnali

| Ustun | Turi |
|---|---|
| Id | int, PK |
| At | datetime (UTC) |
| Username | text |
| Action | text |
| Details | text |

Narx o'zgarishi, sessiyani erta to'xtatish, kompyuter yoki dastur o'chirilishi kabi amallar yoziladi. Bu egasiga xodimlar ishini nazorat qilishga yordam beradi.

---

## 7. Pul va vaqt hisoblash qoidalari

- Pul har doim **butun son (`long`), so'mda.** `double` yoki `float` ishlatma.
- Vaqt soniyalarda (`int`).
- Summadan vaqtga: `soniya = floor(summa × 3600 / soatlik_narx)`.
  - Narx 10 000 so'm/soat, summa 15 000 → 5400 s = 1 soat 30 daqiqa.
  - Narx 12 000 so'm/soat, summa 5 000 → 1500 s = 25 daqiqa.
- Admin vaqt kiritishni ham tanlashi mumkin. Vaqtdan summaga: `summa = ceil(soniya × soatlik_narx / 3600)`.
- Har bir to'lov o'sha paytdagi narx bilan hisoblanadi va `Payments.HourlyRateUsed` ga yoziladi. Narx o'zgarsa, eski to'lovlar o'zgarmaydi.
- Sessiya tugash vaqti: `EndsAt = StartedAt + GrantedSeconds` (server soati, UTC). Vaqt qo'shilsa `GrantedSeconds` oshadi.
- Server qayta ishga tushsa, faol sessiyalar `EndsAt` bo'yicha davom etadi. `EndsAt` o'tib ketgan sessiyalar `TimeUp` sababi bilan yopiladi.
- Admin sessiyani erta to'xtatsa: `UsedSeconds` yoziladi, pul avtomatik qaytarilmaydi (17-bo'limdagi ochiq savol).
- Clientga absolyut vaqt emas, **qolgan soniyalar** yuboriladi, chunki kompyuterlarning soatlari farq qilishi mumkin. Client bu qiymatni `Stopwatch` (monoton soat) bilan sanaydi.
- Bu mantiq `Services/BillingService` ichida, UI'dan alohida turadi va **xUnit testlari bilan** qoplanadi (chegaraviy holatlar: 0 summa, juda kichik summa, narx o'zgarishi, vaqt qo'shish).

---

## 8. Aloqa protokoli

### 8.1 Client ↔ Server (SignalR, `/hubs/client`)

**Ulanish va autentifikatsiya:** client `ServerUrl` ga ulanadi va `Register` ni chaqiradi. U `ClientKey` (umumiy maxfiy kalit) yuboradi. Kalit noto'g'ri bo'lsa, server ulanishni uzadi. Kalit to'g'ri, lekin kompyuter raqami bazada yo'q bo'lsa, admin panelda "Yangi kompyuter topildi" deb ko'rsatiladi; admin uni bitta tugma bilan qo'shadi (MAC va IP avtomatik to'ldiriladi).

**Client → Server:**

| Metod | Ma'lumot | Qachon |
|---|---|---|
| `Register` | ComputerNumber, ClientKey, MachineName, MacAddress, IpAddress, AppVersion | Ulanganda va qayta ulanganda |
| `Heartbeat` | State (Idle/Active), RemainingSeconds, RunningAppIds[] | Har 15 soniyada |
| `ReportAppLaunched` | AppId, Success, Error | Dastur ochishga urinilgandan keyin |
| `ReportError` | Message | Jiddiy xatolikda |
| `VerifyTechPassword` | Password | Maxfiy chiqish oynasida (10.3) |

**Server → Client:**

| Buyruq | Ma'lumot | Client nima qiladi |
|---|---|---|
| `SessionStarted` | RemainingSeconds, AllowedApps[] | Faol ekranga o'tadi |
| `TimeUpdated` | RemainingSeconds | Taymerni yangilaydi (vaqt qo'shilganda) |
| `SessionEnded` | Reason | Dasturlarni yopadi, Bo'sh ekranga qaytadi (yoki sozlamaga ko'ra o'chadi) |
| `AllowedAppsChanged` | AllowedApps[] | Dastur plitkalarini yangilaydi |
| `LaunchApp` | AppId | Dasturni ochadi |
| `ShowMessage` | Text | Ekranda xabar ko'rsatadi |
| `Shutdown` | | Kompyuterni o'chiradi |
| `Restart` | | Kompyuterni qayta yuklaydi |

`AllowedApps` elementi: `{ Id, Name, ExePath, Arguments, WorkingDirectory, Category }`.

**Qayta ulanish:** `WithAutomaticReconnect` cheksiz urinishlar bilan (5–30 soniya oralig'ida). Qayta ulanganda client yana `Register` chaqiradi, server esa joriy holatni qayta yuboradi (sessiya bo'lsa `SessionStarted`).

**Wake-on-LAN** SignalR orqali emas. O'chiq kompyuter tarmoqqa ulanmagan bo'ladi, shuning uchun server UDP "magic packet" yuboradi: 6 bayt `0xFF` va undan keyin MAC manzil 16 marta. Paket `255.255.255.255` va subnet broadcast manziliga, 9-portga yuboriladi. Buning uchun tashqi kutubxona kerak emas.

### 8.2 Admin UI ↔ Server

- REST API (`/api/...`), JSON, cookie orqali autentifikatsiya.
- Jonli yangilanishlar SignalR `/hubs/admin` orqali (faqat login qilgan admin ulanadi). Hodisalar: `ComputerStatusChanged`, `SessionChanged`, `NewComputerDetected`.

**REST endpointlar:**

```
POST   /api/setup                          birinchi admin hisobini yaratish (faqat localhost'dan, faqat bir marta)
POST   /api/auth/login                     {username, password}
POST   /api/auth/logout
POST   /api/auth/change-password

GET    /api/computers                      holati bilan birga
POST   /api/computers
PUT    /api/computers/{id}
DELETE /api/computers/{id}

POST   /api/computers/{id}/session         {amount} yoki {minutes}
POST   /api/computers/{id}/session/topup   {amount} yoki {minutes}
POST   /api/computers/{id}/session/stop
POST   /api/computers/{id}/launch          {appId}
POST   /api/computers/{id}/message         {text}
POST   /api/computers/{id}/wake
POST   /api/computers/{id}/shutdown
POST   /api/computers/{id}/restart
POST   /api/computers/wake-all
POST   /api/computers/shutdown-idle        faqat bo'sh kompyuterlarni o'chirish

GET    /api/apps
POST   /api/apps
PUT    /api/apps/{id}
DELETE /api/apps/{id}
GET    /api/computers/{id}/apps
PUT    /api/computers/{id}/apps            {appIds: [...]}
POST   /api/apps/{id}/assign-all

GET    /api/settings
PUT    /api/settings
POST   /api/settings/regenerate-client-key

GET    /api/reports/sessions?from=&to=&computerId=
GET    /api/reports/summary?from=&to=
GET    /api/reports/sessions.csv?from=&to=

POST   /api/backup                         hozir zaxiralash
GET    /api/backups
```

---

## 9. Admin dasturi

### 9.1 Ishga tushish va oyna

- AdminApp.exe ochilganda: (1) server fon thread'da ishga tushadi va `0.0.0.0:5000` da tinglaydi (LAN'dan ham ochiladi), (2) WinForms oynasi WebView2 ichida `http://localhost:5000` ni ochadi.
- Bir vaqtda faqat bitta nusxa ishlaydi (Mutex). Ikkinchi marta ochilsa, mavjud oyna oldinga chiqadi.
- **Oynani yopish dasturdan chiqish degani emas.** X bosilsa, oyna tray'ga (soat yonidagi belgi) yashirinadi, server esa ishlashda davom etadi. Aks holda barcha kompyuterlar server bilan aloqani yo'qotadi. To'liq chiqish faqat tray menyusidagi "Chiqish" orqali va tasdiq bilan: "Faol sessiyalar bor. Rostdan chiqasizmi?"
- Sozlamalarda "Windows bilan birga ishga tushirish" (Task Scheduler, foydalanuvchi kirganda ishga tushadigan vazifa).

### 9.2 Birinchi ishga tushish

- Baza yo'q bo'lsa yaratiladi va standart sozlamalar yoziladi.
- Admin hisobi yo'q bo'lsa, "Birinchi sozlash" ekrani ochiladi: login va parol yaratiladi. Standart (oldindan ma'lum) parol ishlatilmaydi. Bu ekran faqat admin kompyuterining o'zidan (localhost) ochiladi.
- Client kaliti avtomatik yaratiladi va sozlamalarda bir marta ko'rsatiladi (client o'rnatishda kerak bo'ladi).

### 9.3 Ekranlar

1. **Login.**
2. **Bosh panel** — kompyuterlar kartalar to'rida:
   - Har bir kartada: raqam va nom, holat rangi (kulrang — oflayn, yashil — bo'sh, ko'k — band, qizil — ogohlantirish vaqti qolgan), qolgan vaqt (brauzerda har soniyada sanaladi), ochiq dastur nomi.
   - Karta tugmalari: Sessiya boshlash, Vaqt qo'shish, To'xtatish, Dastur ochish, Xabar yuborish, Yoqish, O'chirish, Qayta yuklash.
   - Yuqori qatorda: bugungi tushum, band kompyuterlar soni, "Hammasini yoqish" tugmasi.
   - Xavfli amallar (to'xtatish, o'chirish) tasdiq so'raydi.
3. **Sessiya boshlash / vaqt qo'shish oynasi** — summa yoki vaqt kiritish, jonli hisob ("15 000 so'm = 1 soat 30 daqiqa"), tez tugmalar (masalan 5 000 / 10 000 / 20 000).
4. **Dasturlar:**
   - Katalog: qo'shish va tahrirlash (nom, exe yo'li, argumentlar, kategoriya).
   - Kompyuter × dastur jadvali: katakchalarni belgilab, qaysi kompyuterda nima ko'rinishini tanlash; "Barchaga qo'llash" tugmasi.
   - Eslatma UI'da ko'rinsin: exe yo'li **client kompyuterlarda** mavjud bo'lishi kerak. Client dasturni topa olmasa, plitkani kulrang qiladi va serverga xabar beradi; admin panelda ogohlantirish chiqadi.
5. **Kompyuterlar** — qo'shish va tahrirlash (raqam, nom, MAC). "Yangi kompyuter topildi" ro'yxatidan bir bosishda qo'shish.
6. **Hisobotlar** — sana oralig'i bo'yicha sessiyalar jadvali (kompyuter, boshlanish, tugash, davomiylik, summa, tugash sababi), jami summa va jami soatlar, kompyuterlar bo'yicha guruhlash, CSV eksport (Excel'da ochish uchun).
7. **Sozlamalar** — soatlik narx, ogohlantirish daqiqasi, vaqt tugaganda amal, zaxira papkasi va vaqti, Windows bilan ishga tushirish, client kaliti, parolni almashtirish, amallar jurnali (AuditLog).

### 9.4 Telefon orqali kirish

- UI **responsive** bo'lsin: telefonda kartalar 1–2 ustunda, tugmalar barmoq bilan bosishga qulay.
- Telefon xuddi shu tarmoqda bo'lsa, `http://<admin-IP>:5000` orqali ochiladi. Login majburiy.

### 9.5 Zaxira (backup)

- Ishlab turgan SQLite bazani xavfsiz nusxalash uchun `SqliteConnection.BackupDatabase` (yoki `VACUUM INTO`) ishlatiladi, oddiy fayl nusxalash emas.
- Har kuni belgilangan vaqtda avtomatik va qo'lda tugma orqali. Fayl nomi: `data-2026-10-23_0300.db`. `BackupKeepDays` kundan eskilari o'chiriladi.
- Zaxiradan tiklash hozircha qo'llanmada qo'lda bajariladigan yo'riq sifatida yoziladi (dasturni yopish va faylni almashtirish).

---

## 10. Client (kiosk) dasturi

### 10.1 Holatlar

```
[Ulanmagan] ──server topildi──▶ [Bo'sh] ──SessionStarted──▶ [Faol]
                                   ▲                          │
                                   │                          │ N daqiqa qoldi
                                   │                          ▼
                                   └──── vaqt tugadi ──[Ogohlantirish]
```

Admin sessiyani to'xtatsa, Faol holatdan to'g'ridan-to'g'ri Bo'sh holatga o'tiladi.

- **Ulanmagan:** "Server bilan aloqa yo'q. Qayta ulanilmoqda..." va kompyuter raqami. Mijoz hech narsa ocha olmaydi. Faol sessiya paytida aloqa uzilsa, client Faol holatda qoladi va vaqtni o'zi sanaydi; ekranda faqat kichik "aloqa yo'q" belgisi chiqadi.
- **Bo'sh (qulflangan):** to'liq ekran: katta kompyuter raqami, "Kompyuter bo'sh. Admin bilan bog'laning" yozuvi, soat.
- **Faol:** katta qolgan vaqt va ruxsat etilgan dasturlar plitkalari (kategoriyalar bo'yicha, exe'dan olingan ikonka bilan). Plitka bosilsa dastur ochiladi.
- **Ogohlantirish:** `WarningMinutes` qolganda doim ustda turadigan kichik oyna: "5 daqiqa qoldi". 1 daqiqa qolganda yana chiqadi.
- **Vaqt tugadi:** ochilgan dasturlar yopiladi (avval yumshoq `CloseMainWindow`, taxminan 10 soniya kutib, keyin `Kill`), so'ng Bo'sh ekranga qaytiladi yoki `TimeUpAction` ga ko'ra kompyuter o'chiriladi.

### 10.2 Dasturlarni ochish va kuzatish

- Faqat serverdan kelgan ro'yxatdagi dasturlar ochiladi. Client o'zboshimchalik bilan boshqa exe ochmaydi.
- Ochilgan har bir jarayon **Windows Job Object**'ga biriktiriladi. Shunda vaqt tugaganda uning bola jarayonlari ham birga yopiladi.
- Ma'lum muammo: Steam, Epic, Riot kabi launcherlar o'yinni alohida jarayon sifatida ochadi va u Job Object'dan "qochib" ketishi mumkin. Shuning uchun qo'shimcha himoya: dasturning exe nomi va papkasi bo'yicha jarayonlarni qidirib yopish. Bu qism real o'yinlar bilan albatta sinab ko'riladi (13-bo'lim, 6-bosqich).
- Exe topilmasa: plitka kulrang bo'ladi va serverga xabar yuboriladi.

### 10.3 Kiosk himoyasi (faqat test client'da, 8-bosqichda)

- **Alohida Windows foydalanuvchisi**, masalan `Mijoz` — oddiy foydalanuvchi, administrator emas, avtomatik login bilan.
- **Shell faqat shu foydalanuvchi uchun almashtiriladi:** `HKCU\Software\Microsoft\Windows NT\CurrentVersion\Winlogon` → `Shell` = `"C:\Program Files\Kompyuterxona\Client\ClientApp.exe" --watchdog`. Administrator hisobida oddiy desktop saqlanib qoladi, muammo bo'lsa o'sha hisobga kirib tuzatish mumkin. HKLM'dagi umumiy `Shell` qiymatiga tegilmaydi. Bu usul maqsadli Windows nashrlarida (Home/Pro) ishlashi sinab tasdiqlanadi.
- `Mijoz` foydalanuvchisi uchun cheklovlar (`HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\...`): Task Manager o'chiriladi (`DisableTaskMgr`), Ctrl+Alt+Del menyusidagi "Qulflash", "Parolni o'zgartirish", "Chiqish" yashiriladi (`DisableLockWorkstation`, `DisableChangePassword`, `NoLogoff`). Ctrl+Alt+Del'ning o'zini dastur bloklay olmaydi, bu Windows'ning himoyalangan kombinatsiyasi.
- Kiosk oynasi: chegarasiz, to'liq ekran. Bo'sh holatda doim eng ustda. Alt+F4 Bo'sh va Faol ekranlarda e'tiborsiz qoldiriladi.
- **Watchdog:** ClientApp yiqilsa, qora ekran qolmasligi kerak. Shell sifatida `ClientApp.exe --watchdog` ishga tushadi; u asosiy UI jarayonini ochadi va u kutilmaganda yopilsa qayta ochadi.
- **Texnik xizmat uchun maxfiy chiqish:** maxsus tugmalar kombinatsiyasi (masalan `Ctrl+Shift+Alt+F12`) → parol oynasi → parol to'g'ri bo'lsa `explorer.exe` ishga tushadi. Parol server orqali admin paroli bilan tekshiriladi (`VerifyTechPassword`); server bilan aloqa bo'lmasa, config'dagi xesh bilan.
- Har bir kiosk o'zgarishi uchun **qaytarish (rollback) skripti** bo'lishi shart va u o'zgarishdan **OLDIN** yoziladi.

### 10.4 Yengillik talablari

- Bo'sh turganda CPU ≈ 0%, RAM maqsadi 100 MB dan kam.
- UI taymer soniyasiga 1 marta yangilanadi. Soniyadan tez aylanadigan sikllar yo'q.
- Animatsiyalar minimal. O'yin paytida kiosk oynasi orqada turadi va hech narsa chizmaydi.
- Faol sessiya paytida ekranda doimiy ustma-ust taymer turmaydi (to'liq ekranli o'yinlarga xalaqit beradi), faqat ogohlantirish oynasi chiqadi.

### 10.5 Config fayl

`C:\ProgramData\Kompyuterxona\Client\client.config.json`:

```json
{
  "ServerUrl": "http://192.168.1.100:5000",
  "ComputerNumber": 1,
  "ClientKey": "<o'rnatishda kiritiladi>",
  "TechExitPasswordHash": "<o'rnatishda yaratiladi>",
  "LogLevel": "Information"
}
```

Repo'da faqat `client.config.example.json` saqlanadi. Haqiqiy config `.gitignore` da.

Client `--windowed` parametri bilan oddiy oyna rejimida ham ishga tushadi (ishlab chiqish va sinov uchun).

---

## 11. Xavfsizlik

- **Admin panel:** login majburiy; parol PBKDF2 xeshi bilan saqlanadi; cookie `HttpOnly` va `SameSite=Strict`; 5 marta noto'g'ri urinishdan keyin qisqa muddatga bloklash.
- **Client kompyuterlardan admin panelni bloklash:** mijoz kiosk ichidagi brauzerda `http://<admin-IP>:5000` ni ochib ko'rishi mumkin. Shuning uchun ro'yxatdagi client kompyuterlar IP manzillaridan keladigan so'rovlar admin UI va `/api` ga ruxsat olmaydi (403). Ular uchun faqat `/hubs/client` ochiq.
- **Clientlar:** faqat to'g'ri `ClientKey` bilan ulanadi. Kalitni admin sozlamalarda yangilashi mumkin.
- **Faqat LAN:** Firewall qoidasi `RemoteAddress = LocalSubnet` bilan. Routerda port forwarding qilinmaydi.
- **HTTP (HTTPS emas):** LAN ichida qabul qilinadi. Shuning uchun Wi-Fi albatta parolli bo'lishi kerak; qo'llanmada buni eslatamiz.
- Parollar va kalitlar logga yozilmaydi va git'ga tushmaydi.
- `Mijoz` foydalanuvchisi administrator emas; dastur fayllari `Program Files` da.

---

## 12. Test muhiti (SSH)

- **Men ishlayotgan kompyuter = admin/server.** Kod shu yerda yoziladi va build qilinadi.
- **Tezkor sinov:** ClientApp'ni avval shu kompyuterning o'zida `--windowed` rejimida, `http://localhost:5000` ga ulab sina.
- **Test client kompyuteri:** LAN orqali ulangan, IP `192.168.1.101` (o'zgarsa shu yerda yangilanadi), Windows foydalanuvchisi `[NOM]` (men to'ldiraman). Unda OpenSSH Server yoqilgan va ed25519 kalit orqali parolsiz ulanish sozlangan.
- Fayl nusxalash: `scp`. Buyruqlar: `ssh`. Buning uchun `scripts/dev/` ichida yordamchi PowerShell skriptlar yoz (masalan build, nusxalash va ishga tushirishni bitta buyruq bilan bajaradigan).
- SSH sessiyasidan ochilgan oynali dasturlar client ekranida **ko'rinmaydi**. Ekranda ko'rinadigan qilib ochish uchun Task Scheduler ishlatiladi: `schtasks /create ... /ru <foydalanuvchi> /it` va keyin `schtasks /run`.
- Client ekranini sen ko'ra olmaysan. Har bir sinovdan keyin menga **aniq nimaga qarashim kerakligini** ayt, natijani men aytib beraman. Loglarni esa SSH orqali o'zing o'qi.
- **Kiosk va shell o'zgarishlarini HECH QACHON shu (admin) kompyuterda sinama.** Faqat test client'da va faqat `scripts/dev/rollback-kiosk.ps1` yozilib, menga ko'rsatilgandan keyin. SSH Windows xizmati sifatida ishlaydi, shuning uchun ekran bo'sh qolsa ham SSH orqali qaytarish mumkin.
- Wake-on-LAN sinovi uchun test client'ning BIOS'ida WOL yoqilgan bo'lishi kerak (buni men qo'lda qilaman).
- Sinovlar tugagach (va haqiqiy joyga o'rnatishda) client kompyuterlarda SSH o'chiriladi.

---

## 13. Ishlash bosqichlari

Har bir bosqich kichik qadamlarga bo'linadi. Bosqich faqat "Tayyor" mezoni bajarilgandan keyin tugagan hisoblanadi.

| # | Bosqich | Nima qilinadi | Tayyor mezoni |
|---|---|---|---|
| 0 | Muhit va skelet | .NET SDK tekshiruvi, git, solution va 4 loyiha, `.gitignore` | `dotnet build` xatosiz |
| 1 | Baza va hisob mantiqi | Entity'lar, DbContext, birinchi migratsiya, BillingService va testlar | `dotnet test` yashil, `data.db` yaratiladi |
| 2 | Server | Minimal API, auth, birinchi sozlash, SignalR hub'lar, ConnectionRegistry | API brauzer yoki `curl` orqali ishlaydi |
| 3 | Admin UI | Login → bosh panel → sessiya oynasi → dasturlar → kompyuterlar → hisobotlar → sozlamalar (har biri alohida qadam) | Brauzerda to'liq boshqarish mumkin; telefonda to'g'ri ko'rinadi |
| 4 | Admin desktop | WinForms + WebView2 oyna, tray, bitta nusxa, single-file publish, `wwwroot` exe ichiga joylangan | AdminApp.exe ikki marta bosilganda ochiladi |
| 5 | Client asosi | WPF, `--windowed` rejim, ulanish va qayta ulanish, Bo'sh/Faol ekranlar, taymer | Admin sessiya boshlasa client'da taymer chiqadi, tugaganda Bo'sh ekranga qaytadi |
| 6 | Dasturlar | Ochish, Job Object, yopish, ogohlantirish oynasi | Brauzer va kamida bitta launcher orqali ochiladigan o'yin bilan sinovdan o'tdi |
| 7 | Masofaviy boshqaruv | WOL, o'chirish, qayta yuklash, xabar | O'chiq test kompyuter WOL bilan yonadi |
| 8 | Kiosk himoyasi | Avval rollback skripti; keyin `Mijoz` foydalanuvchisi, shell, cheklovlar, watchdog, maxfiy chiqish | Qayta yuklangandan keyin faqat kiosk ko'rinadi; rollback ishlaydi |
| 9 | Sayqal | Zaxira, avtostart, hisobot va CSV, AuditLog | Hammasi admin panel orqali ishlaydi |
| 10 | Deploy | `install-admin.ps1`, `install-client.ps1`, `uninstall`/rollback, `docs/admin-qollanma.md` | Toza Windows'da skript bilan noldan o'rnatiladi |
| 11 | Yakuniy sinov | 2 kompyuterda to'liq ssenariy: uzoq ishlash, server qayta yuklanishi, kabel uzilishi | Hech qanday sessiya va pul ma'lumoti yo'qolmaydi |

---

## 14. Kod yozish qoidalari

- `<Nullable>enable</Nullable>` va `<ImplicitUsings>enable</ImplicitUsings>`. Kompilyator ogohlantirishlarini e'tiborsiz qoldirma.
- Nomlar (klasslar, metodlar, o'zgaruvchilar, API, baza ustunlari) — **inglizcha**. Kod izohlari va foydalanuvchiga ko'rinadigan matnlar — **o'zbekcha (lotin)**.
- UI matnlari bir joyda turadi (`Strings.cs`, `wwwroot/js/strings.js`), keyinchalik rus tilini qo'shish oson bo'lishi uchun.
- Biznes mantiq (hisob, sessiyalar) UI'dan ajratilgan servislarda; servislar Dependency Injection orqali ulanadi.
- `async`/`await` ishlat; UI thread'ni bloklama; `.Result` va `.Wait()` ishlatma.
- Bazadagi barcha vaqtlar UTC; ekranda kompyuterning mahalliy vaqti.
- Har bir tashqi amal (jarayon ochish, WOL, fayl, tarmoq, Registry) `try/catch` va log bilan.
- Loglar: `C:\ProgramData\Kompyuterxona\logs\` (admin) va `C:\ProgramData\Kompyuterxona\Client\logs\` (client), kunlik fayllar, 14 kun saqlanadi.
- Fayllar kichik va o'qiladigan bo'lsin; bitta faylga 300 qatordan ko'p yig'ma.
- Admin UI: vanilla JS, ES modullar, `fetch`. Framework, npm va build vositalari yo'q.
- Har bir bosqich oxirida: `dotnet build`, `dotnet test`, git commit.

---

## 15. Build va publish

```powershell
# Build
dotnet build

# Testlar
dotnet test

# Admin — bitta exe (.NET o'rnatilmagan kompyuterda ham ishlaydi)
dotnet publish src/Kompyuterxona.Admin -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish/admin

# Client — bitta exe
dotnet publish src/Kompyuterxona.Client -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish/client
```

- `PublishTrimmed` ishlatma: WinForms va WPF bilan mos emas.
- `wwwroot` fayllari exe ichiga joylanadi (`ManifestEmbeddedFileProvider`), shunda admin uchun haqiqatan bitta fayl bo'ladi.
- WebView2 single-file rejimda to'g'ri ishlashini 4-bosqichda alohida tekshir.

---

## 16. Deploy (o'rnatish) — keyingi bosqich

Bu skriptlar 10-bosqichda yoziladi. Hozir faqat yodda tut.

**`install-client.ps1`** (administrator sifatida ishga tushiriladi):

1. Fayllarni `C:\Program Files\Kompyuterxona\Client\` ga nusxalash.
2. Kompyuter raqami, server IP'si va client kalitini so'rab, config yozish.
3. Statik IP o'rnatish (ixtiyoriy, savol bilan).
4. `Mijoz` foydalanuvchisini yaratish va avtomatik login sozlash (parolni ochiq matnda Registry'da saqlamaslik uchun Sysinternals Autologon tavsiya qilinadi; u deploy paketiga qo'shiladi).
5. `Mijoz` uchun shell va cheklovlar.
6. Tarmoq kartasida Wake-on-LAN ruxsatini yoqishga urinish (natijani ko'rsatish). BIOS'dagi WOL qo'lda yoqiladi.
7. Rollback skriptini ham o'rnatish.
8. Client uchun kiruvchi Firewall port **shart emas** (ulanishni client o'zi boshlaydi).

**`install-admin.ps1`:**

1. Fayllarni nusxalash.
2. Firewall: 5000-port, faqat `LocalSubnet` uchun.
3. Statik IP.
4. Sleep va Hibernate'ni o'chirish.
5. Avtostart.
6. WebView2 Runtime borligini tekshirish, yo'q bo'lsa offline o'rnatish.

Haqiqiy joyda SSH yoqilmaydi.

---

## 17. Ochiq savollar (mijoz bilan aniqlashtiriladi)

Bu savollarga javob kelguncha qavs ichidagi vaqtinchalik qarorlar bilan ishlayver va ularni sozlanadigan qilib qo'y.

- Soatlik narx qancha? (hozircha 10 000 so'm)
- Qaysi dasturlar va o'yinlar o'rnatiladi, qaysi launcherlar orqali (Steam, Epic, Riot...)? (test uchun Chrome va bitta Steam o'yini)
- Client kompyuterlardagi Windows nashri: Home yoki Pro? (ikkalasida ishlaydigan usullarni tanlash)
- Vaqt tugaganda standart amal: Bo'sh ekranga qaytishmi yoki o'chirishmi? (Bo'sh ekranga qaytish)
- Erta to'xtatilgan sessiya uchun pul qaytariladimi? (yo'q, faqat ishlatilgan vaqt yoziladi)
- Kompyuterxonada Wi-Fi bormi (telefondan kirish uchun)? (bor deb hisoblaymiz)
- Bir nechta xodim admin bo'ladimi, har biriga alohida login kerakmi? (bitta hisob, lekin sxema bir nechtasini qo'llaydi)
- Kompyuterlar soni aniq nechta? (15)
- Tarmoq kartalari va BIOS'lar Wake-on-LAN'ni qo'llab-quvvatlaydimi? (joyida tekshiriladi)

---

## 18. Lug'at

| Atama | Oddiy tilda |
|---|---|
| Shell | Windows ishga tushganda foydalanuvchiga ko'rsatiladigan asosiy dastur. Odatda `explorer.exe` (desktop, taskbar). Biz uni mijoz foydalanuvchisi uchun kiosk dasturiga almashtiramiz |
| Kiosk rejimi | Kompyuterda faqat bitta dastur va ruxsat etilganlar ishlaydigan, boshqa joyga kirib bo'lmaydigan holat (bankomat ekraniga o'xshaydi) |
| Wake-on-LAN (WOL) | O'chiq kompyuterni tarmoq orqali maxsus "magic packet" yuborib yoqish |
| MAC manzil | Tarmoq kartasining zavod raqami; WOL shu raqam bo'yicha ishlaydi |
| SignalR | Server va dastur o'rtasidagi doimiy ochiq "telefon liniyasi": server xohlagan paytda buyruq yubora oladi |
| WebView2 | Dastur ichiga joylashtirilgan Edge brauzer oynasi |
| SQLite | Bitta fayldan iborat ma'lumotlar bazasi |
| EF Core | C# klasslari orqali baza bilan ishlash vositasi; SQL so'rovlarni o'zi yozadi |
| Migratsiya | Baza tuzilishidagi o'zgarishlar tarixi; dastur yangilanganda baza ham avtomatik yangilanadi |
| Heartbeat | Client har 15 soniyada yuboradigan "men tirikman" signali |
| Job Object | Windows'da bir guruh jarayonni birga boshqarish va birga yopish imkoniyati |
| Watchdog | Asosiy dastur yiqilsa uni qayta ishga tushiradigan "qo'riqchi" jarayon |
| Self-contained single-file | .NET o'rnatilmagan kompyuterda ham ishlaydigan bitta exe fayl |
| Rollback | O'zgarishni oldingi holatga qaytarish |

---

## 19. Loyiha holati

> Claude Code har bir bosqich tugagach shu bo'limni yangilaydi.

- [x] 0. Muhit va skelet
- [ ] 1. Baza va hisob mantiqi
- [ ] 2. Server
- [ ] 3. Admin UI
- [ ] 4. Admin desktop
- [ ] 5. Client asosi
- [ ] 6. Dasturlar
- [ ] 7. Masofaviy boshqaruv
- [ ] 8. Kiosk himoyasi
- [ ] 9. Sayqal
- [ ] 10. Deploy
- [ ] 11. Yakuniy sinov

**Hozirgi bosqich:** 1 (Baza va hisob mantiqi)
**Oxirgi yangilanish:** 2026-10-23

### 0-bosqichda nima qilindi

- `.NET SDK 10.0.113` tekshirildi (`dotnet --list-sdks`). Barcha loyihalar `net10.0` / `net10.0-windows` ga mo'ljallangan.
- `KompyuterxonaTizimi.sln` (klassik `.sln` formatda — eski Visual Studio versiyalari ham ocha oladi) va 4 loyiha yaratildi:
  - `src/Kompyuterxona.Shared` — `net10.0`, umumiy kod (`Constants.cs`, `Dtos/ComputerState.cs`, `Commands/ClientCommandNames.cs`).
  - `src/Kompyuterxona.Admin` — `Microsoft.NET.Sdk.Web` + `UseWindowsForms` + `WinExe`, chiqish fayli `AdminApp.exe`.
  - `src/Kompyuterxona.Client` — WPF + `WinExe`, chiqish fayli `ClientApp.exe`.
  - `tests/Kompyuterxona.Tests` — xUnit.
- `.gitignore` (baza, loglar, zaxiralar, haqiqiy `client.config.json` — hammasi git'dan tashqarida), `.editorconfig`.
- `Directory.Build.props` — barcha loyihalar uchun umumiy sozlamalar: `Nullable`, `ImplicitUsings`, `EnableWindowsTargeting`.
- `dotnet build` → 0 xato, 0 ogohlantirish. `dotnet test` → 3 test yashil.

### Qabul qilingan texnik qarorlar (0-bosqich)

- **`EnableWindowsTargeting=true`** (`Directory.Build.props` da): WinForms va WPF loyihalarini Windows'dan boshqa tizimda ham **kompilyatsiya** qilish imkonini beradi. Windows'da hech narsani o'zgartirmaydi.
- **`SupportedOSPlatformVersion` ishlatilmaydi.** `net10.0-windows` TFM'ida standart platforma versiyasi 7.0 bo'lgani uchun u bilan to'qnashadi (NETSDK1135 xatosi). Agar keyinchalik Windows 10 ga xos API kerak bo'lsa, TFM `net10.0-windows10.0.19041.0` ga o'zgartiriladi.
- Ikkala Windows loyihasida `AssemblyName` aniq belgilangan: `AdminApp` va `ClientApp` (5-bo'limdagi nomlarga mos).

### Eslatmalar

- **Bu sessiya Linux (bulutli konteyner) da ishlagan, Windows'da emas.** Shuning uchun kod shu yerda yozilib, `dotnet build` va `dotnet test` bilan tekshirildi, lekin **WinForms/WPF oynalarini ishga tushirib ko'rish imkoni bo'lmadi.** Haqiqiy ishga tushirish va ekranga qarash bilan bog'liq barcha sinovlar (12-bo'lim) sizning Windows kompyuteringizda bajariladi.
- Shu sababli: Windows'ga bog'liq qismlarni (WebView2, Job Object, Registry, WOL, SSH, kiosk) har bir bosqichda siz o'zingiz sinab, natijani menga aytib turasiz.
- `scripts/dev/`, `scripts/deploy/` va `docs/` papkalari hozircha bo'sh (git bo'sh papkalarni saqlamaydi) — ular 10-bosqichda to'ldiriladi.
