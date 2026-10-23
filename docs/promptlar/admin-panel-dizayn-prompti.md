Kompyuterxona (o'yin klubi) boshqaruv tizimining **admin paneli** uchun bosiladigan, interaktiv UI dizayn qil. Natija — bitta mustaqil HTML fayl: ma'lumotlar soxta va server yo'q, lekin hamma tugma, oyna va taymer haqiqatan ishlashi kerak. Maqsad: panelni haqiqiy kod yozishdan oldin ishlatib ko'rish va kelishib olish.

## Mahsulot haqida

- Kompyuterxonada 15 ta mijoz kompyuteri va 1 ta admin kompyuteri. Hammasi Windows, bitta mahalliy tarmoqda. **Internet yo'q.**
- Mijoz pul to'laydi, tizim pulni vaqtga aylantiradi va kompyuterda sessiya ochiladi. Orqaga sanoq boradi; vaqt tugaganda kompyuter qulflanadi.
- Admin shu panel orqali: sessiya ochadi, vaqt qo'shadi, pauza qiladi, to'xtatadi, kompyuterni masofadan yoqadi va o'chiradi, mijozga dastur ochib beradi, hisobot ko'radi.
- Panel admin kompyuterida dastur oynasi ichida ochiladi, va shu tarmoqdagi telefon brauzeridan ham ochiladi.

## Foydalanuvchi kim

Klub administratori. Kassada o'tiradi va smena davomida soatlab shu ekranga qaraydi. Ko'p amalni tez va xatosiz bosishi kerak — ayniqsa "sessiya boshlash" va "vaqt qo'shish". Zalni aylanib yurganda telefondan ham boshqaradi.

## Qattiq texnik cheklovlar — dizaynga bevosita ta'sir qiladi

1. **Oddiy HTML + CSS + vanilla JavaScript.** Framework ishlatma: React, Vue, Svelte, Tailwind, Bootstrap — hech biri.
2. **CDN yo'q, Google Fonts yo'q, tashqi so'rov yo'q.** Klubda internet yo'q, shuning uchun hamma narsa fayl ichida bo'lishi shart. Faqat tizim shriftlari.
3. **Ikonkalar** — inline SVG yoki Unicode belgi. Ikonka kutubxonasi yuklanmaydi. Emoji'ni ikonka o'rniga ishlatma.
4. **Telefon kengligida (≈390px) to'liq ishlashi shart.** Gorizontal scroll bo'lmasin; kartalar bitta ustunga tushsin; tugmalar barmoq bilan bosishga qulay bo'lsin.
5. **Qorong'u va yorug' mavzu** — ikkisi ham CSS o'zgaruvchilari (tokenlar) orqali, tizim sozlamasiga qarab avtomatik almashsin.
6. Natija — **bitta mustaqil HTML fayl.**

## Dizayn asosi

Quyidagi tokenlar mavjud prototipdan olingan. **Ranglarni saqla**, lekin joylashuvni, oraliqlarni va detallarni yaxshilashga ruxsat — ayniqsa prototipda yo'q ekranlarda (jadvallar, hisobotlar, sozlamalar).

Qorong'u mavzu (asosiy):

```
--bg #0e1319   --surface #161d26   --surface-2 #1c2531   --line #2a3542
--fg #e8eef5   --fg-dim #94a3b4    --fg-faint #64748b
--accent #e9a93c   --accent-fg #1a1205
```

`--accent` — tilla rang. Faqat pul raqamlari va asosiy tugmalar uchun, kam ishlat. Sababi: bu kassa dasturi, pul eng muhim ma'lumot.

Holat ranglari (ma'nosi qat'iy, o'zgartirma):

```
oflayn    #64748b   kulrang
bo'sh     #3fb950   yashil
band      #4a9eff   ko'k
tugayapti #f05a4a   qizil  (5 daqiqadan kam qolgan)
pauza     #b07cd6   binafsha
```

Yorug' mavzu uchun shu tokenlarning yorug' variantlarini o'zing tanla (fon oqish, matn to'q).

Shriftlar:

```
UI:      "Segoe UI", -apple-system, system-ui, "Noto Sans", Arial, sans-serif
Raqamlar: "Cascadia Mono", Consolas, "SF Mono", ui-monospace, monospace
```

Raqamlar uchun `font-variant-numeric: tabular-nums` — taymer sanaganda ustunlar qalqimasligi uchun. Burchak radiusi 10px. Kompyuter kartasining chap chetida 3px holat chizig'i.

## Hisob qoidalari — raqamlar ishonchli ko'rinishi uchun

- Pul — butun son, **so'm**da. Mingliklar bo'sh joy bilan ajratiladi: `15 000`, `385 000`.
- Soatlik narx 10 000 so'm/soat (sozlamalardan o'zgaradi).
- **Summadan vaqtga:** `soniya = floor(summa × 3600 / narx)`. Masalan 15 000 so'm → 5400 s = 1 soat 30 daqiqa.
- **Vaqtdan summaga:** `summa = ceil(soniya × narx / 3600)`. Masalan 60 daqiqa → 10 000 so'm.
- Vaqt formati: `01:23:45`. Bir daqiqadan kam qolsa soniyalarda yoz ("36 soniya").
- 5 daqiqa qolganda karta qizilga o'tadi.
- Bitta sessiyaga eng ko'p 24 soat. Oshsa, tasdiq so'rashga ogohlantirish chiqadi.

