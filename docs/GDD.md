# PROJECT ECLIPSE: LOST ZONE (PC EDITION)
## MASTER GAME DESIGN DOCUMENT — ŞİRKET İÇİ / GİZLİ

---

### DOKÜMAN KONTROL

| Alan | Bilgi |
|---|---|
| Doküman Tipi | Master GDD (Living Document) |
| Sürüm | v1.0 — Bölüm 1 |
| Durum | ONAYA HAZIR — Pre-Production |
| Platform | Windows 11 PC (Steam + Epic Games Store) |
| Hedef Motor | Unity 6 (URP 2D Renderer) — alternatif: Godot 4.x |
| Gizlilik | Şirket İçi — NDA Kapsamında |
| Bölüm Yapısı | 1: Genel Bakış & Hikâye · 2: Karakterler & AI · 3: Silahlar & Gunsmith · 4: Harita Tasarımı · 5: Teknik GDD & Sistemler |

---

# BÖLÜM 1: GENEL BAKIŞ, CORE LOOP, HİKÂYE VE EVREN TASARIMI

---

## 1. EXECUTIVE SUMMARY

### 1.1 High Concept (Tek Cümle)

> **S.T.A.L.K.E.R.'ın atmosferi, Escape from Tarkov'un risk/ödül gerilimi ve Dead Cells'in kusursuz oynanış hissi; 3D dinamik aydınlatmalı HD pixel-art bir top-down extraction ARPG'de birleşiyor.**

### 1.2 Elevator Pitch

2089. "Eclipse Olayı" gerçekliği yırttı ve dünyanın kalbinde fizik kurallarının çöktüğü bir bölge bıraktı: **The Zone**. Sen, dağılmış "Eclipse Protocol Unit"in hayatta kalan son operatörüsün. Her gece Zone'a giriyorsun: ganimet topla, kayıp bilim insanlarını bul, anomalinin kaynağına bir adım daha yaklaş — ve **canlı çık**. Ölürsen sırtındaki her şey Zone'un olur. Çıkarsan, üssünü büyütür, silahını modifiye eder ve bir sonraki sefere daha derine inersin. Zone seni öğrenir. Sen de onu öğrenmek zorundasın.

### 1.3 Kimlik Kartı

