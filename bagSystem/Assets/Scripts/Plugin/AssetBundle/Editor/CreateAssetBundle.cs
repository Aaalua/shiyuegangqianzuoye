using UnityEngine;
using UnityEditor;
using System.IO;

///<summary>
///打包
///</summary>

public class CreateAssetBundle
{


    [MenuItem("Build/Build AssetBundle")]
    private static void BuildAssetBundles()
    {
        string assetBundlePath = Application.streamingAssetsPath;
        Debug.Log("output path: " + assetBundlePath);
        //检查路径
        if (!Directory.Exists(assetBundlePath))
            Directory.CreateDirectory(assetBundlePath);
        Debug.Log("开始打包");
        /*UncompressedAssetBundle:
        1、UncompressedAssetBundle	Don't compress the data when creating the asset bundle.
        2、ChunkBasedCompression	Use chunk-based LZ4 compression when creating the AssetBundle.*/
#if UNITY_ANDROID
    BuildPipeline.BuildAssetBundles(
            assetBundlePath,
            BuildAssetBundleOptions.UncompressedAssetBundle,
            BuildTarget.Android);
#elif UNITY_IPHONE
    BuildPipeline.BuildAssetBundles(
            assetBundlePath,
            BuildAssetBundleOptions.UncompressedAssetBundle,
            BuildTarget.ios);
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        BuildPipeline.BuildAssetBundles(
            assetBundlePath,
            BuildAssetBundleOptions.UncompressedAssetBundle,
            BuildTarget.StandaloneWindows64);
#endif
        Debug.Log("打包成功");

    }
}