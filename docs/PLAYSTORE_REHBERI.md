# ZİFİR — Google Play Store Yayın Rehberi

Oyun artık **tam mobil**: dokunmatik joystick, lazer nişan, titreşim, dokunmatik
menüler hazır ve test edildi. Play Store HTML dosyası kabul etmez — oyunu bir
Android uygulamasına (AAB) çevireceğiz. En sağlam yol: **Capacitor** (oyunu
uygulamanın içine gömer, internet gerektirmez, offline çalışır).

> Önce telefonda dene: bilgisayarında sunucu açıkken telefonun tarayıcısından
> `http://<PC-IP>:8420/index.html` aç (aynı Wi-Fi). Joystick/lazer hissini
> hemen görürsün, paketlemeden önce ayar yapmak istersen kolay olur.

---

## 0. Gerekenler (bir kere kurulur)

| Araç | Nereden | Not |
|---|---|---|
| Node.js LTS | nodejs.org | Capacitor için |
| Android Studio | developer.android.com/studio | SDK + derleyici + emülatör |
| Google Play Console hesabı | play.google.com/console | tek seferlik 25$ |

---

## 1. Proje iskeleti (PowerShell'de, boş bir klasörde)

```powershell
mkdir zifir-android; cd zifir-android
npm init -y
npm install @capacitor/core @capacitor/cli @capacitor/android
npx cap init "ZIFIR" "com.altaowxgm.zifir" --web-dir=www
mkdir www
copy "<proje-klasoru>index.html" www\index.html
npx cap add android
```

`capacitor.config.json` dosyasını şöyle düzenle:

```json
{
  "appId": "com.altaowxgm.zifir",
  "appName": "ZİFİR",
  "webDir": "www",
  "backgroundColor": "#05060c",
  "android": { "allowMixedContent": false }
}
```

> Oyun güncelleyince tek yapman gereken: `index.html`'i `www\` içine tekrar
> kopyala → `npx cap sync android` → yeniden derle.

## 2. Android ayarları (Android Studio'da bir kere)

`npx cap open android` ile projeyi aç, sonra:

**a) Yatay kilit + titreşim izni** — `android/app/src/main/AndroidManifest.xml`:

```xml
<!-- <activity ...> etiketine ekle: -->
android:screenOrientation="sensorLandscape"

<!-- <manifest> altında, <application> öncesine: -->
<uses-permission android:name="android.permission.VIBRATE"/>
```

**b) Tam ekran (çentik dahil)** — `android/app/src/main/res/values/styles.xml`
içindeki `AppTheme.NoActionBarLaunch`'ın üstünde `AppTheme.NoActionBar`'a ekle:

```xml
<item name="android:windowFullscreen">true</item>
<item name="android:windowLayoutInDisplayCutoutMode">shortEdges</item>
```

**c) Sistem çubuklarını gizle (immersive)** — `MainActivity.java`:

```java
import android.os.Bundle;
import androidx.core.view.WindowCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.core.view.WindowInsetsControllerCompat;
import com.getcapacitor.BridgeActivity;

public class MainActivity extends BridgeActivity {
  @Override
  public void onCreate(Bundle savedInstanceState) {
    super.onCreate(savedInstanceState);
    WindowCompat.setDecorFitsSystemWindows(getWindow(), false);
    WindowInsetsControllerCompat c =
      new WindowInsetsControllerCompat(getWindow(), getWindow().getDecorView());
    c.hide(WindowInsetsCompat.Type.systemBars());
    c.setSystemBarsBehavior(
      WindowInsetsControllerCompat.BEHAVIOR_SHOW_TRANSIENT_BARS_BY_SWIPE);
  }
}
```

**d) Uygulama ikonu** — Android Studio'da `res` klasörüne sağ tık →
New → Image Asset → 512×512 ikonunu seç (aşağıda "Görseller").

## 3. Telefonda test

Telefonu USB ile bağla (geliştirici modu + USB hata ayıklama açık) →
Android Studio'da yeşil ▶ Run. Oyun telefonda uygulama olarak açılır.
Kontrol listesi: joystick sol yarımda doğuyor mu · lazer düşmanda kırmızıya
dönüp otomatik ateş ediyor mu · titreşim geliyor mu · [L] İngilizce çalışıyor mu ·
kayıt uygulama kapatılıp açılınca duruyor mu.

## 4. İmzalı AAB üret

1. Android Studio → **Build → Generate Signed App Bundle** → `Android App Bundle`.
2. **Create new keystore**: dosyayı ve şifreleri kaydet — **BU DOSYAYI ASLA
   KAYBETME**, güncellemeler ancak aynı anahtarla imzalanabilir. (Yedeğini
   buluta at.)
3. `release` seç → AAB üretilir: `android/app/release/app-release.aab`.

## 5. Play Console

1. **Uygulama oluştur** → ad: ZİFİR, tür: Oyun, ücretsiz.
2. **Store listing:**
   - Kısa açıklama + uzun açıklama: `ITCH_SAYFASI.md`'deki metinler birebir kullanılabilir.
   - **Görseller (zorunlu):** uygulama ikonu 512×512 PNG · feature graphic
     **1024×500** PNG · en az 2 telefon ekran görüntüsü (16:9, oyun içinden).
     Kapak tabanı olarak `ZIFIR_KAPAK.html`'i açıp görüntü al, 1024×500'e kırp.
3. **İçerik derecelendirme anketi:** şiddet = evet (fantastik/piksel), kan =
   hayır (enerji efektleri), kumar/alışveriş = hayır → muhtemelen PEGI 7-12.
4. **Veri güvenliği formu:** "Veri toplanmıyor, paylaşılmıyor" — doğru, çünkü
   oyun tamamen offline, kayıt cihazda (localStorage). Reklam/analitik yok.
5. **Gizlilik politikası URL'si (zorunlu):** tek paragraflık "veri toplamıyoruz"
   sayfası yeterli. Ücretsiz seçenekler: GitHub Pages / Google Sites / itch
   sayfana bir bölüm. İstersen metnini ben yazarım.
6. **Üretim → Yeni sürüm** → AAB'yi yükle → incelemeye gönder.
   İlk inceleme genellikle 1-7 gün. Yeni hesaplarda Google önce **kapalı test**
   (12 test kullanıcısı, 14 gün) isteyebilir — Console yönlendirir.

## 6. Sonrası (istersen)

- **Reklam/IAP:** `@capacitor-community/admob` ile rewarded ad (ölünce devam,
  çift kredi) eklenebilir — oyana rahatsızlık vermeyen tek model bu. İstediğinde
  entegre ederim; veri güvenliği formu o zaman güncellenir.
- **Güncelleme akışı:** `index.html` değişti → `www\`'a kopyala →
  `npx cap sync android` → Signed AAB → Console'da yeni sürüm.

---

## Sık sorulanlar

**Zip mi, tek dosya mı?** Play Store'a giden şey `.aab` dosyasıdır — HTML'in
kendisi uygulamanın içine gömülür. itch.io'ya ise `index.html`'i tek başına
yüklemeye devam edebilirsin; aynı dosya artık mobil tarayıcıda da oynanıyor.

**Kayıtlar silinir mi?** Hayır — Capacitor WebView'i kalıcı localStorage
kullanır; uygulama güncellemelerinde kayıt korunur. Uygulama *kaldırılırsa*
silinir (normal davranış).

**İnternet izni gerekiyor mu?** Hayır. Oyun sıfır ağ isteği yapar — bu, veri
güvenliği formunu da en basit haline indirir.
