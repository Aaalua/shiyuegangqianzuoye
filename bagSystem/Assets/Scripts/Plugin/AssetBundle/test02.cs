using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test02 : MonoBehaviour
{
    private IEnumerator Start()
    {
        Debug.Log("test assetbundle load,it can Instantiate a red cube.");
        LoadAssetBundle.Instance.LoadAsset("StreamingAssets", "model", "Cube");
        while (!LoadAssetBundle.Instance.isDone)
        {
            Debug.Log("wait ab load");
            yield return new WaitForSeconds(0.5f);

        }
        Debug.Log("load ab finish");
        Instantiate(LoadAssetBundle.Instance.Asset, Vector3.zero, Quaternion.identity);
    }

}
