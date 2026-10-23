# Windows cheklovlari, xatarlar va yechimlar

> Bu hujjat `CLAUDE.md` ning qo'shimchasi. Unda Windows platformasining bizning loyihamizga
> ta'sir qiladigan cheklovlari yig'ilgan. Har bir yangi bosqichni boshlashdan oldin tegishli
> qismini o'qing. Har bir band bir xil tuzilishga ega: **nima bo'ladi** va **biz nima qilamiz**.

**Oxirgi yangilanish:** 2026-10-23

---

## A. Ishonchli ishlaydi — xavotir yo'q

| Narsa | Izoh |
|---|---|
| Foydalanuvchi uchun shell'ni almashtirish | `HKCU\Software\Microsoft\Windows NT\CurrentVersion\Winlogon\Shell` Windows 10/11 **Home va Pro**'da ishlaydi. Taskbar, Start menu va desktop yo'qoladi |
| Administrator hisobi buzilmaydi | O'zgarish faqat `Mijoz` foydalanuvchisiga tegishli (HKCU). Muammo chiqsa, administrator hisobiga kirib tuzatish mumkin |
| Win-kombinatsiyalarining ko'pi o'ladi | `Win+R`, `Win+E` va boshqalarni explorer bajaradi. Explorer bo'lmagach ular o'z-o'zidan ishlamaydi — bu bizga foyda |
| Oddiy foydalanuvchi dastur o'rnata olmaydi | `Program Files` ga yozish uchun administrator huquqi kerak |
| O'yinlar explorer'siz ishlaydi | Steam, o'yinlar va brauzerlar shell'siz ham normal ochiladi |
| Oddiy foydalanuvchi kompyuterni o'chira oladi | `SeShutdownPrivilege` standart holatda `Users` guruhida bor, shuning uchun "vaqt tugadi → o'chirish" ishlaydi |

---

## B. Hech qachon qila olmaymiz

Bularni bilib turish kerak — ularni dastur bilan yopishga urinish vaqtni behuda sarflash bo'ladi.

### B1. Ctrl+Alt+Del ni bloklash

**Nima bo'ladi.** Bu Windows'ning himoyalangan kombinatsiyasi (Secure Attention Sequence). Uni hech
bir oddiy dastur ushlab qola olmaydi — aynan shu narsa foydalanuvchini soxta login oynalaridan himoya qiladi.

**Biz nima qilamiz.** Kombinatsiyaning o'zini emas, undan keyin chiqadigan **menyuni** cheklaymiz:
`DisableTaskMgr` (Task Manager), `DisableLockWorkstation` (Qulflash), `DisableChangePassword`,
`NoLogoff`. Natijada menyuda amalda faqat "Bekor qilish" qoladi.

### B2. Jismoniy harakatlar

**Nima bo'ladi.** Reset tugmasi, quvvat kabelini sug'urish, USB flesh yoki boshqa diskdan yuklash,
korpusni ochib qattiq diskni olish — bularning hech biri dasturga bog'liq emas.

**Biz nima qilamiz.** Hech narsa — bu dastur ishi emas. Qo'llanmada aniq yoziladi: **BIOS/UEFI
paroli qo'yilsin, boot tartibi qulflansin, korpus qulflansin.** Bu klub egasining ishi.

### B3. Microsoft'ning tayyor kiosk vositalari bizga yaramaydi

**Nima bo'ladi.** Windows'da ikkita rasmiy kiosk vositasi bor:

- **Shell Launcher** — aynan bizga kerak bo'lgan narsa (istalgan dasturni shell qilish), lekin u
  faqat **Enterprise va Education** nashrlarida. Pro'da yo'q.
- **Assigned Access** — Pro'da bor, lekin u faqat **bitta** ilova (UWP yoki Edge) uchun mo'ljallangan;
  bizga 10 xil o'yin va brauzer kerak.

**Biz nima qilamiz.** Shell'ni qo'lda, Registry orqali almashtiramiz — bu Home va Pro'da ham ishlaydi
va aynan Shell Launcher qiladigan ishni qiladi, faqat qo'lda.

### B4. Mijozning o'z jarayonini yopishini butunlay to'sish

**Nima bo'ladi.** `Mijoz` nomidan ishlayotgan dasturni o'sha foydalanuvchi yopa oladi — bu Windows'ning
asosiy qoidasi. Task Manager'ni o'chirsak ham, boshqa yo'llar qolishi mumkin.

**Biz nima qilamiz.** Yopilishiga yo'l qo'yamiz, lekin **qaytarib turamiz**: shell + Task Scheduler
qo'riqchisi (`CLAUDE.md` 10.3). Vazifani administrator yaratgani uchun mijoz uni o'chira olmaydi.

---

## C. Ishlaydi, lekin to'g'ri sozlash kerak

Eng ko'p muammo shu bo'limdan chiqadi.

### C1. Qo'riqchi: `AutoRestartShell` ga ishonib bo'lmaydi

