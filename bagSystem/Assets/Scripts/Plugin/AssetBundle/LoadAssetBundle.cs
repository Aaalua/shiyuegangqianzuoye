using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetBundle : MonoBehaviour
{
    private static LoadAssetBundle _instance = null;
    public static LoadAssetBundle Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(LoadAssetBundle)) as LoadAssetBundle;
                if (_instance == null)
                {
                    _instance = new GameObject("SingletonManager_" + typeof(LoadAssetBundle).Name, typeof(LoadAssetBundle)).GetComponent<LoadAssetBundle>();
                }

            }
            return _instance;
        }
    }

    private bool _isDone = false;
    public bool isDone
    {
        get
        {
            if (asset != null || allAssets != null)
            {
                _isDone = true;
            }
            else
            {
                _isDone = false;
            }
            return _isDone;
        }
    }
    private Object asset = null;
    public Object Asset
    {
        get
        {
            Object temp = asset;
            asset = null;
            _isDone = false;
            return temp;

        }

        set
        {
            asset = value;
        }
    }
    private Object[] allAssets = null;
    public Object[] AllAssets
    {
        get
        {
            Object[] temps = allAssets;
            allAssets = null;
            _isDone = false;
            return temps;
        }

        set
        {
            allAssets = value;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this as LoadAssetBundle;
        Init();
    }
    private void OnApplicationQuit()
    {
        _instance = null;
    }

    private string pathURL;


    public void Init()
    {
        pathURL =
#if UNITY_ANDROID
            "jar:file//" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
        Application.dataPath + "/Raw/";
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        "file://" + Application.streamingAssetsPath + "/";
#endif
    }

    /// <summary>
    /// 直接从目标ab包中加载需求资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public IEnumerator LoadAB(string path, string assetBundle, string res)
    {
        UnityWebRequest uwr = UnityWebRequest.GetAssetBundle(pathURL + path);
        yield return uwr.SendWebRequest();
        if (!string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log("main manifest file load error");
        }
        else
        {
            // Get an asset from the bundle and instantiate it.
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
            AssetBundleRequest loadAsset = bundle.LoadAssetAsync<GameObject>(res);
            yield return loadAsset;
            asset = loadAsset.asset;
            bundle.Unload(false);
        }
    }

    /// <summary>
    /// 有依赖的文件资源加载方法
    /// </summary>
    /// <param name="path">读取主ab文件,获取所有ab资源文件信息</param>
    /// <param name="assetBundle">资源所在的ab包</param>
    /// <param name="res">资源名</param>
    /// <returns></returns>

    public void LoadAsset(string path, string assetBundle, string res)
    {
        StartCoroutine(LoadAsset(path, assetBundle, res, true));
    }
    private IEnumerator LoadAsset(string path, string assetBundle, string res, bool b)
    {
        string mUrl = pathURL + path;
        //获取主文件信息
        UnityWebRequest uwr = UnityWebRequest.GetAssetBundle(mUrl);
        yield return uwr.SendWebRequest();
        if (!string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log("main manifest file load error");
        }
        else
        {
            AssetBundle mab = DownloadHandlerAssetBundle.GetContent(uwr);
            AssetBundleManifest manifest = mab.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            mab.Unload(false);
            //获取需要的依赖文件
            //先加载依赖文件
            string[] dps = manifest.GetAllDependencies(assetBundle);
            AssetBundle[] abs = new AssetBundle[dps.Length];
            for (int i = 0; i < abs.Length; i++)
            {
                string subUrl = pathURL + dps[i];
                UnityWebRequest dwww = UnityWebRequest.GetAssetBundle(subUrl);
                yield return dwww.SendWebRequest();
                abs[i] = DownloadHandlerAssetBundle.GetContent(dwww);
            }
            //获取资源所在的ab包
            //加载需求资源
            UnityWebRequest resUwr = UnityWebRequest.GetAssetBundle(pathURL + assetBundle);
            yield return resUwr.SendWebRequest();
            if (!string.IsNullOrEmpty(resUwr.error))
            {
                Debug.Log("Load res ab error : " + resUwr.error);
            }
            else
            {
                AssetBundle ab = DownloadHandlerAssetBundle.GetContent(resUwr);
                AssetBundleRequest loadAsset = ab.LoadAssetAsync(res);
                yield return loadAsset;
                if (loadAsset.isDone)
                    this.asset = loadAsset.asset;
                ab.Unload(false);
            }
            foreach (var t in abs)
            {
                t.Unload(false);
            }
        }

    }
    /// <summary>
    /// 加载一个包里的所有资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="assetBundle"></param>
    public void LoadAllAsset(string path, string assetBundle)
    {
        StartCoroutine(LoadAllAsset(path, assetBundle, true));
    }
    private IEnumerator LoadAllAsset(string path, string assetBundle, bool b)
    {
        string mUrl = pathURL + path;
        //获取主文件信息
        UnityWebRequest uwr = UnityWebRequest.GetAssetBundle(mUrl);
        yield return uwr.SendWebRequest();
        if (!string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log("main manifest file load error");
        }
        else
        {
            AssetBundle mab = DownloadHandlerAssetBundle.GetContent(uwr);
            AssetBundleManifest manifest = mab.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            mab.Unload(false);
            //获取需要的依赖文件
            //先加载依赖文件
            string[] dps = manifest.GetAllDependencies(assetBundle);
            AssetBundle[] abs = new AssetBundle[dps.Length];
            for (int i = 0; i < abs.Length; i++)
            {
                string subUrl = pathURL + dps[i];
                UnityWebRequest dwww = UnityWebRequest.GetAssetBundle(subUrl);
                yield return dwww.SendWebRequest();
                abs[i] = DownloadHandlerAssetBundle.GetContent(dwww);
            }
            //获取资源所在的ab包
            //加载需求资源
            UnityWebRequest resUwr = UnityWebRequest.GetAssetBundle(pathURL + assetBundle);
            yield return resUwr.SendWebRequest();
            if (!string.IsNullOrEmpty(resUwr.error))
            {
                Debug.Log("Load res ab error : " + resUwr.error);
            }
            else
            {
                AssetBundle ab = DownloadHandlerAssetBundle.GetContent(resUwr);
                AssetBundleRequest loadAsset = ab.LoadAllAssetsAsync();
                yield return loadAsset;
                this.allAssets = loadAsset.allAssets;
                ab.Unload(false);
            }
            foreach (var t in abs)
            {
                t.Unload(false);
            }
        }
    }

}
