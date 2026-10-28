# ZİFİR — Unity 6 Remake · Mimari & Yol Haritası

Kaynak tasarım: `../index.html` (HTML5 orijinal) + `../PROJECT_ECLIPSE_GDD.md`.
Bu bir port değil, aynı oyun tasarımının mobile-first Unity 6 remake'i.

## Mimari kararlar

| Karar | Neden |
|---|---|
| 3 assembly: `Zifir.Core` / `Zifir.Game` / `Zifir.Editor` | Derleme izolasyonu; Core hiçbir şeye bağımlı değil, Game→Core, Editor→ikisi |
| ScriptableObject tabanlı veri (WeaponDef, OperatorDef, SectorDef) | Inspector'dan ayarlanabilir, kod değişmeden denge yaması |
| Statik event hub (`GameEvents`) | Sistemler birbirine referans tutmaz; God Object yok |
| Kod-inşa UI + sahne (composition root `GameBootstrap`) | Prefab çakışması yok, her şey diff'lenebilir; sahne YAML elle bozulmaz |
| 1 world unit = 1 tile (16 px) | HTML orijinaldeki tüm denge sayıları ÷16 ile birebir taşınır |
| Object pooling (mermi/efekt/düşman) | GC spike sıfır hedefi |
| JSON save (persistentDataPath, versiyonlu migrasyon) | HTML save v5 şemasının aynası |
| Key tabanlı lokalizasyon (`Loc.T("menu.start")`), varsayılan EN | Hardcoded metin yok; yeni dil = yeni tablo |

## Fazlar

- **F1 — Çekirdek dilim (bu oturum):** input (çift joystick + hassasiyet), oyuncu
  (hareket/sprint/stamina), lazer nişan + yapışkan kilit + oto ateş, silah/mermi
  (pool), basit düşman (kovala+temas hasarı), test arenası, kamera, HUD (HP/mermi/
  joystick görselleri), save, lokalizasyon, haptics, Android build ayar scripti.
  **Çıkış kriteri: hatasız derleme + editörde oynanabilir dilim.**
- **F2 — Oynanış paritesi:** 8 silah + recoil paternleri, 6 operatör + yetenekler,
  düşman zoo (asker/hound/drone/spitter/suicider/elite), ses-tabanlı AI, procgen
  harita (yol/bina/anomali makro planı), extraction döngüsü, loot.
- **F3 — Meta:** menüler, market, üs, ustalık, başarımlar, codex, hikâye, boss,
  The Collector, günlük zone, NG+.
- **F4 — His & mobil cila:** synth SFX/müzik (procedural AudioClip), parçacıklar,
  ışıklandırma (URP 2D lights), titreşim desenleri, safe area, performans geçişi
  (profiler, pooling denetimi), adaptive icon, splash.
- **F5 — Yayın:** IL2CPP/ARM64 AAB, Play Console.

## Derleme doğrulaması

Unity kapalıyken: `Unity.exe -batchmode -nographics -quit -projectPath Zifir -logFile build.log`
→ log'da `CS` hatası aranır. Android ayarları: menü **Zifir → Android → Apply Player Settings**,
AAB: **Zifir → Android → Build AAB**.