**Nima bo'ladi.** `HKLM\...\Winlogon\AutoRestartShell = 1` Windows shell'ni qaytaradi, lekin
hujjatlarga ko'ra faqat u **"kutilmaganda to'xtaganda"** (crash). Foydalanuvchi dasturni ataylab
yopganda Windows uni qaytarmasligi haqida amaliy hisobotlar bor.

**Biz nima qilamiz.** Ikki qatlam: shell (`ClientApp.exe --watchdog`) UI jarayonini qaraydi, Task
Scheduler vazifasi esa har 1 daqiqada qo'riqchining o'zini tekshiradi. 8-bosqichda test kompyuterda
qo'riqchini qo'lda o'ldirib, qaytishini albatta tekshiramiz.

### C2. Texnik rejim bilan qo'riqchi "urushib" qolmasligi

**Nima bo'ladi.** Agar qo'riqchi har daqiqada kioskni ochaversa, admin Windows'ga chiqqanda kiosk
uni 1 daqiqada bosib qaytaradi va texnik xizmat ko'rsatib bo'lmaydi.

**Biz nima qilamiz.** `maintenance.flag` belgisi: qo'riqchi u amal qilib turganda jim turadi.
Belgi muddati tugasa yoki kompyuter qayta yuklansa, kiosk o'zi qaytadi (`CLAUDE.md` 10.6).

### C3. Ruxsat etilgan dastur orqali qochib chiqish

**Nima bo'ladi.** Exe ro'yxati qum qutisi emas. Brauzerda `Ctrl+O` → fayl oynasi → papkalarni ko'rish,
fayllarni ochish, hatto o'ng tugma bilan boshqa dasturni ishga tushirish mumkin. Steam ichida ham
brauzer bor.

**Biz nima qilamiz.** (1) Brauzer uchun Registry siyosatlari: `file://*` bloklanadi, yuklab olish,
Developer Tools va kengaytmalar o'chiriladi. (2) Software Restriction Policy — faqat ruxsat etilgan
papkalardagi exe'lar ishga tushadi, shunda fayl oynasidan `cmd.exe` ochib bo'lmaydi.
(3) `docs/kiosk-xavfsizlik-tekshiruvi.md` ro'yxati bo'yicha har bir yo'l qo'lda sinaladi.

### C4. Wake-on-LAN — beshta shart

**Nima bo'ladi.** WOL juda injiq: shartlardan bittasi bajarilmasa, kompyuter yonmaydi va sabab
ko'rinmaydi.

**Biz nima qilamiz.** Beshtasini ham ta'minlaymiz: (1) **kabel** ulanish — Wi-Fi'da WOL amalda
ishlamaydi; (2) BIOS'da WOL yoqilgan; (3) tarmoq kartasi xossalarida "Wake on Magic Packet" va
"Allow this device to wake the computer"; (4) **Fast Startup o'chirilgan** — bu eng ko'p unutiladigan
shart, chunki Fast Startup bilan "O'chirish" haqiqiy o'chirish bo'lmaydi; (5) bir xil subnet.

### C5. 5000-port band bo'lishi

**Nima bo'ladi.** Kestrel "An attempt was made to access a socket in a way forbidden by its
permissions" (xato 10013) deb ishga tushmaydi. Ko'pincha sabab — portni boshqa dastur emas, balki
**Windows o'zi rezerv qilib olgani** (Hyper-V / WinNAT / WSL o'rnatilgan bo'lsa).

**Biz nima qilamiz.** Tekshirish: `netsh interface ipv4 show excludedportrange protocol=tcp` va
`netstat -ano | findstr :5000`. Dastur tushunarli xabar chiqaradi, port `ServerPort` sozlamasidan
yoki `AdminApp.exe --port 5050` bilan o'zgartiriladi. O'rnatish skripti portni oldindan tekshiradi.

### C6. WebView2 ma'lumot papkasi

**Nima bo'ladi.** WebView2 standart holatda exe **yonida** `AdminApp.exe.WebView2` papkasini
yaratmoqchi bo'ladi. Dastur `Program Files` da turgani uchun bu taqiqlangan va oyna umuman ochilmaydi.

**Biz nima qilamiz.** Papkani majburan beramiz:
`CoreWebView2Environment.CreateAsync(userDataFolder: @"C:\ProgramData\Kompyuterxona\WebView2")`.

### C7. Papka huquqlari

**Nima bo'ladi.** `C:\ProgramData` ichida administrator yaratgan papkaga oddiy foydalanuvchi
yoza olmasligi mumkin. Natijada baza ham, loglar ham yozilmaydi va dastur tushunarsiz xato beradi.