| Özellik | Değer |
|---|---|
| Tür | 2D High-Fidelity Pixel Art · Top-Down Action RPG · Extraction Shooter · Roguelite elementleri |
| Mod | Tek oyunculu (co-op'a hazır mimari — bkz. Bölüm 5) |
| Kamera | Top-down, mouse imlecine bağlı dinamik ofset kamera + paralaks derinlik |
| Oturum Uzunluğu | 20–45 dk (bir raid) · 5–10 dk (hub/meta yönetimi) |
| Toplam İçerik | Ana hikâye ~35 saat · %100 tamamlama 120+ saat · roguelite döngüyle teorik sonsuz |
| Fiyatlandırma | 24,99 USD premium — **mikro ödeme YOK**, kozmetik DLC + büyük içerik genişlemeleri |
| Yaş Derecelendirmesi | PEGI 18 / ESRB M (gore fizikleri, koyu tema) |
| Hedef Çıkış | Steam Early Access (12 ay) → 1.0 eşzamanlı Steam + EGS |

### 1.4 Benzersiz Satış Noktaları (USP)

1. **Öğrenilebilir recoil paternleri** — top-down bir oyunda ilk kez CS:GO sınıfı sprey kontrolü. Aim, RNG değil beceri.
2. **Extraction gerilimi, tek oyunculu paket** — Tarkov'un kalp atışı, sunucu/cheater derdi olmadan.
3. **HD-2D görsel kimlik** — normal-map'li pixel sprite'lar üzerinde gerçek zamanlı 2D global illumination; "yaşayan diorama" hissi.
4. **Zone Shift** — haftada bir haritaların anomali/loot/düşman yerleşimi prosedürel olarak "kayar"; meta asla bayatlamaz.
5. **CRPG derinliğinde diyalog** — stat ve fraksiyon repütasyonuna bağlı skill-check'li dallanan diyaloglar; 5 farklı son.
6. **Gerçek PC-first tasarım** — kilitsiz FPS, raw input, 21:9/32:9 native UI, DirectStorage ile 1-2 sn harita geçişi.
7. **Tarkov-sınıfı Gunsmith** — 150+ silah, 7 slot modifikasyon, balistik simülasyon (bkz. Bölüm 3).
8. **1. günden mod desteği** — Steam Workshop: silah, harita, görev ve total conversion API'si.

### 1.5 Hedef Kitle ve Pazar Konumlandırması

**Birincil kitle (Core):** 22–38 yaş, PC-native, Tarkov/Hunt: Showdown gerilimini seven ama PvP toksisitesinden ve zaman baskısından yorulmuş oyuncular. Zero Sievert, Darkwood, Dead Cells kütüphanesi olanlar.

**İkincil kitle:** Pixel-art estetiği ve derin hikâye arayan indie oyuncuları (Hyper Light Drifter, Signal From Tölva, Disco Elysium kitlesiyle kesişim).

**Benchmark Analizi:**

| Oyun | Satış (yaklaşık) | Aldığımız Ders | Kaçındığımız Hata |
|---|---|---|---|
| Zero Sievert | 1M+ | Tek oyunculu extraction talebi kanıtlanmış | Sığ hikâye, tekrarlayan haritalar |
| Dead Cells | 10M+ | "Game feel" her şeydir; canlı yol haritası satışı 5 yıl taşır | — |
| Escape from Tarkov | 15M+ hesap | Gunsmith + kayıp korkusu = bağımlılık | Cheater/sunucu sorunları, hardcore-only erişilebilirlik |
| Darkwood | 1.5M+ | Top-down'da korku/gerilim çalışıyor | Zayıf silah hissi |
| Enter the Gungeon | 10M+ | Bullet-hell boss okunabilirliği | Meta ilerleme sığlığı (erken dönem) |

**Konumlandırma cümlesi:** *"Tarkov'un gerilimi, tek başına. Dead Cells'in hissi, bir dünyayla."*

---

## 2. TASARIM SÜTUNLARI (DESIGN PILLARS)

Her tasarım kararı bu beş sütunla test edilir. **Bir özellik herhangi bir sütunu ihlal ediyorsa, ne kadar parlak olursa olsun reddedilir.**

### P1 — "HER MERMİ ÖNEMLİ" (Precision is Sacred)
Fare imleci oyuncunun iradesidir. Nişan alma asla RNG tarafından gasp edilmez; dağılma her zaman öğrenilebilir bir paternden gelir. Mühimmat ekonomiktir, ıskalamak acı verir, headshot ödüllendirilir.
*İhlal örneği:* "Bu silaha %15 rastgele sapma ekleyelim" → RED.

### P2 — "ZONE ASLA GÜVENLİ DEĞİL" (Tension is the Product)
Raid içinde hiçbir an %100 güvenli değildir. Loot ekranı açıkken oyun durmaz. Ses çıkarırsan duyulursun. En değerli ganimet her zaman en tehlikeli üç adımın ötesindedir.
*İhlal örneği:* "Envanter açılınca oyun dursun" → RED (erişilebilirlik modu hariç — bkz. §4.4).

### P3 — "KAYIP ANLAMLI, İLERLEME KALICI" (Loss Stings, Progress Stays)
Ölüm gerçek bir bedel taşır (sigortasız eşya kaybı) ama oyuncunun *bilgisi*, skill tree'si, hideout'u ve hikâye ilerlemesi asla silinmez. Her ölüm bir ders, her extraction bir zaferdir.

### P4 — "ANLATMA, GÖSTER" (The World is the Narrator)
Lore, cutscene'lere değil dünyaya gömülüdür: cesetlerin duruşu, terminal kayıtları, yarım kalmış bir satranç tahtası. Oyuncu hikâyeyi *kazır*.

### P5 — "PC-FIRST, TAVİZSİZ" (Built for the Desk)
Her sistem klavye+mouse hassasiyeti, yüksek Hz monitörler ve mod topluluğu düşünülerek tasarlanır. Konsol/mobil uyarlaması hiçbir PC özelliğini törpüleyemez.

---

## 3. CORE GAMEPLAY LOOP

Oyun üç iç içe döngüden oluşur: **Mikro (saniyeler) → Raid (dakikalar) → Meta (saatler)**.

### 3.1 Mikro Döngü — "Combat Heartbeat" (3–30 saniye)

```
   TESPİT ET ──► KONUMLAN ──► ANGAJE OL ──► ÇÖZ ──► YAĞMALA/İLERLE
      ▲            (siper,        (recoil       (öldür,          │
      │             flank,         kontrolü,     kaç veya        │
      │             gizlen)        yetenek,      sessiz geç)     │
      └──────────────────────── DEĞERLENDİR ◄────────────────────┘
                        (mühimmat? HP? gürültü? risk?)
```

**Tasarım hedefi:** Her çatışma bir *karar zinciridir*, refleks testi değil. Oyuncu her 10 saniyede en az bir anlamlı taktik karar vermeli (pozisyon, mühimmat tipi, yetenek, geri çekilme).

**Mikro döngü kalite ölçütleri (Game Feel KPI):**

| Ölçüt | Hedef |
|---|---|
| Input → ateş animasyonu gecikmesi | ≤ 16 ms (1 frame @ 60fps, uncapped'de daha az) |
| Düşman hit reaction | Her isabette görünür stagger/flash (8 frame) |
| Ölüm okunabilirliği | Oyuncu ölümünün nedeni her zaman ekranda görünür olmalı |
| TTK (Time-to-Kill) — normal düşman | 0.4–1.5 sn |
| TTK — oyuncu (zırhsız) | 0.8–2.0 sn (tehlike hissi için düşük) |

### 3.2 Raid Döngüsü — Orta Döngü (20–45 dakika)

Bir raid altı fazdan oluşur. Gerilim eğrisi bilinçli olarak asimetriktir: en yüksek stres **extraction anında** yaşanır (elindekini kaybetme korkusu en tepededir).

| Faz | Süre Payı | Oyuncu Aktivitesi | Gerilim | Tasarım Notu |
|---|---|---|---|---|
| 1. Briefing & Loadout | Hub'da | Görev seçimi, sigorta, çanta tetrisi, mühimmat | ▂ | "Ne kadar riske gireyim?" kararı burada verilir |
| 2. Infiltration | %10 | Giriş noktası seçimi, ilk keşif | ▃ | Her haritada 3+ giriş noktası; giriş loadout'a göre kısıtlanabilir (ağır zırh kanalizasyondan sığmaz) |
| 3. Exploration | %30 | Loot, terminal hack, anomali navigasyonu | ▄ | Ses/ışık disiplini; envanter dolmaya başlar → risk artar |
| 4. Engagement | %25 | Çatışma, mini-boss, dinamik olaylar | ▆ | AI taktikleri (bkz. Bölüm 2); kaçmak her zaman geçerli seçenek |
| 5. Objective & Deep Loot | %20 | Ana/yan görev hedefi, yüksek-tier loot odaları | ▇ | En iyi loot = en gürültülü erişim (C4, jeneratör, alarm) |
| 6. Extraction | %15 | Exfil noktasına ulaşım, bekleme, son savunma | █ | Bkz. §3.4 — oyunun imza anı |

**Dinamik olay sistemi (Raid Modifiers):** Her raid'e %0–2 arası rastgele olay enjekte edilir: anomali fırtınası (harita anomalileri yer değiştirir), avcı timi (elit AI squad oyuncuyu izler), tüccar konvoyu (fırsat), Blackout (tüm ışıklar söner). Olaylar telsizden önceden sinyallenir — sürpriz değil, *gelen fırtına* hissi.

### 3.3 Meta Döngü — Makro (oturumlar arası)

```
RAID SONUCU ──► HUB: "HAVEN" ──► SAT / STAŞLA ──► CRAFT & GUNSMITH
     ▲                                                  │
     │                                                  ▼
YENİ BÖLGE / DERİNLİK ◄── FRAKSİYON GÖREVLERİ ◄── HIDEOUT UPGRADE
     ▲                                                  │
     └────────────── SKILL TREE & OPERATÖR ◄────────────┘
```

**Meta ilerleme kanalları (5 paralel şerit):**

1. **Hikâye ilerlemesi** — ana görev zinciri, yeni bölgelerin kilidini açar.
2. **Hideout (Üs Geliştirme)** — 12 modül: Atölye, Revir, Cephanelik, Sera, Jeneratör, İstihbarat Masası, Anten, Su Arıtma, Kütüphane, Atış Poligonu, Anomali Kasası, Karaborsa Bağlantısı. Her modül pasif bonus + craft tarifi açar.
3. **Skill Tree** — 4 dal (Combat / Tactical / Psionic / Survival), operatör başına benzersiz dal (bkz. Bölüm 2).
4. **Fraksiyon repütasyonu** — 6 fraksiyon, -100 ile +100 arası; tüccar fiyatları, özel görevler, bölge geçiş hakları.
5. **Zone Bilgisi (Codex)** — keşfedilen lore, taranan anomaliler, incelenen mutant dokuları; %100 codex gizli sonun anahtarıdır.

### 3.4 Extraction Sistemi — Detay Tasarım

| Exfil Tipi | Kural | Risk Profili |
|---|---|---|
| **Sabit Nokta** | Her zaman aktif (harita başına 1) | Düşmanlar tarafından pusuya en müsait — herkes yerini bilir |
| **Koşullu Nokta** | Jeneratör yakıtı / anahtar kart / rüşvet gerektirir | Hazırlık maliyeti, ama sessiz |
| **Dinamik Nokta** | Raid sırasında telsizle duyurulur, 4 dk açık kalır | Konumu rastgele — fırsatçı |
| **Tek Kullanımlık** | Craft edilen "Sinyal Fişeği" ile istediğin açık alanda helikopter çağır | Pahalı + 90 sn boyunca TÜM haritaya konumunu duyurur |

- Extraction'da **10 saniyelik kanal süresi** vardır; hasar alırsan sıfırlanır. Son 10 saniye oyunun en gergin anıdır ve ses tasarımı bunu destekler (yükselen kalp atışı, alçalan helikopter).
- **Sigorta sistemi:** Raid öncesi eşya başına prim ödenir. Ölürsen sigortalı eşyalar 12 saat (gerçek zaman değil — 2 raid) sonra hasar görmüş hâlde geri döner. Prim, eşya değerinin %15–40'ı (Zone derinliğine göre).

### 3.5 Risk/Ödül Matrisi — Zone Derinlik Katmanları

| Katman | Bölge Örnekleri | Loot Tier | Düşman Seviyesi | Anomali Yoğunluğu | Ölüm Maliyeti (ort.) |
|---|---|---|---|---|---|
| T1 — Perimeter | Çöl Harabeleri, Terk Edilmiş Metro (dış) | Yaygın–İyi | 1–10 | Düşük | Düşük |
| T2 — Outskirts | Mega Şehir (dış), Askeri Üs | İyi–Nadir | 10–25 | Orta | Orta |
| T3 — Midzone | Kimyasal Fabrika, Radyoaktif Orman, Nükleer Santral | Nadir–Epik | 25–40 | Yüksek | Yüksek |
| T4 — Deep Zone | Yeraltı Bio-Laboratuvarı, Buzul Üssü | Epik–Efsanevi | 40–55 | Çok Yüksek | Ağır |
| T5 — The Core | Gizli Tesis, Çökmüş Yörünge İstasyonu Enkazı | Efsanevi–Artefakt | 55–70 | Ekstrem | Yıkıcı |

**Altın kural:** Bir üst katmanın *en kötü* loot'u, bir alt katmanın *en iyi* loot'undan değerlidir. Derine inmek her zaman matematiksel olarak mantıklı, psikolojik olarak korkutucudur.

### 3.6 Ölüm Cezası Modları (oyun başında seçilir, Haven'da değiştirilebilir)

| Mod | Ölünce | Hedef Kitle |
|---|---|---|
| **Story** | Loot korunur, para cezası | Hikâye odaklı oyuncular |
| **Stalker (varsayılan)** | Sigortasız eşyalar kaybolur, güvenli konteyner (2x2) korunur | Core deneyim |
| **Veteran** | Güvenli konteyner yok, sigorta primleri 2x | Tarkov gazileri |
| **Eclipse (Hardcore)** | Full loot kaybı + karakter permadeath (hideout hariç), ayrı liderlik tablosu | Streamers / %1 |

### 3.7 Retention Hook'ları

- **Günlük Görevler (3):** Fraksiyon tüccarlarından; "Kimyasal Fabrika'dan 5 mutant dokusu getir" tarzı, 15–20 dk'lık hedefler.
- **Haftalık Zone Shift:** Prosedürel remix + haftalık özel anomali eventi + liderlik tablosu (en hızlı Core extraction).
- **Aylık "Kızıl Ay":** 72 saatlik yüksek risk/ödül global etkinliği — tüm loot tier'ları +1, tüm düşmanlar elit varyant.
- **Koleksiyon sistemleri:** Silah müzesi (hideout'ta sergilenir), artefakt vitrini, codex.

### 3.8 Oyuncu Motivasyon Haritası

| Arketip | Beslendiği Sistem |
|---|---|
| Achiever | Skill tree, hideout, koleksiyonlar, liderlik tabloları |
| Explorer | 25 harita, gizli geçitler, lore terminalleri, gizli son |
| Killer | Boss fazları, recoil ustalığı, Eclipse modu |
| Storyteller | Diyalog ağaçları, fraksiyon seçimleri, 5 son |

---

## 4. ZORLUK FELSEFESİ — "HARD TO LEARN, IMPOSSIBLE TO MASTER"

### 4.1 Üç İlke

1. **Adil ölüm:** Oyuncu neden öldüğünü her zaman bilmeli. Ekran dışından tek atımlık hasar YASAK; her ölümcül tehdit ses, ışık veya telegraf animasyonuyla önceden sinyallenir.
2. **Bilgi = güç:** Zorluğun ana kaynağı istatistik şişirme değil, *bilgi eksikliğidir*. Anomali paternleri, düşman rutinleri, harita kestirmeleri öğrenildikçe oyun "kolaylaşır" — sayılar değişmeden.
3. **Ustalık tavanı yok:** Recoil kontrolü, hareket tekniği (slide-cancel, siper peek'i), rota optimizasyonu ve ekonomi yönetimi — her sistemin skill tavanı bilinçli olarak erişilemez yükseklikte.

### 4.2 Onboarding — İlk 15 Dakika (kritik)

| Dakika | Deneyim |
|---|---|
| 0–2 | Cold open: Eclipse Olayı'nın flashback'i — oynanabilir, kaybedilmesi imkânsız, spektaküler |
| 2–6 | İlk raid (scripted T1 mini-harita): hareket, nişan, ilk öldürme, ilk loot |
| 6–10 | İlk anomali ölümü (bilinçli tasarım: ucuz ama *öğretici* — cıvata fırlatma mekaniği öğretilir) |
| 10–13 | İlk extraction — kalp atışı anı yaşatılır |
| 13–15 | Haven'a varış, Dr. Evelyn'in telsiz mesajı → ana görev kancası atılır |

**Hedef:** 15. dakikada oyuncu core loop'un tamamını bir kez yaşamış ve "bir raid daha" demiş olmalı.

### 4.3 Zorluk Ayarı Yaklaşımı

Ayrı bir "kolay/zor" slider'ı yoktur; zorluk **Zone derinliği + ölüm cezası modu + loadout riski** üçgeninden doğal olarak çıkar. Oyuncu her zaman kendi risk seviyesini seçer.

### 4.4 Erişilebilirlik (zorluğu değil, engelleri kaldırır)

- Screen shake / flash yoğunluğu slider'ı, fotosensitivite modu
- Renk körlüğü paletleri (3 tip) + anomali görsel dilinin renkten bağımsız okunabilirliği (şekil + ses)
- Envanter-duraklatma opsiyonu (yalnız Story modda)
- Tam tuş atama + tek elle oynanabilir preset'ler
- İşitilebilir tehditler için görsel ses radarı (opsiyonel, varsayılan kapalı)

---

## 5. EVREN VE HİKÂYE (LORE)

### 5.1 Tema ve Ton

**Ana tema:** *"Anlamadığın şeyi sahiplenemezsin."* — Zone, insanlığın açgözlülüğüne kayıtsız, kozmik bir yara. Herkes onu kullanmak istiyor; o kimseyi umursamıyor.

**Alt temalar:** Devlet sırlarının bedeli · hayatta kalmanın ahlaki aşınması · bilimin şeytanla pazarlığı.

**Ton referansları:** *Roadside Picnic* (Strugatski kardeşler), *Annihilation*, S.T.A.L.K.E.R. serisi, *Control*, *Blade Runner 2049* (görsel melankoli).

**Ton kuralı:** Umut nadir ama gerçek. Karakterler karikatür değil; en tekinsiz NPC'nin bile anlaşılır bir gerekçesi var.

### 5.2 Zaman Çizelgesi

| Yıl | Olay |
|---|---|
| 2077 | "Prometheus Girişimi": Çok uluslu konsorsiyum, sıfır-nokta enerjisi için boyutsal membran deneylerine başlar (kamuoyundan gizli) |
| 2081 | İlk mikro-yarık kazası; 40 ölü. Örtbas edilir. Proje yeraltı tesislerine taşınır (bugünkü **Gizli Tesis** ve **Yeraltı Bio-Laboratuvarı**) |
| 2084 | "Eclipse Protocol Unit" (EPU) kurulur: yarık kazalarını kontrol altına almak için özel harekât + bilim birimi |
| 2087 | Deneyler ivmelenir; Dr. Evelyn Cross liderliğindeki etik itirazlar bastırılır |
| **2089, 14 Mart** | **ECLIPSE OLAYI:** Ana reaktör senkronizasyonu sırasında membran çöker. 30 km çapında bir alan "gerçeklik dışına" katlanır. 1.2 milyon kayıp. Gökyüzünde 6 saat süren yapay güneş tutulması — olayın adı buradan gelir |
| 2089–2093 | Ordu Zone'u karantinaya alır (**Perimeter Duvarı**). İçeriden sızan artefaktlar karaborsayı doğurur; ilk Stalker'lar duvarı delmeye başlar |
| 2094 (OYUN BAŞLANGICI) | EPU resmi olarak lağvedilmiştir. Son operatör — **sen** — merkeze doğru genişlemeye başlayan Zone'u durdurabilecek tek istihbarata sahipsin |

### 5.3 Zone'un Anatomisi

Zone, merkezden (**The Core**) dışa doğru halkalar hâlinde örgütlenir. Merkeze yaklaştıkça fizik kuralları bozulur:

| Halka | Gerçeklik Bütünlüğü | Gözlemlenen Fenomen |
|---|---|---|
| Perimeter (T1) | %95 | Hafif radyasyon, izole anomali cepleri |
| Outskirts (T2) | %85 | Elektronik parazit, mutasyon başlangıcı, "yankı" hayaletleri (geçmiş olayların görsel kayıtları) |
| Midzone (T3) | %70 | Yerçekimi cepleri, zaman genleşmesi (bazı odalarda saatler farklı akar), agresif flora |
| Deep Zone (T4) | %50 | Mekân katlanması (koridorlar kendine bağlanır), Psionic varlıklar, "The Source"un fısıltıları |
| The Core (T5) | %20 | Kurallar askıda. Bkz. Bölüm 4 (harita tasarımı) — spoiler seviyesi maksimum |

**Anomali ön izlemesi** (tam katalog Bölüm 4'te):

| Anomali | Tehlike | Karşı Taktik | Ödül |
|---|---|---|---|
| Görünmez Radyasyon Cebi | DoT hasarı, ekipman aşınması | Geiger sesi + cıvata fırlatma | İçinde genelde artefakt |
| Yerçekimi Kuyusu | Çekip ezer (anında ölüm) | Toz/duman görsel ipucu | Havada asılı loot |
| Zaman Kırığı | İçindeyken dış dünya 10x hızlı akar | Saat UI'ı bozulur (sinyal) | Güvenli "dondurulmuş" saklanma |
| Ayna Perdesi | Mermileri geri yansıtır | Yakın dövüş / dolanma | Arkasında kestirme |
| Fısıltı Alanı | Psionic hasar, ekran distorsiyonu | Odak stat'ı direnci | Psionic craft malzemesi |
| Kızıl Sarmaşık | Temasta yakalar, mutant çağırır | Ateşle temizlenir | Yanınca nadir reçine düşürür |

### 5.4 Ana Karakter: OPERATÖR "KESTREL"

- **Kimlik:** Eski EPU Black Ops operatörü. Kod adı Kestrel (oyuncu gerçek ismi ve operatör sınıfını kendi seçer — bkz. Bölüm 2, 12 sınıf).
- **Motivasyon (katmanlı):**
  1. *Yüzeysel:* Hayatta kalmak ve artefakt ekonomisinde borcunu kapatmak (Broker Kane'e olan borç — oyunun ekonomi tutorial'ının diegetik gerekçesi).
  2. *Kişisel:* Eclipse Olayı gecesi içeride kalan EPU bilim ekibini — özellikle akıl hocası **Dr. Evelyn Cross**'u — bulmak.
  3. *Varoluşsal:* Kestrel, olay gecesi Core'a en yakın hayatta kalan kişidir ve Zone onu "hatırlamaktadır" (Psionic düşmanlar Kestrel'e farklı tepki verir — oynanışa yansıyan lore).
- **Anlatı ilkesi:** Amnezi klişesi YOK. Kestrel ne olduğunu biliyor; oyuncunun keşfettiği şey *neden* olduğu ve *kimin* izin verdiğidir.
- **Ses/karakterizasyon:** Az konuşur; diyalog seçenekleri kişiliği oyuncuya bırakır (üç ton: Profesyonel / Alaycı / Tehditkâr — seçimler NPC güven sistemine işler).

### 5.5 Ana Hikâye Yapısı — Üç Perde, 14 Ana Görev

Ana görev zinciri: **"ECLIPSE'İN KAYNAĞI"**

#### PERDE I — "DUVARIN İÇİ" (Görev 1–4, T1–T2 bölgeleri, ~8 saat)

| # | Görev | Özet | Kilit Mekanik Tanıtımı |
|---|---|---|---|
| 1 | Son Operatör | Haven'a varış; Broker Kane'le borç anlaşması; ilk resmi raid | Core loop, sigorta sistemi |
| 2 | Kayıp Bilim İnsanları | Terk Edilmiş Metro'da EPU kaçış kapsülünün izini sür → Dr. Evelyn'in hayatta olduğuna dair kanıt | Terminal hack, lore sistemi |
| 3 | Veri Merkezi | Mega Şehir'deki üç sunucu çekirdeğini topla (2/3 yeterli — rota seçimi) | Çoklu-çözüm görev tasarımı |
| 4 | **Dr. Evelyn'i Kurtar** | Askeri Üs'te tutulan Evelyn'i çıkar — ilk eskort-extraction hibrit görevi. **Perde finali boss: THE HARVESTER** | Boss fazları, eskort AI |

*Perde I dönüm noktası:* Evelyn Haven'a katılır (hub NPC olur) ve gerçeği açıklar: Eclipse bir kaza değildi. Biri senkronizasyonu **bilerek** bozdu.

#### PERDE II — "DERİNE İNİŞ" (Görev 5–10, T3–T4, ~15 saat)

| # | Görev | Özet |
|---|---|---|
| 5 | Kimyasal İz | Kimyasal Fabrika'da sabotajcının kullandığı bileşiğin kaynağını bul |
| 6 | Sessiz Orman | Radyoaktif Orman'da EPU'nun kayıp 2. timinin akıbeti (tamamı environmental storytelling — tek satır diyalog yok) |
| 7 | Çekirdek Erimesi | Nükleer Santral'i stabilize et VEYA fraksiyonlardan birine sat (büyük seçim — bölge kalıcı değişir) |
| 8 | Soğuk Depo | Buzul Üssü'nde dondurulmuş "Sıfır Numune"yi ele geçir. **Boss: COLOSSUS UNIT** |
| 9 | **İHANET** | Kanıt zinciri Haven'daki birini gösterir. Oyuncu suçlayacağı kişiyi seçer — yanlış seçim kalıcı sonuçlar doğurur (NPC ölümü / tüccar kaybı) |
| 10 | Maskeler Düşerken | Gerçek sabotajcı: **Mysterious Man** — Prometheus Girişimi'nin baş mimarı ve "Children of the Eclipse" kültünün gizli lideri. **Boss: VOID STALKER** (kaçar — ilk karşılaşma kazanılamaz olarak tasarlanmıştır ama bunu oyuncuya hissettirmeden) |

#### PERDE III — "ÇEKİRDEK" (Görev 11–14, T5, ~7 saat + final)

| # | Görev | Özet |
|---|---|---|
| 11 | Gizli Tesis | Prometheus'un orijinal laboratuvarına sız; membran anahtarını craft etmek için 3 bileşen |
| 12 | Yörünge Enkazı | Çökmüş istasyonda sıfır yerçekimi bölümü; **Boss: APEX PREDATOR** |
| 13 | Son Sürgün | Mysterious Man ile final hesaplaşması (Void Stalker rövanşı — bu kez tam güçle) |
| 14 | **THE SOURCE** | Core'a iniş. Final boss: **THE SOURCE** — 4 fazlı, bullet-hell + anomali arena hibridi. Ardından FİNAL SEÇİMİ |

### 5.6 Sonlar (5 adet)

Final seçimi + oyun boyu biriken **Güven (Trust)** ve **Tehdit (Threat)** eksenleri + fraksiyon durumları sonucu belirler:

| Son | Koşul | Sonuç |
|---|---|---|
| **MÜHÜRLE** | Evelyn hayatta + membran anahtarı tam | Zone kapanır; dünya kurtulur ama artefaktlar/anomali güçleri sonsuza dek yiter. Buruk-umutlu |
| **KULLAN** | Broker Kane hattı + yüksek Threat | Kestrel Core'u kontrol eder; Zone bir silaha dönüşür. Güç fantezisi, karanlık epilog |
| **YOK ET** | Ordu (Command) fraksiyonu + Santral görevinde stabilizasyon | Nükleer çözüm: Zone buharlaşır — çevresindeki her şeyle birlikte. Pyrrhic zafer |
| **BİRLEŞ** | Kült hattı VEYA yüksek Psionic build + düşük Trust | Kestrel membrandan geçer. Kozmik-belirsiz son |
| **★ GİZLİ: "KAYNAK KODU"** | %100 Codex + 6 fraksiyonun tamamında pozitif repütasyon + gizli görev zinciri | The Source'un gerçek doğası açıklanır (Zone'un bilinçli olduğu ve *yardım istediği* ortaya çıkar). Tek gerçek "iyi son" — NG+ kilidini açar |

Tüm sonlar **New Game+**'a bağlanır: NG+'da Zone Shift kalıcı aktif, düşman kompozisyonları remix, özel NG+ artefaktları.

### 5.7 Fraksiyonlar (6)

| Fraksiyon | İdeoloji | Hub Rolü | Görev Tipi | Doğal Düşmanı |
|---|---|---|---|---|
| **The Wardens** | Zone'u karantinada tutmak; kimse girmemeli | Zırh/savunma ekipmanı tüccarı | Bölge temizliği, mühürleme | Broker's Guild |
| **Broker's Guild** (lider: Kane) | Zone bir pazardır; her şeyin fiyatı var | Karaborsa, sigorta, nadir loot alımı | Kaçakçılık, ele geçirme | The Wardens |
| **Children of the Eclipse** | Zone kutsaldır; Eclipse bir "doğum"du | Psionic craft malzemeleri (gizli tüccar) | Artefakt hasadı, hac görevleri | Command |
| **Command** (EPU/ordu kalıntısı, lider: Captain Raymond) | Devlet sırrını koru, Zone'u silahlaştır | Askeri sınıf silah/mühimmat | Kayıt imhası, hedef eliminasyonu | Children of the Eclipse |
| **Free Stalkers** (temsilci: Lira) | Bağımsız yağmacılar birliği; kimseye hesap vermeyiz | Görev panosu, harita istihbaratı, rehberler | Kurtarma, keşif, rekabetçi loot | Duruma göre herkes |
| **MOTHER** | Yeraltı Bio-Lab'ının kurtulan yönetim AI'ı; "çocuklarını" (mutantları) koruyor | Benzersiz: mutant bölgelerinden güvenli geçiş protokolleri satar | Etik açıdan rahatsız edici bilim görevleri | İnsanlığın geri kalanı |

**Repütasyon mekaniği:** -100…+100. Karşıt fraksiyonlarda kazanç/kayıp bağlıdır (Wardens +10 → Guild -5). +75 üstü "Müttefik" statüsü: özel silah, hideout modülü ve son varyantı açar. -75 altı: o fraksiyonun devriyeleri raid'lerde oyuncuya saldırır (dinamik dünya tepkisi).

### 5.8 Ana NPC Kadrosu (Haven Hub'ı)

| NPC | Rol | Hub İşlevi | Kişisel Görev Zinciri | Sırrı |
|---|---|---|---|---|
| **Dr. Evelyn Cross** | Baş bilim insanı, akıl hocası | Artefakt analizi, Psionic craft | "Etik Kurulu" (3 görev) | Prometheus'un etik raporunu imzalamıştı — suçluluk duygusuyla yaşıyor |
| **Captain Raymond** | Command irtibat subayı | Askeri görev panosu, ağır silah ruhsatları | "Zincir Komuta" (4 görev) | Eclipse gecesi tahliye emrini geciktiren subay — 1.2M ölümün vicdani yükü |
| **Lira** | Free Stalker rehberi | Harita istihbaratı, keşfedilmemiş bölge koordinatları | "Kayıp Kardeş" (3 görev) | Kardeşi bir "yankı" olarak Zone'da hâlâ görünüyor |
| **Broker Kane** | Karaborsa patronu | Sigorta, nadir eşya müzayedesi, borç sistemi | "Faiz" (3 görev) | Zone dışındaki hükümetlere artefakt satıyor — oyuncu bunu koz olarak kullanabilir |
| **The Technician** | İsimsiz silah ustası | Gunsmith tezgâhı, silah craft | "İmza" (3 görev — her biri efsanevi silah parçası) | Eski Prometheus mühendisi; Mysterious Man'i tanıyor |
| **Mysterious Man** | ??? | Perde I–II'de rastgele raid'lerde beliren tüccar (imkânsız derecede iyi mallar, tuhaf fiyatlar: "bir anı", "bir isim") | — | Ana antagonist. Ondan alışveriş yapmak gizli son zincirinin parçası VE Threat ekseni maliyeti |

(60+ ikincil NPC'nin tam listesi, rutin tabloları ve diyalog ağacı şablonları → Bölüm 2.)

### 5.9 Anlatı Sistemleri

**1. Terminal Hack & Lore Veritabanı**
- Hedef: 300+ okunabilir metin (e-posta zincirleri, güvenlik kayıtları, kişisel günlükler).
- Hack mini-oyunu: 5–15 saniyelik devre yeniden yönlendirme puzzle'ı (mouse hassasiyeti temelli, skill'le atlanabilir).
- Kural: Hiçbir lore metni 150 kelimeyi geçemez. Uzun hikâyeler zincirlenmiş kısa kayıtlarla anlatılır ("bölünmüş belgesel" tekniği).

**2. Environmental Storytelling Kuralları**
- Her el yapımı odada bir "sahne" olmalı: *ne oldu burada?* sorusuna 10 saniyede sezgisel cevap.
- "Yankı" sistemi (T2+): geçmiş olayların yarı saydam holografik tekrarları — sesli anlatım olmadan sahne anlatımı.
- Ceset yerleşimi, kan izleri ve barikat yönleri seviye tasarım dilinin parçasıdır (Bölüm 4'te stil rehberi).

**3. Diyalog Ağacı Mekaniği (CRPG katmanı)**
- Skill-check'ler görünürdür: `[ZEKA 14] Reaktör şemasındaki hatayı göster` / `[COMMAND +50] Rütbeni hatırlat`.
- Başarısız check'ler kapı kapatmaz, *farklı* kapı açar (başarısızlık = içerik, ceza değil).
- İki eksenli sosyal sistem: **Trust** (NPC'ler sana güveniyor mu?) ve **Threat** (senden korkuyorlar mı?). İkisi de kapı açar; ikisi aynı anda yüksek olamaz.

**4. Seçim Kalıcılığı**
- Perde II'deki Santral kararı gibi 6 "dünya durumu" seçimi harita düzeyinde kalıcı sonuç üretir (bölge erişimi, tüccar stoku, düşman kompozisyonu).
- Otomatik kayıt tek slot + raid başı snapshot: seçim geri alınamaz (save-scum tasarımla engellenir, Story modda esnetilir).

### 5.10 Lokalizasyon ve Seslendirme Hedefi

| Kapsam | Hedef |
|---|---|
| Metin lokalizasyonu (1.0) | TR, EN, DE, FR, ES, PT-BR, RU, ZH-Hans, JA, KO |
| Seslendirme | EN tam VO (ana görevler + hub NPC'leri); diğer diller altyazı |
| Metin hacmi tahmini | ~450.000 kelime (diyalog 250K + lore 100K + UI/sistem 100K) |

---

## 6. DENEYİM HEDEFLERİ (EXPERIENCE GOALS)

| Oyuncu şunu hissetmeli... | Bunu sağlayan sistem |
|---|---|
| "Elim titriyor ama çıkışa 50 metre kaldı" | Extraction kanal süresi + ses tasarımı + sigortasız loot |
| "Bu ölüm benim hatamdı, bir daha olmaz" | Adil ölüm ilkesi, telegraf kuralları, ölüm tekrarı ekranı (killcam verisi) |
| "Bu spreyi artık ezbere biliyorum" | Deterministik recoil paternleri + poligon modülü |
| "Bu odada ne yaşanmış böyle?" | Environmental storytelling + yankı sistemi |
| "Bir raid daha, sonra kapatacağım" (kapatamaz) | 20–45 dk oturum yapısı + günlük görev + sigorta dönüş zamanlayıcısı |

---

## 7. BAŞARI METRİKLERİ (KPI)

| Metrik | Hedef |
|---|---|
| Steam wishlist (çıkış öncesi) | 250.000+ |
| İlk yıl satış | 1.000.000+ |
| Steam inceleme skoru | %92+ ("Overwhelmingly Positive" eşiği) |
| Medyan oturum süresi | 75+ dk |
| D7 retention (EA) | %35+ |
| Refund oranı | < %8 |
| Workshop içerik sayısı (1.0 + 6 ay) | 5.000+ öğe |

---

## 8. SONRAKİ BÖLÜMLER — YOL HARİTASI

| Bölüm | İçerik | Durum |
|---|---|---|
| **Bölüm 2: Karakterler, Düşmanlar ve AI** | 12 operatör sınıfı + skill tree'ler, 60+ NPC rutin sistemi, 120+ düşman taksonomisi, davranış ağacı (BT) mimarisi, 15 ana boss faz tasarımları | ✅ Tamamlandı |
| **Bölüm 3: Silahlar, Balistik ve Gunsmith** | 150+ silah istatistik tabloları, recoil patern matematiği, mühimmat tipleri, 7-slot modifikasyon sistemi, game feel spesifikasyonları | ✅ Tamamlandı |
| **Bölüm 4: Harita ve Dünya Tasarımı** | 25 bölgenin tam dökümü, anomali kataloğu, yıkılabilir çevre teknolojisi, Zone Shift prosedürel sistemi, boss arenaları | ✅ Tamamlandı |
| **Bölüm 5: Teknik GDD ve Sistemler** | Motor mimarisi, DX12/Vulkan, DirectStorage, envanter/ekonomi/crafting sayısal modelleri, UI/UX spesifikasyonları, ses motoru, Steam/mod entegrasyonu | ✅ Tamamlandı |

---

*— BÖLÜM 1 SONU —*

---

# BÖLÜM 2: KARAKTERLER, DÜŞMANLAR VE YAPAY ZEKÂ (AI)

---

## 1. KARAKTER İSTATİSTİK SİSTEMİ (CORE STATS)

### 1.1 Altı Ana İstatistik

Tüm operatörler, NPC'ler ve düşmanlar aynı stat modelini paylaşır (tek simülasyon dili — mod desteği için kritik). Aralık: 1–30. Başlangıç dağılımı operatör sınıfına göre değişir; seviye atlamada otomatik artış yoktur, stat puanları skill tree ve implant'lardan gelir.

| Stat | Etkilediği Sistemler | Puan Başına Değer |
|---|---|---|
| **GÜÇ (STR)** | Yakın dövüş hasarı, taşıma kapasitesi, ağır silah recoil direnci, kapı kırma | +%4 melee hasar · +4 kg taşıma · -%1.5 ağır silah sway |
| **ÇEVİKLİK (AGI)** | Hareket hızı, dodge (yuvarlanma) mesafesi/iframe, sessiz hareket, silah değiştirme hızı | +%1.5 hız · +%2 dodge şansı · -%2 ayak sesi yarıçapı |
| **DAYANIKLILIK (END)** | Max HP, stamina, kanama/zehir direnci, radyasyon toleransı | +12 HP · +5 stamina · +%2 DoT direnci |
| **ZEKA (INT)** | Hack hızı, craft kalitesi, XP kazanımı, diyalog skill-check'leri | -%3 hack süresi · +%2 XP · +%1 craft kritik şansı |
| **ODAK (FOC)** | Enerji (yetenek kaynağı), Psionic direnç, nişan stabilitesi, kritik hasar | +8 Enerji · +%2 Psionic direnç · +%2 kritik hasar |
| **ŞANS (LCK)** | Loot kalitesi zar ağırlığı, kritik şans, anomali artefakt drop'u, tüccar "özel stok" şansı | +%0.5 kritik şans · +%1 nadir loot ağırlığı |

### 1.2 Türetilmiş İstatistikler

| Türetilmiş Stat | Formül (temel) | Not |
|---|---|---|
| HP | 100 + (END × 12) + skill bonusları | Zırhtan bağımsız |
| Enerji | 50 + (FOC × 8) | Aktif yeteneklerin kaynağı; çatışma dışında yenilenir |
| Stamina | 100 + (END × 5) | Sprint, dodge, melee tüketir |
| Taşıma Kapasitesi | 40 kg + (STR × 4) | Aşımda kademeli yavaşlama (bkz. Bölüm 5, envanter) |
| Kritik Şans | %3 taban + LCK + silah + mod | Cap: %35 |
| Kritik Hasar | ×1.5 taban + FOC bonusu | Cap: ×3.0 |
| Dodge (i-frame) | 0.25 sn taban + AGI bonusu | Cap: 0.45 sn |
| Zırh | Tamamen ekipmandan (bkz. Bölüm 3) | Stat'tan zırh gelmez — loot değerini korur |

### 1.3 Seviye ve XP Eğrisi

- Seviye aralığı: **1–70**. XP eğrisi: `XP(n) = 500 × n^1.85` (70. seviye toplamı ≈ 4.1M XP; ana hikâye + yan içerikle ~doğal seviye 55).
- Her seviye: **1 skill puanı**. Ana görev kilometre taşları ve bazı fraksiyon görevleri ek puan verir (toplam havuz: 92 puan — tüm ağacın ~%40'ı; build kimliği zorunlu).
- **Respec:** Haven Revir modülünde "Nöral Sıfırlama" — maliyet her kullanımda artar (5K → 15K → 45K kredi, haftalık sıfırlanır).

---

## 2. OYNANABILIR OPERATÖRLER (12 SINIF)

### 2.1 Tasarım İlkeleri

1. **Sınıf ≠ kilit:** Her operatör her silahı kullanabilir; sınıf, *nasıl* oynadığını değiştirir (yetenek + pasif + benzersiz dal), *neyle* oynadığını değil.
2. **Yetenek ≠ ultimate spam:** Aktif yetenekler Enerji tüketir ve çatışma başına 1–3 kez kullanılır; oyun bir hero-shooter değil, yetenekler taktik araçtır.
3. **Asimetrik zorluk:** Her sınıfın belirtilmiş bir ustalık eğrisi vardır; UI'da yıldızla gösterilir.

### 2.2 Operatör Matrisi

| # | Operatör | Rol | Zorluk | Stat Eğilimi | Aktif Yetenek | Pasif |
|---|---|---|---|---|---|---|
| 1 | **SENTINEL** | Hat tutucu / dengeli | ★☆☆ | END+STR | *Bastion Protokolü:* 6 sn taşınabilir enerji siperi kurar (yön seçimli) | Siper arkasında recoil -%20 |
| 2 | **JUGGERNAUT** | Ağır zırh / breach | ★★☆ | STR+END | *Hız Treni:* 3 sn durdurulamaz sprint — kapıları/barikatları kırar, çarptığını devirir | Ağır zırh hareket cezası yarıya iner; dodge YOK (tek dodge'suz sınıf) |
| 3 | **ASSASSIN** | Yakın menzil burst | ★★★ | AGI+LCK | *Gölge Adımı:* İmleç konumuna 8 m faz-atlama (duvardan geçmez), çıkışta 1 sn kritik garantisi | Arkadan saldırılar +%100 hasar |
| 4 | **GHOST** | Gizlilik / infiltrasyon | ★★★ | AGI+FOC | *Optik Kamuflaj:* 8 sn yarı görünmezlik (ateş edince bozulur), Enerji'yi tamamen tüketir | Çömelme hareketi ses üretmez; tespit göstergesi %30 yavaş dolar |
| 5 | **ENGINEER** | Alan kontrolü / kurulum | ★★☆ | INT+END | *Sentry Mk.II:* Yönlendirilebilir taret kurar (30 sn / 200 mermi) | Tuzak ve turret'ler +%50 dayanıklılık; hurda loot'u ×2 |
| 6 | **TECHIE** | Hack / drone / EMP | ★★★ | INT+LCK | *Hijack:* Menzildeki mekanik düşmanı 12 sn kendi tarafına çevirir (boss hariç) | Terminaller mini-oyunsuz açılır; drone yoldaşı (keşif modu) |
| 7 | **MEDIC** | Sürdürülebilirlik / kimya | ★☆☆ | END+INT | *Stim Sahası:* 4 m yarıçapta 6 sn/sn iyileşme bulutu (co-op hazır) | Medikal eşyalar %40 hızlı kullanılır; kanama otomatik durur |
| 8 | **HUNTER** | İzleme / uzun menzil | ★★☆ | FOC+AGI | *İşaretleme Atışı:* Vurulan hedef 10 sn duvar arkasından görünür + aldığı hasar +%15 | Mutant izleri ve loot yönü mini-haritada belirir; DMR/Sniper sway -%30 |
| 9 | **PSIONIC** | Anomali güçleri / risk-ödül | ★★★ | FOC+LCK | *Rezonans Dalgası:* Konik telekinetik itme — mermileri de saptırır (Ayna Perdesi mekaniği elde) | Anomalilerden %50 az hasar; HP yerine Enerji ile "aşırı yükleme" yapabilir |
| 10 | **RONIN** | Saf yakın dövüş | ★★★ | STR+AGI | *Iaijutsu:* 0.3 sn içinde 3 hedefe zincir kesme (mükemmel zamanlama = mermi savurma) | Melee öldürmeler stamina + %10 HP iade eder; kılıçla mermi savuşturma (timing) |
| 11 | **SCAVENGER** | Ekonomi / yük | ★☆☆ | LCK+STR | *Altıncı His:* 15 sn — duvar arkasındaki konteyner/gizli geçit vurgusu | +16 kg taşıma; sigorta primleri -%25; extraction kanal süresi -2 sn |
| 12 | **CHEMIST** | Alan reddi / DoT | ★★☆ | INT+FOC | *Katalizör Bombası:* Seçilebilir kimya (yangın/asit/duman/donma) fırlatır — zemin yüzeyleriyle reaksiyona girer | Kendi kimyasallarından etkilenmez; mutant dokusu drop'u ×2 |

### 2.3 Sınıf Kimlik Detayı (örnek şablon — tam 12'li set ek dokümanda aynı formatta)

Aşağıdaki üç örnek, her sınıf için doldurulan standart şablonu gösterir:

**GHOST — "Görünmeyen"**
- **Fantezi:** "Kimse öldüğümü bilmeden girdim ve çıktım."
- **Başlangıç loadout'u:** Susturuculu VSK Tabanca, Combat Knife, 2× Duman, hafif zırh (Tier-C).
- **Benzersiz skill dalı (10 node):** Sessiz eliminasyon zinciri → ceset taşıma → kilit açma ustalığı → capstone: *"Hayalet Protokolü"* (öldürme sonrası 2 sn görünmezlik, cooldown'suz).
- **Zayıflık (bilinçli):** Düşük HP havuzu (END başlangıcı 4/30); açık alan çatışmasında en kırılgan sınıf. Blackout eventi Ghost için avantaj, anomali fırtınası (görsel parazit görünmezliği bozar) dezavantajdır.

**PSIONIC — "Yankı Taşıyan"**
- **Fantezi:** "Zone'un gücünü Zone'a karşı kullanıyorum — bedelini bile bile."
- **Aşırı Yükleme mekaniği:** Enerji bitince yetenekler HP harcayarak kullanılabilir (%150 etki). Ölüm riski oyuncunun elindedir — P1 sütunuyla uyumlu: risk her zaman *seçimdir*.
- **Benzersiz dal capstone:** *"Membran Görüşü"* — anomaliler çıplak gözle görünür (Geiger/cıvata gerekmez).
- **Zayıflık:** Psionic yetenekler ses üretmese de mutantları *çeker* (Fısıltı mekaniği); Psionic build gürültüsüz ama "kokulu"dur.

**JUGGERNAUT — "Duvar"**
- **Fantezi:** "Kapı mı? Ben kapıyım."
- **Dodge'suz tasarım:** Tek kaçınmasız sınıf — hasar *yemek* üzerine kuruludur (zırh plakası sistemi, Bölüm 3). Yüksek Hz nişan becerisi yerine pozisyon oyunu öğretir; klavye-mouse'a yeni başlayanlar için de okunabilir.
- **Kısıt:** Dar giriş noktalarını (kanalizasyon, havalandırma) kullanamaz → rota planlaması sınıf seçiminde başlar (Bölüm 1 §3.2 Infiltration ile bağ).

### 2.4 Skill Tree Mimarisi

**Yapı:** 4 ortak ağaç + 1 sınıf dalı. Toplam 100 ortak node + 12×10 sınıf node'u.

| Ağaç | Node | Odak | Örnek Capstone |
|---|---|---|---|
| **COMBAT** (25) | Silah ustalıkları, recoil, kritik | Silah sınıfı başına uzmanlık zincirleri | *"Dead Hand":* Şarjördeki son mermi +%80 hasar |
| **TACTICAL** (25) | Gadget, hareket, siper, keşif | Slide-cancel, hızlı peek, fırlatılabilir ustalığı | *"Satranç Ustası":* İşaretlenen düşmanların görüş konileri görünür |
| **PSIONIC** (20) | Anomali etkileşimi, Enerji, direnç | Tüm sınıflara açık (Psionic sınıfı 2× verim alır) | *"Simbiyoz":* Artefakt taşıma slotu +1 |
| **SURVIVAL** (20) | HP, DoT direnci, iyileşme, ekonomi | Radyasyon/kimyasal adaptasyon | *"Zone'un Evladı":* Radyasyon HP yakmak yerine Enerji doldurur |

**Kurallar:**
- Node maliyeti kademeli (1/1/2/2/3 puan); ağaç derinliği kilidi yok — yatay geçiş serbest (build çeşitliliği).
- **Perk sistemi (ayrı katman):** "Nöral Çip" adlı loot eşyaları 3 perk slotuna takılır — skill tree kalıcı, perkler *loot'tur* (ölünce sigortasızsa kaybedilir → P3 sütunu: kayıp anlamlı, ilerleme kalıcı).
- Örnek perkler: *Maratoncu* (sprint stamina -%25), *Cerrah* (kritikler kanama açar), *Paranoyak* (arkadan yaklaşan düşmanda kalp atışı sesi).

---

## 3. NPC SİSTEMİ (60+ KARAKTER)

### 3.1 Haven Hub Yerleşimi

Haven, Perimeter Duvarı'na gömülü eski bir metro istasyonudur. 5 bölge: **Pazar Katı** (tüccarlar), **Atölye Koridoru** (Technician, craft), **Revir** (Medic NPC'leri, respec), **Komuta Odası** (görev panoları, fraksiyon temsilcileri), **Barınak** (ikincil NPC'ler, bar, söylenti sistemi).

### 3.2 NPC Kategorileri

| Kategori | Adet | İşlev |
|---|---|---|
| Ana kadro (Bölüm 1 §5.8) | 6 | Hikâye + benzersiz hizmetler |
| Tüccarlar | 9 | Fraksiyon başına 1 + 3 bağımsız (silah/zırh/tüketilebilir) |
| Hizmet NPC'leri | 7 | Berber (kozmetik), sigortacı, poligon hocası, aşçı (buff yemekleri), arşivci (codex), radyocu (event duyuruları), rehber |
| Görev NPC'leri | 22 | Yan görev zincirlerinin sahipleri; çoğu raid'lerde kurtarılarak Haven'a katılır (**hub büyümesi = oyuncu başarısının görsel kanıtı**) |
| Ambiyans NPC'leri | 16+ | Bar müdavimleri, mülteciler, çocuklar — söylenti/bark üreticileri |

### 3.3 Rutin Sistemi (Gündüz/Gece Döngüsü)

Her adlandırılmış NPC'nin 24 saatlik çizelgesi vardır (oyun içi 1 gün = 2 saat gerçek zaman):

```
LIRA — Örnek Rutin
06:00 Komuta Odası (harita istihbaratı satışı — tam stok)
14:00 Pazar Katı'nda Kane'le tartışma (dinlenebilir diyalog — lore)
20:00 Bar (Trust > 40 ise "Kayıp Kardeş" görev diyaloğu SADECE burada açılır)
02:00 Barınak (erişilemez — "rahatsız etme")
```

- **Tasarım amacı:** Hub'ı canlı kılmak + bilgiyi zamana bağlamak. Bazı diyalog/fiyat fırsatları yalnızca belirli saat+konumda açılır (Explorer arketipi ödülü).
- Tüccar stokları her Zone Shift'te (haftalık) + her 3 raid'de yenilenir; "özel stok" LCK ve repütasyonla ağırlıklanır.

### 3.4 Söylenti (Rumor) Sistemi

Ambiyans NPC'leri, dinamik dünya durumundan üretilen bark'lar konuşur: *"Duydun mu, Nükleer Santral hâlâ ışıl ışılmış..."* (oyuncunun Perde II 7. görev seçiminin yankısı). Söylentilerin %20'si mekanik ipucudur (gizli geçit, tüccar indirimi, mini-boss konumu) — dedikodu dinlemek ödüllendirilir.

---

## 4. DÜŞMAN TAKSONOMİSİ

### 4.1 Dört Familya

| Familya | Normal Tür | Elit | Davranış Kimliği |
|---|---|---|---|
| **İNSAN** (Yağmacı çeteleri, Command devriyeleri, Kült müritleri) | 48 | 14 | Siper, flank, bastırma ateşi, taktik iletişim — "senin gibi oynarlar" |
| **MUTANT** | 36 | 10 | Sürü, koku/ses takibi, pusu, siper kavramı yok — "doğal afet gibi" |
| **MEKANİK** (Prometheus güvenlik sistemleri, çılgın droneler) | 24 | 7 | Patern-tabanlı, tarama konileri, EMP zafiyeti — "bulmaca gibi" |
| **ANOMALİ/PSİONİK** | 12 | 4 | Fizik kurallarını ihlal eder: duvardan geçme, ışınlanma, mermi bükme — "kâbus gibi" |
| **TOPLAM** | **120** | **35** | + 20 mini-boss + 15 ana boss |

### 4.2 Çekirdek Arketipler (T1–T2 örnek istatistiklerle)

| Arketip | Familya | HP | Zırh | Davranış Özeti | Zayıflık |
|---|---|---|---|---|---|
| **Soldier** | İnsan | 80 | Hafif | Temel siper kullanımı, 3'lü mangalarda | Flank'e karşı yavaş döner |
| **Rifleman** | İnsan | 70 | Yok | Orta mesafe bastırma; mermisi bitince geri çekilir | Şarjör değişimi 3 sn — cezalandır |
| **Heavy** | İnsan | 220 | Ağır plaka | LMG bastırma, yavaş dönüş | Sırt tüpü (zayıf nokta ×3 hasar) |
| **Sniper** | İnsan | 60 | Yok | Lazer izi telegrafı (1.2 sn), pozisyon değiştirir | İz görünür — P4.1 adil ölüm kuralı |
| **Suicider** | Mutant/İnsan hibrit | 50 | Yok | Çığlık atarak koşar (ses telegrafı), ölünce patlar | Bacaklar — düşünce yerinde patlar |
| **Drone** | Mekanik | 40 | Yok | Havada sinüs paterni, taramada takviye çağırır | EMP/şok mühimmatı anında düşürür |
| **Mutant Hound** | Mutant | 90 | Yok | 4'lü sürü, dairesel kuşatma, sırayla dalış | Ateş — panikleyip sürüyü dağıtır |
| **Elite Guard** | İnsan | 300 | Ağır + kalkan | Kapı tutma, kalkan duvarı, yetenek kesme EMP'si | Kalkan menteşesi; Juggernaut çarpması devirir |
| **Shock Unit** | Mekanik | 150 | Orta | Kısa teleport-benzeri dash + elektrik alanı | Dash sonrası 1.5 sn soğuma penceresi |

*(120 türün tam istatistik matrisi: Ek-B tablosu — üretimde spreadsheet olarak tutulur; bu GDD davranış kimliklerini tanımlar.)*

### 4.3 Elit Sistem (Affix Modeli)

35 elit tür = 14 el yapımı elit + **affix kombinasyon sistemi**. T3+ bölgelerde normal düşmanlar 1–2 affix ile spawn olabilir:

| Affix | Etki | Görsel Tel |
|---|---|---|
| *Zırhlı* | +plaka, zayıf nokta küçülür | Kaynaklı metal parçalar |
| *Fısıltılı* | Ölünce Psionic patlama | Mor aura |
| *Avcı* | Oyuncunun son bilinen konumuna değil, *tahmin edilen* konumuna gider | Kızıl vizör |
| *Rejeneratif* | Çatışma dışı hızlı iyileşme | Nabız atan doku |
| *Kararlı* | Stagger immünitesi | Ağır duruş animasyonu |

**Kural:** Affix her zaman siluetten/renkten okunur (angajman öncesi bilgi = adil ölüm ilkesi).

### 4.4 Tier Ölçekleme Kuralı

Düşman istatistikleri katman başına çarpanla değil **kompozisyonla** ölçeklenir: T4'te Soldier'ın HP'si T1'dekiyle aynıdır; fark, T4 devriyesinin Elite Guard + Chemist-sınıfı destek + drone gözcü içermesidir. *"Sünger düşman" yasak — zorluk sayı şişirmeden değil, taktik derinlikten gelir* (P1 sütunu).

---

## 5. YAPAY ZEKÂ MİMARİSİ

### 5.1 Katmanlı Mimari

```
ALGISAL KATMAN (Perception)          — görüş konisi, ses, ışık, koku(mutant)
        │ stimulus events
BLACKBOARD (paylaşımlı bellek)       — son bilinen konum, tehdit skoru, manga durumu
        │
UTILITY SELECTOR (karar)             — "şu an en değerli davranış hangisi?" skorlama
        │ seçilen hedef-davranış
BEHAVIOR TREE (icra)                 — siper al / flank / bastır / geri çekil alt ağaçları
        │
LOCOMOTION & STEERING                — navmesh, kalabalık kaçınma, animasyon senkronu
```

- **Neden hibrit (Utility + BT):** Saf BT dallanma patlaması yaratır; utility katmanı *ne yapılacağını* skorlar, BT *nasıl yapılacağını* icra eder. Mod API'si her iki katmanı da açar (Bölüm 5).

### 5.2 Algı Sistemi

**Görüş:**

| Parametre | Değer |
|---|---|
| İnsan FOV | 110° koni, 25 m (gündüz açık alan) |
| Işık çarpanı | Tam karanlıkta menzil ×0.3; el feneri ışığındaki oyuncu ×1.6 |
| Tespit süresi | Mesafe/ışık/duruşa göre 0.4–2.5 sn dolum (UI'da düşman üstünde göz ikonu — Ghost pasifi bunu yavaşlatır) |

**Ses (yarıçap, metre):**

| Olay | Yarıçap | Not |
|---|---|---|
| Yürüme | 4 | Zemin malzemesi çarpanı (cam kırığı ×2) |
| Sprint | 14 | |
| Susturuculu atış | 12 | |
| Susturucusuz tüfek | 60 | Kapalı alanda koridor boyunca taşınır (ses portalları) |
| Patlama / C4 | 120 | Harita çapında mutant ilgisi |
| Cıvata fırlatma | 6 (düştüğü yerde) | Dikkat dağıtma aracı — anomali testiyle aynı tuş |

**Bellek:** Tespit sonrası "son bilinen konum" 20 sn tutulur → arama paterni (genişleyen daire, insan mangalarında ikili tarama). Bulamazsa "alarmda" durumuna döner (10 dk boyunca devriye rotası sıklaşır — sızma kalıcı iz bırakır).

### 5.3 Manga (Squad) Taktikleri — İnsan Familyası

- **Rol dağıtımı:** 3–6 kişilik mangalarda dinamik roller: *Pointman* (temas arar), *Suppressor* (bastırma ateşi — isabet amaçlı değil, oyuncuyu siperde tutar), *Flanker* (bastırma sırasında kanat döner), *Leader* (komut bark'ları — **sesli olarak duyulur**: "Sağdan dolan!" duyan oyuncu karşı-oynayabilir → bark'lar süs değil, bilgi kanalıdır).
- **Saldırı jetonu (attack token):** Aynı anda en fazla 3 düşman aktif ateş eder; diğerleri pozisyon alır. Kalabalık = kaos değil, koreografi.
- **Moral sistemi:** Leader ölürse manga 5 sn bocalar (bark: "Komutan düştü!"); %50 kayıpta geri çekilme + takviye çağrısı. Mermisi biten düşman *gerçekten* şarjör değiştirir/geri çekilir — sonsuz mermi yok.
- **Sağlık davranışı:** Ağır yaralı düşman sürünerek siper arar; yoldaşı onu çekmeye çalışabilir (ahlaki mikro-an: yaralıyı bitir → Threat +, bırak → mühimmat tasarrufu).

### 5.4 Mutant AI — Stimulus Sürümü

Siper/jeton sistemi kullanmaz; **dürtü kuyruğu** ile çalışır: Açlık > Ses > Işık > Koku. Sürü lideri (alfa) hedef seçer, sürü senkron dalış paternleri uygular. Ateş, ses bombası ve Psionic fısıltı dürtü kuyruğunu manipüle edebilir → mutantlar *silah olarak kullanılabilir* (bir sürüyü insan devriyesinin üstüne çekmek geçerli ve teşvik edilen taktiktir).

### 5.5 AI Direktörü (Pacing Katmanı)

Left 4 Dead esinli üst katman; raid gerilim eğrisini izler:
- **Girdi:** Oyuncu HP'si, gürültü bütçesi, geçen süre, loot değeri, son çatışmadan bu yana geçen süre.
- **Çıktı:** Devriye rotası yoğunluğu, dinamik olay tetikleme (Bölüm 1 §3.2), ambiyans ses katmanı seçimi.
- **Kural:** Direktör asla düşman *spawn etmez* (spawn'lar seviye tasarımında deterministiktir — öğrenilebilirlik/P2); yalnızca mevcut devriyeleri yönlendirir ve zamanlamayı ayarlar.

### 5.6 Performans Bütçesi

| Katman | Limit |
|---|---|
| Tam simülasyon (ekran + 20 m) | 24 AI |
| LOD-1 (algı açık, animasyon basit) | 40 AI |
| LOD-2 (yalnız konum/rota simülasyonu) | 200 AI (harita geneli devriye persistansı) |
| BT tick oranı | Tam: 10 Hz · LOD-1: 4 Hz · LOD-2: 0.5 Hz |

### 5.7 Müttefik AI (Eskort Görevleri)

Dr. Evelyn kurtarma görevi (Perde I finali) şablonu: Müttefik NPC **asla yol tıkamaz** (çarpışma oyuncuya karşı kapalı), **kendini siperler** (oyuncu angajmanında otomatik en yakın sipere), **ölmez — düşer** (30 sn içinde kaldırılmazsa görev başarısız; sünger eskort yerine kurtarma gerilimi). Eskort hızı oyuncu hızını takip eder; "bekle/ilerle" iki komutlu telsiz menüsü (Q-menü).

---

## 6. MİNİ-BOSS'LAR (20)

Mini-boss'lar rastgele devriye eden "korku unsurlarıdır" — arena gerektirmez, kaçılabilir, öldürülürse benzersiz craft malzemesi düşürür. Seçki (tam liste Ek-C):

| Mini-Boss | Familya | Bölge | Gimmick |
|---|---|---|---|
| Broodmother | Mutant | Metro tünelleri | Ölünce 12 yavru; yuvası yakılmazsa yeniden doğar |
| Rust Titan | Mekanik | Kimyasal Fabrika | Manyetik çekim — fırlatılabilirleri havada yakalar |
| The Curator | Anomali | Mega Şehir müzesi | Işık altında donar, karanlıkta ışınlanır (Weeping Angel) |
| Alfa Sürüsü "Kızıl Diş" | Mutant | Radyoaktif Orman | 8'li koordineli sürü; alfa ölmeden sürü kaçmaz |
| Reactor Ghost | Anomali | Nükleer Santral | Yalnızca Geiger sesiyle konumlanır — görünmez |
| Twin Serpents | Mutant | Buzul Üssü | İki gövde tek HP havuzu; aynı anda 3 sn içinde vurulmalı |
| The Collector | İnsan (?) | Rastgele (T3+) | Oyuncuların ölümde düşürdüğü eşyaları toplar — öldüren, kayıp loot'ları geri kazanır |
| Warden of the Wall | Mekanik | Askeri Üs | Sabit ağır taret + drone ağı; alanı ele geçirme savaşı |

---

## 7. ANA BOSS'LAR (15)

### 7.1 Boss Tasarım Anayasası

1. Her boss **en az 3 faz**; her faz yeni mekanik ekler, öncekini iptal etmez (katmanlı öğrenme).
2. Tüm saldırılar telegraf kuralına uyar: **bullet-hell yoğunluğu ≤ ekranın %60'ı** her an güvenli koridor bırakır.
3. Arena tehlikeleri çift taraflıdır: patlayan boru boss'u da yakar.
4. Boss HP bar'ı faz eşiklerini gösterir (bilgi = güç ilkesi).
5. Ölüm → arena girişinde hızlı tekrar (loot cezası korunur ama yürüme cezası yoktur).
6. Her boss: garantili benzersiz drop (silah/artefakt) + sınıf craft malzemesi + codex girişi.

### 7.2 Hikâye Boss'ları — Faz Dökümleri

**BOSS 1: THE HARVESTER** *(Perde I Finali — Askeri Üs biyo-atık deposu)*
Prometheus'un ceset toplama otomatı, mutant dokusuyla kaynaşmış et-makine hibriti.

| Faz | HP | Mekanik |
|---|---|---|
| 1 (%100–70) | Kanca zinciri (oyuncuyu çeker — dodge zamanlaması dersi), süpürme lazer taraması | Arena: 4 vinç kolonu (siper) |
| 2 (%70–35) | + Zeminden ceset "hasat eder" → Suicider'lara dönüştürür; kolonları tek tek yıkar | Azalan siper = artan hareket baskısı |
| 3 (%35–0) | + Çekirdek açılır (zayıf nokta), öfke modunda zincir ×2; arena kenarı çöker | Bullet-hell yoğunluk tavanı: %50 |

*Ders verdiği beceri:* Dodge i-frame zamanlaması + zayıf nokta önceliklendirme. *Drop:* "Harvester's Hook" (yakın dövüş — düşman çekme).

**BOSS 2: COLOSSUS UNIT** *(Perde II — Buzul Üssü hangarı)*
Eclipse gecesi terk edilmiş prototip savaş mekanoidi; buz katmanının altında yarı gömülü uyanır.

| Faz | Mekanik |
|---|---|
| 1 | Yalnızca üst gövde: mortar yağmuru (zeminde hedef daireleri) + omuz gatling süpürmesi. Buz zemin: sprint kayması — hareket planı şart |
| 2 | Gövde buzdan kurtulur: şarj saldırısı duvarları yıkar (yeni oda açılır — arena büyür), eklem zayıf noktaları açığa çıkar |
| 3 | Reaktör aşırı yükü: ısı dalgaları buzu eritir → zemin kısımları suya döner (elektrik saldırısı + su = alan reddi zinciri). EMP mühimmatı fazı kısaltır (Techie/Chemist parlar) |

*Ders:* Alan kontrolü + mühimmat tipi seçimi. *Drop:* "Titan Core" (Juggernaut zırh craft ana bileşeni).

**BOSS 3: VOID STALKER** *(Perde II §10 — kazanılamaz ilk karşılaşma / Perde III §13 rövanş)*
Mysterious Man'in Zone-bükülmüş muhafızı; Assassin sınıfının kâbus aynası.

- **İlk karşılaşma (kaybedilecek):** 90 saniyelik hayatta kalma — öldürülemez (HP %25 altına inmez), süre dolunca kaçar. Oyuncuya *asla* "scripted yenilgi" hissettirilmez: hedef "hayatta kal" olarak verilir ve başarılabilir.
- **Rövanş (3 faz):** F1: kısa menzil faz-atlama + gölge klonlar (gerçek olan Geiger'ı öttürür — ses tabanlı hedef seçimi). F2: arena karartılır, yalnız muzzle flash ve anomali ışığı (Blackout eventinin boss versiyonu). F3: klon sayısı 3'e çıkar ama *hepsi gerçek hasar alır* — DPS yarışına dönüşen kurtuluş finali.
*Ders:* Ses tabanlı tespit + panik yönetimi. *Drop:* "Void Shard" (Gölge Adımı menzil +4 m — sınıflar arası artefakt).

**BOSS 4: APEX PREDATOR** *(Perde III — Çökmüş Yörünge İstasyonu, düşük yerçekimi)*
Zone'un evrimsel tepe noktası: istasyonun sıfır-G kesitinde avlanan akışkan formlu yırtıcı.

| Faz | Mekanik |
|---|---|
| 1 | Düşük-G arena: oyuncu zıplamaları süzülmeye döner (dodge dili değişir); Predator duvar/tavan yüzeylerinde avlanır |
| 2 | Gövde bölünmesi: 2 küçük form (biri gerçek — Hunter İşaretleme Atışı gerçeğini gösterir, sınıf sinerjisi) |
| 3 | İstasyon kabuğu delinir: dekompresyon akıntıları herkesi çeker — çevresel fizik son fazın ana silahıdır (her iki taraf için) |

*Ders:* Değişen fizik kurallarına adaptasyon. *Drop:* "Apex Membrane" (Plasma Sniper craft bileşeni).

**BOSS 5 (FİNAL): THE SOURCE** *(Görev 14 — The Core)*
Zone'un kalbi. Bir yaratık değil, *gerçekliğin enfekte olmuş bağışıklık tepkisi.*

| Faz | Tema | Mekanik |
|---|---|---|
| 1 — "İnkâr" | Bullet-hell klasiği | Radyal mermi perdeleri + anomali cepleri arena içinde döner; tüm oyun boyu öğrenilen anomali karşı-taktikleri sınanır |
| 2 — "Öfke" | Anı arenaları | Oyuncuyu geçmiş boss arenalarının "yankı" versiyonlarına çeker (Harvester/Colossus mekaniklerinin hızlandırılmış remix'i) |
| 3 — "Pazarlık" | Diyalog savaşı | Savaş ortasında gerçek zamanlı skill-check'li diyalog (dodge atarken cevap seçme) — Trust/Threat ekseni burada son kez oynanır; doğru replikler 4. fazı kısaltır |
| 4 — "Kabul" | Membran | Gerçeklik katmanları soyulur (görsel: pixel-art çözünürlüğü kademeli düşer — meta estetik an); final seçim ekranına akan oynanabilir geçiş |

*Kural istisnası:* The Source öldürülmez — final seçimi (Bölüm 1 §5.6) savaşın *nasıl* bittiğini belirler.

### 7.3 Kalan 10 Ana Boss (yan içerik / fraksiyon zincirleri)

| Boss | Bağlam | Tek Cümle Kimlik |
|---|---|---|
| The Warden Prime | Wardens zinciri finali | Duvarın otonom savunma çekirdeği — "koruduğu şeye dönüşen bekçi" |
| Kane's Debt | Broker's Guild finali | Kane'in "tahsilatçısı": Elite Guard beşlisi + zırhlı konvoy savaşı |
| The Prophet | Kült zinciri finali | Psionic kitle kontrolü — savaş alanındaki müritler kalkan/silah olarak kullanılır |
| General ARES | Command zinciri finali | Komuta bunker'ında savaş: taret ağı + manga dalgaları + gaz protokolü |
| MOTHER'ın Bahçesi | MOTHER zinciri finali | Boss bir *oda*dır: yaşayan laboratuvar; kesitleri sırayla nötralize edilir |
| Kızıl Orman Ana Kovanı | Dünya boss'u (T3) | Haftalık respawn; sunucu-yok liderlik tablosu hedefi |
| Pale Choir | Gizli (Fısıltı Alanları) | 3 Psionic varlık — yalnızca Odak 20+ build'ler duyabilir/görebilir |
| The Amalgam | Yeraltı Bio-Lab gizli katı | Başarısız deneylerin birleşimi; her öldürüşte farklı parça kompozisyonu (NG+ değişkeni) |
| Echo of Kestrel | NG+ özel | Oyuncunun önceki run'ının hayalet verisiyle savaş (loadout + skill kopyası) |
| Permafrost King | Buzul Üssü gizli | Donmuş EPU komutanı; savaş öncesi diyalogla *barışçıl çözülebilen* tek boss |

---

## 8. BÖLÜM 2 KABUL KRİTERLERİ (ÜRETİM CHECKLIST)

- [ ] 12 operatör kağıt üstü dengesi: her sınıf T3'te solo extraction yapabilmeli (iç playtest metriği: sınıf başına başarı oranı %40–60 bandı)
- [ ] Manga AI'ı, oyuncu hiçbir tuşa basmadan izlendiğinde "akıllı görünme" testini geçmeli (10 dk gözlem, tasarımcı puanı ≥4/5)
- [ ] Her boss fazı ilk denemede %90 oyuncu tarafından "okunabilir" olmalı (telegraf testi)
- [ ] Bark sistemi: taktik bilgilerin %100'ü altyazılı + görsel ikonlu (işitme erişilebilirliği)
- [ ] AI performans bütçesi min-spec donanımda (GTX 1060) doğrulanmalı

---

*— BÖLÜM 2 SONU —*

---

# BÖLÜM 3: SİLAHLAR, BALİSTİK VE GUNSMITH

---

## 1. SİLAH FELSEFESİ

> **"Silah bir stat bloğu değil, bir enstrümandır."** Her silahın öğrenilebilir bir kişiliği vardır: recoil paterni, ses imzası, şarjör ritmi. Oyuncu bir silahı "seviyesi yüksek" olduğu için değil, *elinin ezberlediği* için taşır.

**Üç kural:**
1. **Deterministik her şey:** Aynı girdi her zaman aynı sonucu verir. RNG yalnızca kritik hasar zarında ve loot'ta yaşar; balistikte asla.
2. **Yatay güç, dikey uzmanlık:** T5 silahı T1 silahının "2 katı hasar" versiyonu değildir; farklı problemleri çözer. Eski silahlar modlarla ömür boyu geçerli kalır.
3. **Gürültü = para birimi:** Her silahın ses profili taktik bir maliyettir (Bölüm 2 §5.2 ses yarıçapları).

## 2. SİLAH KATEGORİLERİ (TOPLAM 156)

| Kategori | Adet | Kimlik | Örnek Arketipler |
|---|---|---|---|
| Tabanca | 18 | Yan silah, susturucu dostu, hızlı çekiş | 9mm servis, revolver (.357), otomatik machine pistol |
| SMG | 20 | Yüksek RPM, yakın mesafe, düşük AP | PDW, klasik SMG, drum-mag sokak yapımı |
| Pompalı | 14 | Oda temizleme, choke modları | Pump, yarı-oto, **Titan Shotgun** (efsanevi) |
| Taarruz Tüfeği | 26 | Omurga sınıf; patern ustalığı merkezi | AR, AK ailesi, bullpup, **Pulse Carbine MK.2** |
| DMR | 14 | Yarı-oto hassasiyet, orta-uzun menzil | Klasik DMR, taktik lever-action |
| Keskin Nişancı | 12 | Tek atış felsefesi, sway yönetimi | Bolt-action, anti-materyel, **Plasma Sniper** (efsanevi) |
| LMG | 10 | Bastırma, Juggernaut sinerjisi | Kemerli LMG, mag-fed |
| Enerji | 16 | Zone teknolojisi; mühimmat = Enerji hücresi | **Void Rifle**, ark atıcı, ışın kesici |
| Yakın Dövüş | 18 | Stamina ekonomisi, parry (Ronin) | Bıçak, balta, katana, elektro-çekiç, Harvester's Hook |
| Fırlatılabilir/Gadget | 8+ | Taktik araçlar | Frag, duman, EMP, yapışkan C4, cıvata (sonsuz) |

## 3. SİLAH İSTATİSTİK MODELİ

Her silah 12 birincil parametreyle tanımlanır (data-driven — tek spreadsheet, mod API'sine açık):

| Parametre | Aralık | Notlar |
|---|---|---|
| Hasar (mermi başı) | 8–220 | Gövde referansı; bölge çarpanları: kafa ×1.8 / uzuv ×0.7 |
| Atış Hızı (RPM) | 40–950 | |
| Etkili Menzil (m) | 8–120 | Sonrasında hasar düşüşü eğrisi (linear-pürüzsüz, silah başına) |
| Zırh Delme (AP) | 0–60 | Mühimmatla toplanır — bkz. §5 matris |
| Mermi Hızı (m/sn) | 250–1000 | Projectile simülasyonu; hitscan YOK (ışın silahları hariç) |
| Ergonomi | 1–100 | ADS hızı, silah değiştirme, sway'e girer |
| Ağırlık (kg) | 0.4–11 | Envanter + AGI etkileşimi |
| Gürültü (m) | 6–70 | Susturucu çarpanı ×0.2–0.4 |
| Şarjör | 1–100 | + şarjör değiştirme süresi (taktik/boş ayrımı: boş şarjör +0.6 sn) |
| Recoil Patern ID | — | §4'teki spline referansı |
| Durability | 0–100 | §7.4 aşınma sistemi |
| Mod Uyumluluk Seti | — | 7 slot × uyumlu parça listeleri |

## 4. RECOIL VE NİŞAN MODELİ — PC'NİN KALBİ

### 4.1 Deterministik Patern Matematiği

Her silahın recoil'i, **atış indeksine bağlı sabit bir vektör dizisidir** (spline):

```
kick[n] = BasePattern[n] × StanceMod × GripMod × StockMod
```

- `BasePattern`: Silah başına elle tasarlanmış 30 noktalı 2D vektör eğrisi (örn. AK ailesi: ilk 7 mermi sert dikey → sola kanca → sağa test eresi). İmleç, ekranda **gerçekten** bu vektörle itilir; oyuncu ters yönde fare çekerek kompanse eder — CS:GO gramerinin birebir top-down uyarlaması.
- İlk atış her zaman %100 isabetlidir (First Shot Accuracy) — tap-fire her silahı hassas kılar.
- Ateşe ara verilince patern indeksi geri sarar (0.35 sn/index) — "resetleme" öğrenilebilir ritim yaratır.
- **Poligon modülü** (hideout) patern overlay'i gösterir: antrenman görselleştirmesi, maçta kapalı.

### 4.2 Sway ve Duruş

| Durum | Dağılım/Sway Etkisi |
|---|---|
| Ayakta hip-fire | Taban koni +%40 |
| ADS (sağ tık — imleç hassas mod) | Koni %0, kamera imlece doğru uzar (keşif ödülü + tünel görüşü riski) |
| Çömelme | Recoil ×0.85, ses -%50 |
| Hareket hâlinde | Koni +%25 (AGI azaltır) |
| Nefes tutma (Shift, sniper) | Sway 3 sn donar, stamina yakar |

### 4.3 Kamera-İmleç Sözleşmesi

Kamera, oyuncu ile imleç arasındaki noktayı takip eder (ağırlık: %65 oyuncu). ADS'de ağırlık %45'e kayar → uzağa "lean" etme hissi. Bu tek mekanik, top-down'da FPS'lerdeki "peek" derinliğini yaratır ve 21:9 monitörlerde doğal avantaj yerine *daha fazla atmosfer* verecek şekilde FOV-normalize edilir (rekabet dengesi).

## 5. MÜHİMMAT SİSTEMİ

### 5.1 Tipler

| Mühimmat | Etki | Ödünleşim |
|---|---|---|
| FMJ (standart) | Denge | — |
| AP (zırh delici) | +25 AP | Et hasarı -%20, pahalı |
| HP (genişleyen) | Et hasarı +%35, kanama şansı | Zırhta -%50 etki |
| Şok | Mekaniklere ×2, 1 sn stun | Düşük ham hasar |
| Zehirli | 8 sn DoT, iyileşme keser | Mutantlar kısmen dirençli |
| Yangın | Alan tutuşturma, panik | Kendi dumanın görüşü kapatır |
| İzli | Patern öğrenmeyi kolaylaştırır, sürüye korku | Konumunu ele verir |
| EMP hücresi | Enerji silahlarına özel; kalkan/drone eritir | Ete zayıf |

### 5.2 Delme Matrisi (özet)

`Etkin Hasar = Hasar × PenCurve(AP − ZırhSınıfı)` — AP zırhı aşarsa tam hasar + plaka aşındırma; aşamazsa hasar %15'e düşer ama plaka durability yer. **Sonuç:** düşük AP silahla ağır zırh "sünger" değil, *yanlış alet* hissi verir; zırhı eritmek meşru ama pahalı bir B planıdır.

### 5.3 Zırh Sınıfları

| Sınıf | Durur | Örnek |
|---|---|---|
| C (bez/deri) | Sadece sekme | Stalker ceketi |
| B (kevlar) | Tabanca/SMG FMJ | Devriye yeleği |
| A (plaka) | Tüfek FMJ | Command plakalı |
| S (kompozit/Zone) | Tüfek AP (kısmen) | Juggernaut seti, boss drop |

Plakalar ayrı loot'tur, hasar yer, craft'la yenilenir. Kafa/gövde/uzuv ayrı hit-zone; kask ayrı slot.

## 6. GUNSMITH — 7 SLOT MODİFİKASYON

| Slot | Örnek Parçalar | Ana Etki |
|---|---|---|
| 1. Optik | Reflex, holo, ×2/×4 scope, termal | Zoom, imleç stili, termal=anomali riski |
| 2. Namlu | Kısa/uzun/ağır | Menzil ↔ ergonomi |
| 3. Namlu Ucu | Susturucu, kompansatör, flash hider | Ses ↔ recoil ↔ namlu alevi |
| 4. Alt Ray | Dikey kabza, bipod, lazer, GL | Recoil deseni yumuşatma / bastırma |
| 5. Yan Ray | Fener, ikincil lazer, Geiger bağlantısı | Bilgi ↔ görünürlük |
| 6. Şarjör | Genişletilmiş, hızlı, drum | Kapasite ↔ ağırlık/ergonomi |
| 7. Dipçik/Kabza | Katlanır, ağır, iskelet | Sway ↔ silah değiştirme hızı |

**Kurallar:** Her parça ağırlık ekler (taşıma ekonomisi); susturucu durability'yi hızlandırır; parçalar silahtan bağımsız loot/craft/ticaret nesnesidir. **Preset sistemi:** 3 kayıtlı yapılandırma, Haven tezgâhında tek tıkla geçiş. Efsanevi silahlarda 1–2 slot kilitli "imza parça" bulunur.

### 6.4 Aşınma ve Bakım

Durability < 30: tutukluk şansı başlar (%1–4, atış başına; UI'da silah kartı sararır — **sürpriz yok, ihmal cezası var**). Tutukluk 1.2 sn mini-QTE ile açılır. Bakım kiti sahada, tam onarım Technician'da.

## 7. EFSANEVİ SİLAH KARTLARI (örnekler)

**PULSE CARBINE MK.2** *(hikâye ödülü, Perde I)* — Taarruz/Enerji hibrit. Hasar 45 · RPM 750 · Şarjör 30 · AP 15. İmza: her 10. mermi otomatik şok mermisi. Kilitli slot: namlu (pulse bobini).

**VOID RIFLE** *(Void Stalker drop)* — Mermiler 4 m'de bir mikro-faz atlar: **ilk siperi deler** (arkasındakini vurur, ikincisini delmez). Karşı-taktik öğreten silah.

**TITAN SHOTGUN** *(Colossus craft zinciri)* — Tek atış, 2.8 sn dolum, kapı/barikat yıkar. Juggernaut pasifiyle omuzdan sekmesiz.

**PLASMA SNIPER** *(Apex Membrane craft)* — 1.5 sn şarjlı atış; tam şarjda zırh sınıfı yok sayılır, iz bırakır (konum ifşası maks).

## 8. YAKIN DÖVÜŞ SİSTEMİ

- Hafif (LMB, stamina 15) / Ağır (basılı, 35, gard kırar) / İtme (F, hasarsız, alan açar).
- **Parry** (Ronin + skill node): 0.2 sn pencere; başarı = düşman 1.5 sn açık + mermi savuşturma (Ronin).
- Sessiz eliminasyon: fark edilmemiş hedefe arkadan tek tuş (animasyon 1.8 sn — riskli, kesilebilir).

## 9. GAME FEEL SPESİFİKASYONU (ölçülebilir)

| Öğe | Spec |
|---|---|
| Hitstop | Kritiklerde 40 ms dünya donması |
| Screen shake | Perlin tabanlı, silah sınıfı başına genlik; slider %0–150 |
| Muzzle flash | 2 frame + dinamik ışık pulse (çevreyi aydınlatır — taktik bilgi) |
| Tracer | Mermi hızıyla senkron, %25 opak |
| Hasar geri bildirimi | Hit-marker sesi (kafa=ayrı tını) + isteğe bağlı hasar sayıları (varsayılan kapalı — atmosfer) |
| Kill teyidi | 60 ms zaman genlemesi (×0.9) + ragdoll impulse mermi vektöründen |
| Ses katmanları | Mekanizma + patlama + kuyruk (mekâna göre reverb) + kabuk düşmesi = 4 katman/atış |

## 10. DENGE HEDEFLERİ

- Aynı tier içinde herhangi iki silahın "cost-adjusted DPS" farkı ≤ %12.
- Her kategoriden en az 1 silah T5'te meta-geçerli olmalı (playtest telemetrisiyle doğrulanır).
- Susturuculu build'ler ham DPS'te %15–20 geride kalmalı (bilgi avantajı bedeli).

---

*— BÖLÜM 3 SONU —*

---

# BÖLÜM 4: HARİTA VE DÜNYA TASARIMI

---

## 1. DÜNYA YAPISI — 25 BÖLGE

Zone yarı-açık dünyadır: bölgeler birbirine geçiş noktalarıyla bağlı, her biri kendi başına raid haritası olarak yüklenir (DirectStorage ile 1–2 sn geçiş). Boyut birimi: "ekran" (1080p'de bir tam görünüm ≈ 30×17 m).

| # | Bölge | Tier | Tema/Kimlik | Boyut (ekran) | Giriş | Exfil | Boss/Mini-boss |
|---|---|---|---|---|---|---|---|
| 1 | Perimeter Duvarı | T1 | Tutorial + Haven bağlantısı | 40 | 3 | 2 | — |
| 2 | Çöl Harabeleri | T1 | Kurumuş banliyö, gömülü evler | 70 | 4 | 3 | Kum Sürüsü Alfası |
| 3 | Terk Edilmiş Metro (Dış Hat) | T1 | Tünel klostrofobisi, ilk mutantlar | 60 | 3 | 2 | Broodmother |
| 4 | Karantina Kampı | T1 | Terk edilmiş mülteci şehri, lore yoğun | 55 | 4 | 3 | — |
| 5 | Tren Garı | T2 | Vagon labirenti, dikey yük vinçleri | 65 | 4 | 3 | The Collector (gezici) |
| 6 | Mega Şehir — Dış Bölge | T2 | Neon enkazı, düşey yaşam blokları | 110 | 5 | 4 | — |
| 7 | Askeri Üs | T2 | Açık alan + bunker ağı | 80 | 4 | 3 | **THE HARVESTER**, Warden of the Wall |
| 8 | Liman ve Tersane | T2 | Konteyner labirenti, vinç keskin nişancı hatları | 75 | 5 | 3 | Pas Kaptanı |
| 9 | Şehir Hastanesi | T2 | Yatay korku, MOTHER ön bağlantısı | 50 | 3 | 2 | The Amalgam (ön formu) |
| 10 | Metro — Derin Hat | T3 | Su basmış tüneller, elektrik tehlikesi | 70 | 3 | 2 | Reactor Ghost (gezici) |
| 11 | Kimyasal Fabrika | T3 | Zehirli bulut katmanları, ventil bulmacaları | 85 | 4 | 3 | Rust Titan |
| 12 | Radyoaktif Kızıl Orman | T3 | Organik labirent, görüşü kapatan flora | 100 | 5 | 3 | Kızıl Diş, **Ana Kovan** (dünya boss'u) |
| 13 | Nükleer Santral | T3 | Görev 7 dünya-durumu bölgesi (iki kalıcı hâli var) | 90 | 4 | 3 | Reactor Ghost |
| 14 | Baraj ve Hidro Tesis | T3 | Su seviyesi mekaniği (kapaklar açılınca rota değişir) | 75 | 4 | 3 | Twin Serpents (göç eder) |
| 15 | Mega Şehir — Çekirdek | T3 | Gökdelen içleri, cam köprüler, Curator müzesi | 120 | 5 | 4 | The Curator |
| 16 | Sibernetik Fabrika | T4 | Aktif üretim hattı — düşman *üretilir* | 85 | 3 | 2 | General ARES |
| 17 | Yeraltı Bio-Laboratuvarı | T4 | MOTHER'ın alanı; steril beyaz + biyo-kaos | 95 | 3 | 2 | MOTHER'ın Bahçesi, The Amalgam |
| 18 | Buzul Üssü | T4 | Isı yönetimi (dışarıda donma sayacı) | 90 | 4 | 2 | **COLOSSUS UNIT**, Permafrost King |
| 19 | Veri Merkezi "Ark" | T4 | Sunucu koridorları, Blackout kalıcı | 60 | 3 | 2 | Pale Choir |
| 20 | Fısıltı Vadisi | T4 | Açık Psionic arazi, gerçeklik %50 | 80 | 3 | 2 | Pale Choir |
| 21 | Kanalizasyon Omurgası | T2–T4 | Bölgeleri bağlayan gizli ağ (Juggernaut giremez) | 50 | çok | — | — |
| 22 | Gizli Tesis (Prometheus) | T5 | Görev 11; laboratuvar dehlizleri | 85 | 2 | 1 | Void Stalker (rövanş) |
| 23 | Çökmüş Yörünge İstasyonu | T5 | Düşük-G enkaz; dikeylik maksimum | 70 | 2 | 1 | **APEX PREDATOR** |
| 24 | Membran Eşiği | T5 | Core girişi; anomali yoğunluğu %100 | 45 | 1 | 1 | Echo of Kestrel (NG+) |
| 25 | **THE CORE** | T5 | Final; kurallar askıda | 35 | 1 | — | **THE SOURCE** |

## 2. SEVİYE TASARIM DİLİ

### 2.1 Üç Rota Kuralı
Her POI'ye (point of interest) en az üç yaklaşım: **Gürültülü** (ön kapı, hızlı, çatışma), **Sessiz** (havalandırma/çatı, yavaş, AGI/Ghost), **Teknik** (kilit/hack/C4 — kaynak harcar). Loot odaları bu üçgenin kesişimine yerleştirilir.

### 2.2 Landmark Navigasyonu
Mini-harita pusula verir, yol çizmez. Yön bulma landmark'larla: her bölgenin ekranlar-ötesinden okunan 2–3 silüeti vardır (santral bacaları, kırık kule, turuncu vinç). Paralaks arka plan katmanı navigasyon aracıdır, süs değil.

### 2.3 Katman (Z-Level) Sistemi
Top-down'da dikeylik: haritalar 2–4 z-katmanı içerir (çatı/zemin/bodrum/derin). Merdiven, asansör, halat inişleri katman geçişidir; üst katman kenarından alt katman *görülür ve ateş edilebilir* (balkon pusuları — çift yönlü risk).

### 2.4 Ekran Yoğunluğu Kuralı
Her ekran dolusu alanda en az 1 etkileşim (loot/lore/tehlike/geçit); en fazla 1 "büyük an". Boş koridor yasak, kalabalık ekran da yasak — gerilim nefes alarak yükselir.

## 3. ANOMALİ KATALOĞU (tam — 12 tip)

Bölüm 1 §5.3'teki 6 tipe ek:

| Anomali | Tehlike | Karşı Taktik | Ödül |
|---|---|---|---|
| Statik Deşarj | Metal zırhla yaklaşana zincir yıldırım | Zırhı çıkar (risk takası) | Enerji hücresi yatağı |
| Kör Nokta | İçindeyken HUD tamamen kapanır | Hafıza + landmark | İçeride sensörsüz kasa |
| Tekrar Odası | Odaya giren 30 sn'lik zaman döngüsüne düşer | Döngü içinde çıkışı değiştir (bulmaca) | Döngü kırılınca "Zaman Kristali" |
| Yankı Fırtınası | Geçmiş çatışmaların hayalet mermileri gerçek hasar verir | Yankı paternini 1 tur izle, sonra geç | Fırtına gözünde artefakt |
| Cazibe | Loot parıltısı taklidi yapar, yaklaşanı yutar | Cıvata testi (sahte parıltı cıvatayı yutmaz) | Gerçek anomali öldüğünde ×3 loot |
| Ağırlık Tersinmesi | Bölgede eşyalar tavana düşer — envanter ağırlığı sıfırlanır | İçinde savaşma (mermiler de sapar) | Aşırı yük taşıyarak geçiş koridoru |

**Artefakt sistemi:** Anomaliler periyodik olarak artefakt üretir (LCK ağırlıklı). Artefaktlar kuşak slotuna takılır (2–3 slot): pasif güç + kalıcı yan etki (örn. *"Common Kalp":* +25 HP, sürekli hafif radyasyon). Bedeli olmayan artefakt yoktur.

## 4. YIKILABİLİR ÇEVRE

| Malzeme Sınıfı | Yıkım | Örnek |
|---|---|---|
| Kırılgan | Her mermi | Cam, çürük tahta, şişeler |
| Yumuşak siper | Kademeli aşınır (HP'li chunk) | Alçıpan, kum torbası — LMG bastırması siperi *eritir* |
| Sert siper | Yalnız patlayıcı | Beton, araç gövdesi |
| Yapısal | Yalnız C4/script | Gizli duvarlar, boss arena değişimleri |

- Patlayan variller/gaz boruları/jeneratörler: zincirleme tepki bütçesi ekran başına 3 (okunabilirlik).
- Yıkım kalıcıdır (raid boyunca) — ikinci geçişte eritilmiş siperler taktik haritayı değiştirir.

## 5. DİNAMİK HAVA VE GÜNDÜZ/GECE

| Koşul | Oynanış Etkisi |
|---|---|
| Gece | AI görüş ×0.5; fener/muzzle flash ihbar değeri ×2; mutant aktivitesi +%40 |
| Yağmur | Ses yarıçapları -%30 (sızma havası); SSR su birikintileri (görsel vitrin) |
| Anomali Fırtınası | Tüm anomaliler yer değiştirir, açıkta Psionic DoT — sığınak anları (Metro girişleri değer kazanır) |
| Sis | Görüş 12 m; Hunter işaretleme değeri artar |
| Kızıl Ay (aylık event) | Loot +1 tier, tüm düşman elit — bkz. Bölüm 1 §3.7 |

Hava, raid başında telsiz tahminiyle bildirilir (loadout kararına girdi) — %20 ihtimalle tahmin yanılır.

## 6. ZONE SHIFT — HAFTALIK PROSEDÜREL REMİX

**Değişen:** anomali yerleşim seed'i, loot masası ağırlıkları, devriye rotaları/kompozisyonları, dinamik exfil konumları, mini-boss göç noktaları, 2 rastgele "sıcak bölge" (loot +1 tier, tehdit +1 tier).
**Asla değişmeyen:** geometri, landmark'lar, ana görev objeleri, sabit exfil'ler.
**İlke:** *Bilgi eskimez, ezber eskir.* Haritayı bilen oyuncu avantajını korur; rotasını körlemesine tekrarlayan oyuncu cezalandırılır.

## 7. ÇEVRESEL BULMACALAR

Tipoloji: **Güç yönlendirme** (jeneratör-şalter ağları), **su/gaz seviyesi** (ventil sıralaması), **ışık-gölge** (Curator tipi düşmanlar araç olur), **anomali kullanımı** (yerçekimi kuyusuyla erişilmez balkona "fırlama"), **ses yemleme** (mutant sürüsünü kapı açtırmak için kullanma). Kural: hiçbir bulmaca ana rotayı kilitleyemez — bulmacalar her zaman *opsiyonel ödül* kapısıdır.

## 8. ÖRNEK DERİN DÖKÜM: MEGA ŞEHİR — ÇEKİRDEK (T3)

- **Fantazma:** "Şehir öldü ama reklamları ölmedi." Neon devremülkleri, 40 kat yukarıda dönen hologram, aşağıda kültün mum ayinleri.
- **Alt bölgeler (5):** Alışveriş Kanyonu (dikey loot, cam köprüler — kırılabilir!), Metro Giriş Meydanı (fraksiyon çatışma sahnesi: Command vs Kült, oyuncu izleyebilir/karışabilir), Konut Kuleleri (apartman dairesi mikro-lore sahneleri), Curator Müzesi (mini-boss + ışık bulmacası), Çatı Hattı (Ghost/AGI rotası, sniper hatları).
- **Loot ısı haritası:** Değer, zeminden çatıya ve merkeze doğru artar; en değerli kasa cam köprünün ortasındaki asılı polis zeplinindedir (ulaşım: vinç bulmacası VEYA yerçekimi anomalisi VEYA 2 C4).
- **Ses sahnesi:** Rüzgârda gıcırdayan tabelalar false-positive ses üretir (paranoya tasarımı); yağmurda bölge "sessiz sızma cenneti"ne döner.

## 9. BOSS ARENA KURALLARI

1. Arena, girmeden önce *okunabilir* olmalı (kapı eşiğinden ön izleme — hazırlık şansı).
2. Her arenada en az 1 yenilenen kaynak noktası (mühimmat sandığı — savaş uzarsa açlık ölümü yok).
3. Arena tehlikeleri boss'a da işler (Bölüm 2 §7.1).
4. Arena girişi = kayıt noktası; tekrar denemede yürüyüş yok.

---

*— BÖLÜM 4 SONU —*

---

# BÖLÜM 5: TEKNİK GDD, SİSTEMLER, UI/UX, SES VE PLATFORM ENTEGRASYONU

---

## 1. MOTOR VE YAZILIM MİMARİSİ

### 1.1 Motor Kararı

**Birincil: Unity 6 (URP 2D Renderer).** Gerekçe: olgun 2D ışıklandırma (Light2D + normal map), Burst/Jobs ile mermi simülasyonu ölçeklenmesi, Steam/EGS eklenti ekosistemi, mod desteği için IL2CPP+Lua hibrit yolu. **Alternatif:** Godot 4.x (lisans esnekliği; risk: konsol portu vendor bağımlılığı).

### 1.2 Mimari İlkeler

```
┌─ SIMULATION CORE (deterministik, render'dan bağımsız) ─┐
│  Fixed timestep 60 Hz · Command pattern girdi kuyruğu   │
│  AI / balistik / fizik / ekonomi                        │
└──────────────┬──────────────────────────────────────────┘
               │ state snapshot + interpolasyon
┌─ PRESENTATION (kilitsiz FPS) ──────────────────────────┐
│  Render · VFX · ses · UI — 144/240/360 Hz              │
└─────────────────────────────────────────────────────────┘
```

- **Neden ayrık simülasyon:** (a) Kilitsiz FPS'te fizik tutarlılığı, (b) replay/killcam bedavaya çıkar, (c) **co-op hazırlığı:** deterministik core + command kuyruğu = ileride lockstep/rollback netcode'a doğrudan geçiş. Co-op için bugün yazılan tek satır ağ kodu yok; ama her sistem "girdi → komut → simülasyon" disipliniyle yazılır.
- **Data-driven her şey:** Silahlar, düşmanlar, loot masaları, diyaloglar = ScriptableObject/JSON. Mod API'si (bkz. §9) aynı veri yolunu kullanır — "bizim içerik ekibimiz ilk modder'dır" ilkesi.
- Mermiler: ECS/Jobs havuzunda simüle (hedef: 2.000 eşzamanlı mermi, <0.5 ms/frame).

## 2. RENDER PIPELINE — "HD-2D" SPESİFİKASYONU

| Katman | Teknik |
|---|---|
| Sprite'lar | 32 px/birim pixel art + el boyaması normal map + emissive map (3 doku/sprite) |
| Işıklandırma | 2D deferred: sınırsız dinamik ışık; muzzle flash, neon, anomali = gerçek ışık kaynağı |
| Gölgeler | Shadow caster 2D — düşman gölgesi köşeden *önce* görünür (oynanış bilgisi!) |
| Yansımalar | Su birikintileri: screen-space planar yansıma (custom shader, yağmur sistemine bağlı) |
| Hacimsel sis | Ekran-uzayı ray-march katmanı (ucuz, 2D derinlik maskeli) |
| Paralaks | 5 katman (arka gökyüzü → ön yağmur); kamera-imleç ofsetiyle senkron |
| Post | Filmik grain (kapatılabilir), kromatik aberasyon yalnız anomali yakınında (diegetik), CRT filtresi (opsiyonel nostalji modu) |
| Gore | Ragdoll + dismemberment sprite parçaları + zemin decal havuzu (512 decal, LRU) |

**Piksel bütünlüğü kuralı:** Tüm dünya öğeleri piksel-grid'e oturur; yalnız kamera, paralaks ve döner mermiler sub-pixel hareket eder (modern akıcılık + retro doku dengesi).

## 3. WINDOWS 11 / PC TEKNİK HEDEFLERİ

| Sistem | Spesifikasyon |
|---|---|
| Grafik API | DX12 (birincil) + Vulkan (ikincil); önyükleyici otomatik seçer |
| DirectStorage | Bölge geçişleri ≤ 2 sn (NVMe), ≤ 5 sn (SATA); asset'ler GDeflate sıkıştırmalı |
| Shader derleme | İlk açılışta arka planda tam ön-derleme (progress bar'lı, ~90 sn) — **oyun içi shader stutter'ı: sıfır tolerans** |
| FPS | Kilitsiz; frame pacing v-sync/G-Sync/FreeSync farkındalıklı; FPS limitleyici (30–∞) |
| Girdi | Raw Input; 8000 Hz mouse polling desteği; girdi → simülasyon gecikmesi ≤ 4 ms; tam keybinding (çift atama + chord) |
| Gamepad | XInput + DualSense (haptik: silah sınıfı başına profil; adaptif tetik: silah direnci) |
| Ekran | 16:9 / 16:10 / 21:9 / 32:9 native. Ultrawide'da FOV sabit, sinematik letterbox yok, HUD "kenar/merkez" iki preset |
| Min spec | GTX 1060 6GB · 8GB RAM · i5-8400 → 1080p/60 sabit |
| Önerilen | RTX 3060 · 16GB RAM → 1440p/144 (tam VFX) |
| Sürücü krallığı | Alt+Tab anında, borderless varsayılan, çoklu monitör davranışı ayarlanabilir, HDR10 desteği |

## 4. KAYIT SİSTEMİ

- Profil (skill/hideout/hikâye) + raid snapshot (raid başında yazılır) ayrı dosyalar; Steam Cloud + EGS Cloud senkron.
- **Anti-save-scum:** Raid içinde tek dinamik slot, çıkışta silinir; seçim anlarında anlık commit. Story modunda 3 manuel slot açılır.
- Yozlaşma koruması: çift yazım + CRC; son 5 yedek döner.

## 5. EKONOMİ — SAYISAL MODEL

### 5.1 Para Akışı

| Kaynak (kredi/saat, T2 ort.) | Gider |
|---|---|
| Loot satışı: 8–15K | Mühimmat/medikal: 3–5K |
| Görev ödülleri: 4–8K | Sigorta primleri: 2–6K |
| Artefakt satışı: 0–20K (varyans yüksek) | Gunsmith parçaları: isteğe bağlı çukur |
| | Hideout inşaat: uzun vadeli emici (toplam ~2.5M) |

**Hedef denge:** Ortalama oyuncu net +%20 kazançta kalır (ilerleme hissi); ölüm serisi 3+ olursa "Kane'in kredisi" güvenlik ağı devreye girer (borç mekaniği = hikâyeyle uyumlu soft floor, oyuncu asla tam sıfırlanamaz).

### 5.2 Envanter (Grid) Spesifikasyonu

- Çanta grid'leri: 6×4 → 12×8 arası; eşya boyutları 1×1 (mermi) → 2×6 (sniper). Döndürme: R.
- Sürükle-bırak + hızlı aktarım (Ctrl-tık) + otomatik düzenle (sıkıştırma algoritması — Tetris zevkini bozmamak için "mükemmel değil, iyi" yerleştirir).
- Güvenli konteyner 2×2 (moda göre); silah kılıfı + sırt slotu grid dışı.
- Ağırlık ayrı eksen: grid *hacim*, kg *hareket* cezası (ikili kısıt = anlamlı loot seçimi).

### 5.3 Crafting ve Hideout Maliyet Eğrisi

- 6 kaynak ailesi: hurda metal, elektronik, kimyasal, kumaş/deri, mutant dokusu, anomali reçinesi.
- Tarif kaynakları: Codex keşfi, fraksiyon rep ödülü, tüccar, boss drop.
- Hideout 12 modül × 3 kademe; kademe maliyeti ≈ ×2.4 katsayı — son kademeler prestij hedefi.

## 6. GÖREV SİSTEMİ MİMARİSİ

- Quest graph: düğümler (hedef) + kenarlar (koşul); dünya-durumu bayrakları (Bölüm 1 §5.9) global blackboard'da.
- **Dinamik yan görev üreticisi:** Şablon (getir/temizle/kurtar/keşfet) × bölge × fraksiyon × Zone Shift seed → haftalık taze görev havuzu; el yapımı zincirlerle asla çakışan ödül vermez.
- Görev takibi HUD'da tek satır; detay haritada. Görev oku YOK — yön tarifi metinle/landmark'la (P4 sütunu).

## 7. UI/UX SPESİFİKASYONU

| Öğe | Karar |
|---|---|
| HUD | Köşelere dağıtık, %85 saydam; HP/Enerji sol alt, mühimmat sağ alt, pusula üst, görev sağ üst. "Minimal HUD" modu: yalnız çatışmada belirir |
| Envanter ekranı | Oyun durmaz (P2) — dünya arka planda blur'suz görünür kalır; envanterdeyken karakter otomatik çömelir |
| Menüler | Tam klavye gezinimi + mouse; her panelin kısayolu (I/M/J/K/T); gamepad'de radyal menüler |
| Harita | El çizimi stil; keşfedilen alan mürekkeple dolar; oyuncu işaretçileri (6 ikon) |
| İmleç | Bağlamsal: nişangâh (silah), el (etkileşim), dişli (hack) — imleç ekranın kalbidir, asla kaybolmaz |
| Erişilebilirlik | Bölüm 1 §4.4 + UI ölçek %75–150, dislexia-dostu font seçeneği, tüm QTE'ler basılı-tut alternatifli |
| Ultrawide | HUD kenar/merkez preset; sinematiklerde 21:9 native kadraj |

## 8. SES TASARIMI VE MÜZİK

### 8.1 Teknoloji
- Middleware: **Wwise**. Windows Sonic / Dolby Atmos / DTS:X passthrough; kulaklıkta HRTF binaural (top-down'da düşey katman sesleri dahil — bodrumdaki mutant *alttan* duyulur).
- Occlusion/obstruction: duvar malzemesine göre alçak geçiren filtre; ses portalları (kapılar/menfezler) yön doğruluğu sağlar — **ayak sesinin yönü fareyle işaret edilebilir doğrulukta** (körlemesine test edilir).

### 8.2 Adaptif Müzik Durum Makinesi

```
KEŞİF (dark ambient, bölge teması)
  → GERİLİM (algılanma %50+ : perküsyon katmanı girer)
    → ÇATIŞMA (synthwave bas + arp)
      → KAÇIŞ/EXFIL (yükselen ostinato + kalp atışı)
BOSS: özel parça, faz başına katman (final fazda elektro-metal tam giriş)
```

- Katmanlar dikey mix'le girer/çıkar (hard cut yok); Haven teması, oyuncunun hideout kademesiyle enstrüman kazanır (üs büyüdükçe müzik zenginleşir — duyulabilir ilerleme).

## 9. STEAM / EGS / MOD ENTEGRASYONU

| Özellik | Kapsam |
|---|---|
| Başarımlar | 60 adet; %30'u gizli (lore spoiler'ları); "Kaynak Kodu" gizli son başarımı ("%0.5 kulübü" hedefi) |
| Rich Presence | "Kızıl Orman'da — T3 raid, 34. dakika" formatı; Discord paralel |
| Steam Cloud / Family | Tam destek; Steam Deck "Playable" hedefi (UI ölçek + gamepad preset'i hazır) |
| Trading Cards | 12 kart (NPC portreleri) + rozet seti |
| **Workshop Mod API** | Kademe 1 (EA ile): veri modları — silah/düşman/loot JSON + sprite paketleri. Kademe 2 (1.0): Lua sandbox — görev/davranış scriptleri, harita editörü (in-house aracın basitleştirilmiş sürümü). Kademe 3 (1.0+6 ay): total conversion çengelleri |
| Anti-tamper | YOK (tek oyunculu; Denuvo bilinçli reddi — topluluk güveni + performans). Liderlik tabloları ayrı doğrulama replay'iyle korunur |

## 10. ÜRETİM PLANI VE RİSKLER

### 10.1 Takım ve Kilometre Taşları

| Faz | Süre | Takım | Çıktı |
|---|---|---|---|
| Pre-prod | 6 ay | 8 kişi | Vertical slice: Askeri Üs + Harvester + 3 sınıf + 20 silah |
| Prod I | 12 ay | 18 kişi | Perde I komple, T1–T2 bölgeleri, core loop final |
| **Early Access** | — | — | Fiyat 19,99$; Perde I + 10 bölge + 6 sınıf |
| Prod II (EA) | 12 ay | 22 kişi | 3 büyük içerik güncellemesi (topluluk telemetrisiyle) |
| **1.0 Launch** | — | — | 24,99$; tam hikâye, 25 bölge, 12 sınıf, mod Kademe 2 |

### 10.2 Risk Matrisi (ilk 5)

| Risk | Olasılık | Etki | Önlem |
|---|---|---|---|
| Recoil sisteminin top-down'da "hissedilmemesi" | Orta | Kritik | Pre-prod'un İLK prototipi; hissedilmezse P1 pivotu erken yapılır |
| İçerik kapsamı (25 harita) taşması | Yüksek | Yüksek | EA modeli + Zone Shift'in az haritayı taze tutması; 1.0 minimumu 18 harita |
| Tek oyunculuya extraction'ın "neden?" sorusu | Orta | Orta | Hikâye + sigorta + Collector mini-boss'u kaybı anlamlı kılar; demo ile erken doğrulama |
| Wwise/DirectStorage entegrasyon maliyeti | Düşük | Orta | Pre-prod'da teknik spike'lar |
| Mod API güvenliği (Lua sandbox) | Orta | Orta | Kademeli açılım, izin listesi tabanlı API |

---

## KAPANIŞ — MASTER GDD DURUMU

| Bölüm | Durum |
|---|---|
| 1. Genel Bakış, Core Loop, Hikâye ve Evren | ✅ |
| 2. Karakterler, Düşmanlar ve AI | ✅ |
| 3. Silahlar, Balistik ve Gunsmith | ✅ |
| 4. Harita ve Dünya Tasarımı | ✅ |
| 5. Teknik GDD ve Sistemler | ✅ |

**Sonraki adımlar (üretim):** Ek-B düşman istatistik spreadsheet'i · Ek-C mini-boss tam listesi · silah veri tablolarının (156 satır) balans spreadsheet'i · vertical slice görev dökümü. Bu ekler GDD'nin değil, canlı üretim veritabanının parçasıdır.

*— MASTER GDD SONU — PROJECT ECLIPSE: LOST ZONE — "Zone seni bekliyor." —*