## Ekranlar

Asosiy navigatsiya — tepadagi yorliqlar: **Bosh panel · Dasturlar · Kompyuterlar · Hisobotlar · Sozlamalar.**

Login va birinchi sozlash alohida ekranlar. Ularni ko'rish uchun burchakda kichik, bilinmaydigan "demo" o'tkazgichi qo'y (masalan `Ekran: Panel / Login / Birinchi sozlash`) — shunda hamma holatni bosib ko'rish mumkin bo'ladi.

### 1. Login

Klub nomi, foydalanuvchi nomi, parol, "Kirish" tugmasi. Noto'g'ri parol holatini ham ko'rsat. Besh marta xato urinishdan keyin "bir necha daqiqaga bloklandi" xabari.

### 2. Birinchi sozlash

Dastur birinchi marta ochilganda chiqadi: admin hisobini yaratish (login, parol, parolni takrorlash) va shu yerda bir marta ko'rsatiladigan "client kaliti" — nusxalash tugmasi va "bu kalitni har bir mijoz kompyuterini o'rnatishda kiritasiz" izohi bilan.

### 3. Bosh panel — eng muhim ekran

Tepa qatorda: dastur nomi, joriy soat.

Statistika plitkalari: **Bugungi tushum** (tilla rangda, eng katta), **Band** (n / 15), **Bo'sh**, **Oflayn**, **Soatlik narx**.

Tugmalar qatori: "Hammasini yoqish", "Bo'shlarini o'chirish".

15 ta kompyuter kartasi to'rida. Har bir kartada:

- `PC-01` ko'rinishidagi raqam, ixtiyoriy nom (`VIP-1`)
- holat yorlig'i (yuqoridagi ranglar bilan)
- katta qolgan vaqt, har soniyada sanaladi
- ochiq dastur nomi ("Counter-Strike 2")
- qolgan vaqtni ko'rsatuvchi ingichka chiziq
- holatga qarab tugmalar:
  - **Oflayn:** Yoqish
  - **Bo'sh:** Sessiya boshlash (asosiy), Dastur
  - **Band:** Vaqt qo'shish (asosiy), Pauza
  - **Pauza:** Davom ettirish (asosiy), To'xtatish
  - hammasida: qo'shimcha amallar tugmasi (⋮)

### 4. Sessiya boshlash / vaqt qo'shish oynasi

Ikki rejim o'tkazgichi: **Summa** va **Vaqt**.

- Katta raqam kiritish maydoni
- Tez tugmalar: summa rejimida 5 000 / 10 000 / 20 000 / 50 000; vaqt rejimida 30 / 60 / 120 / 180 daqiqa
- Natija bloki, kiritish paytida darhol yangilanadi: hisoblangan vaqt (eng katta matn), summa, tugash vaqti ("21:45"), vaqt qo'shilganda — yangi qoldiq
- Soatlik narx oyna sarlavhasida ko'rinadi
- 24 soatdan oshsa ogohlantirish qatori

### 5. Sessiyani to'xtatish tasdig'i

Qaysi kompyuter, ishlatilgan vaqt, qolgan vaqt, va "qolgan vaqt uchun pul qaytarilmaydi" eslatmasi. Ha / Bekor qilish.

### 6. Qo'shimcha amallar (⋮)

Xabar yuborish · Dastur ochish · Sessiyani to'xtatish · **Windows'ga chiqarish (texnik rejim)** · Qayta yuklash · O'chirish. Xavfli amallar (to'xtatish, o'chirish) qizil rangda.

### 7. Dasturlar

Ikki qismdan iborat:

**(a) Katalog jadvali:** nom, exe yo'li, kategoriya (Brauzer / O'yin / Ofis / Boshqa), faol-nofaol. Qo'shish va tahrirlash oynasi. Oynada eslatma: "exe yo'li **mijoz kompyuterlarida** mavjud bo'lishi kerak".