**Biz nima qilamiz.** O'rnatish skripti `C:\ProgramData\Kompyuterxona\` (admin) va
`C:\ProgramData\Kompyuterxona\Client\` (client) papkalariga "Modify" huquqini aniq beradi.

### C8. Windows Update o'rtada qayta yuklashi

**Nima bo'ladi.** Yangilanish mijozning o'yinini buzadi va sessiya vaqti behuda ketadi.

**Biz nima qilamiz.** Client'larda "faol soatlar" (active hours) klub ish vaqtiga qo'yiladi,
avtomatik qayta yuklash cheklanadi. Qayta yuklash bo'lsa ham sessiya yo'qolmaydi — u serverda
saqlangan, kiosk qaytganda davom etadi.

### C9. Uyqu rejimi va to'xtab qoladigan taymer

**Nima bo'ladi.** Kompyuter uyquga ketsa: (a) server bilan aloqa uziladi, (b) client'dagi
`Stopwatch` (monoton soat) **to'xtab qoladi** — chunki u uyquda o'tgan vaqtni sanamaydi.
Natijada client vaqtni haqiqiydan ko'proq ko'rsatadi.

**Biz nima qilamiz.** Client kompyuterlarda uyqu o'chiriladi; faol sessiya paytida client
`SetThreadExecutionState` bilan qo'shimcha himoya qo'yadi; va eng muhimi — client **har 15
soniyada serverdan haqiqiy qolgan vaqtni oladi**, shuning uchun xato uzoq yashamaydi.

### C10. Avtologin paroli

**Nima bo'ladi.** Windows'ning oddiy avtologin usulida parol Registry'da (`DefaultPassword`)
**ochiq matnda** yotadi — har qanday odam o'qib oladi.

**Biz nima qilamiz.** Sysinternals **Autologon** utilitasini ishlatamiz: u parolni LSA'da shifrlangan
holda saqlaydi. Utilita deploy paketiga qo'shiladi (internetsiz ishlaydi).

### C11. SmartScreen va antivirus

**Nima bo'ladi.** Bizning exe imzolanmagan (kod imzolash sertifikati pullik), ustiga-ustak u shell
almashtiradi, jarayonlarni yopadi va Registry'ga yozadi — antivirus uchun shubhali ko'rinadi.

**Biz nima qilamiz.** O'rnatish papkasini Windows Defender istisnolariga qo'shish qo'llanmada
yoziladi. Keyinchalik kerak bo'lsa, kod imzolash sertifikati sotib olinadi.

### C12. Admin hisobdan chiqsa, server to'xtaydi

**Nima bo'ladi.** AdminApp oddiy dastur — u foydalanuvchi sessiyasida yashaydi. Admin "Log off"
qilsa yoki kompyuter o'chsa, server ham o'ladi: yangi sessiya ochib bo'lmaydi.

**Biz nima qilamiz.** Admin kompyuter doim yoqiq va tizimga kirgan holda turadi (uyqu o'chiriladi,
avtostart qo'yiladi). Ishlab turgan sessiyalar bundan zarar ko'rmaydi: client vaqtni o'zi sanab
tugatadi, server qaytganda baza bo'yicha hammasi to'g'rilanadi.

### C13. Steam va Job Object

**Nima bo'ladi.** Agar Steam allaqachon ishlab tursa, o'yinni **Steam** ochadi, biz emas. O'yin
bizning Job Object'imizga tushmaydi va vaqt tugaganda yopilmaydi.

**Biz nima qilamiz.** Launcherlarning Windows bilan avtostarti o'chiriladi; launcher doim client
orqali ochiladi (shunda u ham, uning bolalari ham bitta Job Object ichida bo'ladi). Zahira sifatida
exe yo'li bo'yicha jarayonlarni qidirib yopamiz.

### C14. Windows 10 ning qo'llab-quvvatlash muddati tugagan

**Nima bo'ladi.** Microsoft Windows 10 (22H2) ni **2025-yil 14-oktabrda** qo'llab-quvvatlashni
to'xtatdi — yangi xavfsizlik yangilanishlari kelmaydi.

**Biz nima qilamiz.** Dastur Windows 10'da ham ishlaydi, lekin qo'llanmada Windows 11 tavsiya
qilinadi. Bu klub egasining qarori.

### C15. Ishlab turgan client dasturini yangilash

**Nima bo'ladi.** Kiosk ishlab turganda `ClientApp.exe` fayli **band** bo'ladi — ustiga yangi
versiyani yozib bo'lmaydi.

**Biz nima qilamiz.** Yangilash tartibi: admin panelidan texnik rejimga o'tkazish → qo'riqchi
to'xtaydi → fayl almashtiriladi → kioskka qaytarish. Bu tartib 10-bosqichda skript qilinadi.

### C16. Zaxira nusxalar va svet o'chishi

**Nima bo'ladi.** Zaxira nusxalar bazaning o'zi bilan bir diskda turadi — disk o'lsa, ikkalasi
ham ketadi. Svet o'chsa, SQLite yozuv o'rtasida uzilib qolishi mumkin.

**Biz nima qilamiz.** `synchronous=FULL` bilan yozuvlar diskka kafolatli tushadi; admin kompyuterga
**UPS** tavsiya qilinadi; qo'llanmada haftada bir marta zaxirani USB diskka ko'chirish yoziladi.
