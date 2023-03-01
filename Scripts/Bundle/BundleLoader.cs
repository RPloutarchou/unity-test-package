using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BundleLoader : MonoBehaviour
{
    public string assetName;
    public string bundleName;
    public BundleLoaderMode loadMode;

    private void Start()
    {
        if (assetName == null) { Debug.LogWarning("Bundle Loader Warning: Asset Name Not Set"); return; }
        if (bundleName == null) { Debug.LogWarning("Bundle Loader Warning: Bundle Name Not Set"); return; }
        switch (loadMode)
        {
            case BundleLoaderMode.Web:
                Debug.LogWarning("Bundle Loader Warning: Trying To Load From Web");
                StartCoroutine(LoadFromWeb());
                break;
            case BundleLoaderMode.Local:
                Debug.LogWarning("Bundle Loader Warning: Trying To Load From Local");
                LoadFromLocal();
                break;
            case BundleLoaderMode.LocalAsync:
                Debug.LogWarning("Bundle Loader Warning: Trying To Load From Local Async");
                StartCoroutine(LoadFromLocalAsync());
                break;
            default:
                Debug.LogWarning("Bundle Loader Warning: Load Mode Not Set");
                break;
        }
    }

    private void LoadAsset(GameObject asset)
    {
        Debug.LogWarning("Bundle Loader Warning: Asset Loaded");
        Transform canvas = GameObject.Find("Canvas").transform;
        Instantiate(asset, canvas);
    }

    private IEnumerator LoadFromWeb()
    {
        var bundleUrl = "https://rafaello-dev.spectrum.games/assets/bundles/" + bundleName;
#if (UNITY_ANDROID)
        bundleUrl = bundleUrl + "/android/" + bundleName;
#endif
#if (UNITY_IOS)
        bundleUrl = bundleUrl + "/ios/" + bundleName;
#endif
        UnityWebRequest webAssetBundleRequest = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl);
        yield return webAssetBundleRequest.SendWebRequest();
        AssetBundle remoteAssetBundle = DownloadHandlerAssetBundle.GetContent(webAssetBundleRequest);
        if (remoteAssetBundle == null) { Debug.LogWarning("Bundle Loader Warning: Web Bundle Could Not Be Loaded"); yield break; }
        GameObject asset = remoteAssetBundle.LoadAsset<GameObject>(assetName);
        LoadAsset(asset);
        remoteAssetBundle.Unload(false);
    }

    private void LoadFromLocal()
    {
#if (UNITY_ANDROID)
        AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath + "/Android", bundleName));
#endif
#if (UNITY_IOS)
        AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath + "/iOS", bundleName));
#endif
        if (localAssetBundle == null) { Debug.LogWarning("Bundle Loader Warning: Local Bundle Could Not Be Loaded"); return; }
        GameObject asset = localAssetBundle.LoadAsset<GameObject>(assetName);
        LoadAsset(asset);
        localAssetBundle.Unload(false);
    }

    private IEnumerator LoadFromLocalAsync()
    {
#if (UNITY_ANDROID)
        AssetBundleCreateRequest localAssetBundleRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath + "/Android", bundleName));
#endif
#if (UNITY_IOS)
        AssetBundleCreateRequest localAssetBundleRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath + "/iOS", bundleName));
#endif
        yield return localAssetBundleRequest;
        AssetBundle localAssetBundle = localAssetBundleRequest.assetBundle;
        if (localAssetBundle == null) { Debug.LogWarning("Bundle Loader Warning: Local Async Bundle Could Not Be Loaded"); yield break; }
        AssetBundleRequest assetRequest = localAssetBundle.LoadAssetAsync<GameObject>(assetName);
        yield return assetRequest;
        GameObject asset = assetRequest.asset as GameObject;
        LoadAsset(asset);
        localAssetBundle.Unload(false);
    }
}

public enum BundleLoaderMode
{
    Web,
    Local,
    LocalAsync
}