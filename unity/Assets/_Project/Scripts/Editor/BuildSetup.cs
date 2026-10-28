using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace Zifir.EditorTools
{
    /// <summary>
    /// One-click Google Play configuration and AAB build.
    ///
    /// Zifir → Android → Apply Player Settings: identifier, landscape-only,
    /// IL2CPP + ARM64, min/target SDK, ASTC textures — everything Play needs
    /// except your personal keystore (configure that once in Player Settings →
    /// Publishing Settings).
    ///
    /// Zifir → Android → Build AAB: produces Builds/Zifir.aab.
    /// </summary>
    public static class BuildSetup
    {
        private const string AppIdentifier = "com.altaowxgm.zifir";
        private const string CompanyName = "ALTA OwXGM";
        private const string ProductName = "ZIFIR";
        private const string OutputPath = "Builds/Zifir.aab";

        [MenuItem("Zifir/Android/Apply Player Settings")]
        public static void ApplyPlayerSettings()
        {
            PlayerSettings.companyName = CompanyName;
            PlayerSettings.productName = ProductName;
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, AppIdentifier);

            // Orientation: landscape only (both directions, auto-rotate between them).
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;

            // Google Play requirements: IL2CPP + ARM64, modern SDK levels.
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

            // Offline game: never pause audio/logic for network, no useless permissions.
            PlayerSettings.Android.forceInternetPermission = false;

            // Rendering / performance. (Fullscreen is Unity's Android default;
            // the deprecated startInFullscreen flag is intentionally not touched.)
            PlayerSettings.SetIl2CppCompilerConfiguration(NamedBuildTarget.Android, Il2CppCompilerConfiguration.Release);
            PlayerSettings.Android.renderOutsideSafeArea = true;

            EditorUserBuildSettings.buildAppBundle = true;
            AssetDatabase.SaveAssets();
            Debug.Log("[Zifir] Android player settings applied. Set your keystore in " +
                      "Player Settings → Publishing Settings, then run Zifir → Android → Build AAB.");
        }

        [MenuItem("Zifir/Android/Build AAB")]
        public static void BuildAab()
        {
            ApplyPlayerSettings();

            string dir = Path.GetDirectoryName(OutputPath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

            var options = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/Scenes/SampleScene.unity" },
                locationPathName = OutputPath,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            var report = UnityEditor.BuildPipeline.BuildPlayer(options);
            Debug.Log($"[Zifir] AAB build finished: {report.summary.result} → {OutputPath} " +
                      $"({report.summary.totalSize / (1024 * 1024)} MB)");
        }
    }
}
