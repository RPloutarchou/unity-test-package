using System.IO;
using UnityEditor;

public class BuildAssetBundles
{
    [MenuItem("Bundles/Build iOS Asset Bundle")]
    static void BuildiOSAssetBundle()
    {
        string iOSAssetBundleDirectory = "Assets/StreamingAssets/iOS";
        if (!Directory.Exists(iOSAssetBundleDirectory)) { Directory.CreateDirectory(iOSAssetBundleDirectory); }
        BuildPipeline.BuildAssetBundles(iOSAssetBundleDirectory, BuildAssetBundleOptions.StrictMode, BuildTarget.iOS);
    }

    [MenuItem("Bundles/Build Android Asset Bundle")]
    static void BuildAndroidAssetBundle()
    {
        string AndroidAssetBundleDirectory = "Assets/StreamingAssets/Android";
        if (!Directory.Exists(AndroidAssetBundleDirectory)) { Directory.CreateDirectory(AndroidAssetBundleDirectory); }
        BuildPipeline.BuildAssetBundles(AndroidAssetBundleDirectory, BuildAssetBundleOptions.StrictMode, BuildTarget.Android);
    }

    [MenuItem("Bundles/Build WebGL Asset Bundle")]
    static void BuildWebGLAssetBundle()
    {
        string WebGLAssetBundleDirectory = "Assets/StreamingAssets/WebGL";
        if (!Directory.Exists(WebGLAssetBundleDirectory)) { Directory.CreateDirectory(WebGLAssetBundleDirectory); }
        BuildPipeline.BuildAssetBundles(WebGLAssetBundleDirectory, BuildAssetBundleOptions.StrictMode, BuildTarget.WebGL);
    }
}