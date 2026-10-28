using UnityEngine;

namespace Zifir.Core
{
    /// <summary>
    /// Android haptic feedback via VibrationEffect (API 26+), no plugin needed.
    /// No-op in the editor and on unsupported platforms. Respects the user's
    /// haptics setting.
    /// </summary>
    public static class Haptics
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaObject _vibrator;
        private static bool _initTried;

        private static AndroidJavaObject Vibrator
        {
            get
            {
                if (_vibrator == null && !_initTried)
                {
                    _initTried = true;
                    try
                    {
                        using var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                        using var activity = player.GetStatic<AndroidJavaObject>("currentActivity");
                        _vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                    }
                    catch { _vibrator = null; }
                }
                return _vibrator;
            }
        }
#endif

        /// <summary>Vibrate for the given duration in milliseconds.</summary>
        public static void Vibrate(long milliseconds)
        {
            if (!SaveSystem.Data.settings.haptics) return;
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                var v = Vibrator;
                if (v == null) return;
                using var effectClass = new AndroidJavaClass("android.os.VibrationEffect");
                using var effect = effectClass.CallStatic<AndroidJavaObject>(
                    "createOneShot", milliseconds, -1 /* DEFAULT_AMPLITUDE */);
                v.Call("vibrate", effect);
            }
            catch { /* haptics are best-effort; never crash gameplay */ }
#endif
        }

        public static void Light() => Vibrate(10);
        public static void Medium() => Vibrate(20);
        public static void Heavy() => Vibrate(40);
    }
}