**(b) Kompyuter × dastur jadvali:** qatorlar — kompyuterlar, ustunlar — dasturlar, kataklarda belgilash katakchalari. "Barchaga qo'llash" tugmasi. Bu jadval keng bo'ladi — telefonda qanday ko'rinishini alohida o'yla.

### 8. Kompyuterlar

Jadval: raqam, nom, MAC manzil, IP, holat, oxirgi ko'rilgan vaqt. Qo'shish va tahrirlash oynasi.

Tepada alohida blok: **"Yangi kompyuter topildi"** — tarmoqda ro'yxatda yo'q kompyuter paydo bo'lsa, uning raqami, MAC va IP manzili ko'rinadi va bitta tugma bilan ro'yxatga qo'shiladi.

### 9. Hisobotlar

- Sana oralig'i va tez tanlovlar: Bugun / Kecha / 7 kun / Oy
- Yig'indilar: jami tushum, jami soat, sessiyalar soni
- Kompyuterlar bo'yicha guruhlangan ko'rinish (qaysi kompyuter qancha pul keltirgan)
- Sessiyalar jadvali: kompyuter, boshlanish, tugash, davomiylik, summa, tugash sababi (Vaqt tugadi / Admin to'xtatdi)
- "CSV yuklab olish" tugmasi

### 10. Sozlamalar

Soatlik narx · ogohlantirish daqiqasi · vaqt tugaganda amal (Bo'sh ekranga qaytish / Kompyuterni o'chirish) · zaxira papkasi va vaqti · "Windows bilan birga ishga tushirish" · client kaliti (ko'rsatish va yangilash) · parolni almashtirish.

Pastda **amallar jurnali** jadvali: vaqt, foydalanuvchi, amal, izoh. Bu klub egasiga xodimlar ishini nazorat qilish uchun kerak (narx o'zgarishi, sessiyani erta to'xtatish va shunga o'xshash yozuvlar).

## Interaktivlik talablari

- Taymerlar **haqiqatan** har soniyada sanaladi. Vaqt tugasa karta o'zi "Bo'sh" holatiga o'tadi.
- Tugmalar holatni o'zgartiradi: sessiya ochilsa karta ko'k bo'ladi, pauza bosilsa binafsha bo'ladi, tushum yig'indisi oshadi.
- Ekranlar orasida o'tish ishlaydi. Hamma jadval va forma real ko'rinadigan soxta ma'lumot bilan to'la.
- Amallardan keyin qisqa xabar (toast): "PC-03: 1 soat 30 daqiqa qo'shildi".
- **`alert()`, `confirm()`, `prompt()` ishlatma** — tasdiq va xabarni oynaning o'zida chiz.
- Soxta ma'lumot ishonchli bo'lsin: 15 kompyuter aralash holatlarda (bir nechtasi band, ikkitasi oflayn, bittasi pauzada, bittasi tugayapti), bugungi tushum ~385 000 so'm, dastur nomlari haqiqiy (Google Chrome, Steam, Counter-Strike 2, Dota 2).

## Matnlar

Interfeysdagi **hamma matn o'zbek tilida (lotin yozuvida)**. Masalan: "Sessiya boshlash", "Vaqt qo'shish", "Pauza", "Davom ettirish", "To'xtatish", "Bo'sh", "Band", "Oflayn", "Tugayapti", "Bugungi tushum", "Soatlik narx", "Hammasini yoqish". Lorem ipsum yoki inglizcha matn qoldirma.

## Nimalardan saqlan

- Hamma blokka bir xil radius, soya va ramka berish — ierarxiya yo'qoladi. Karta uslubini faqat kerak joyda ishlat.
- Katta bo'sh "hero" bo'limlar va reklama sahifasi uslubi. Bu — ish quroli, smena davomida boshqariladi.
- Ma'nosiz animatsiya. Harakat faqat holat o'zgarganini bildirish uchun.
- Emoji'ni ikonka o'rniga ishlatish.
- Ekran kengligidan oshib ketadigan jadvallar — ularni o'z ichida scroll qiladigan idishga sol.

## Natija

Bitta mustaqil HTML fayl. Birinchi ochilganda **bosh panel** ko'rinsin, ma'lumotlar bilan to'la va taymerlar ishlab turgan holda — bo'sh ekran yoki "ma'lumot kiriting" holati bo'lmasin.

Agar dizayn kanvasi (artboard) ko'rinishida ishlasang, har bir ekranni alohida artboard qil, lekin **bosh panel baribir interaktiv bo'lishi kerak** — uning qiymati aynan bosib ko'rishda.
