using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetsBundlesManager : MonoBehaviour
{
    public static AssetsBundlesManager instance;

    AssetBundle mAssetBundle;
    public string mBundleType = "tileschunck";
    public string mBundleUrl = "https://steven-sternberger.be/RollTheD/Sessions/NovaStorm/Bundles/tileschunck";
    public bool isLocal = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (isLocal)
            mBundleUrl = Path.Combine(Application.dataPath, "StreamingAssets", mBundleType);

        LoadAssetBundle(mBundleUrl, isLocal);
    }

    void LoadAssetBundle(string bundleUrl,bool isLocal)
    {
        if(isLocal)
        {
            mAssetBundle = AssetBundle.LoadFromFile(bundleUrl);
            OpenAssetBundle();
        }
        else
            DataBaseManager.Instance.DownloadBundle(bundleUrl, new DataBaseManager.OnAssetBundleLoaded(OnBundleReceived), new DataBaseManager.OnDownloadProgress(OnBundleProgress));
        //mAssetBundle = AssetBundle.LoadFromFile(bundleUrl);
        //Debug.Log("Progress : " + (mAssetBundle != null));
    }


    void OpenAssetBundle()
    {
        Object[] objects =  mAssetBundle.LoadAllAssets();
        for (int i = 0; i < objects.Length; i++)
        {
            Debug.Log("Object : " + objects[i].name);
        }
    }

    IEnumerator CreateAllRessources()
    {
        AssetBundleRequest bundleRequest = mAssetBundle.LoadAllAssetsAsync();

        while (!bundleRequest.isDone)
        {
            //Progress
            float progress = bundleRequest.progress;
            yield return 0;
        }
        Object[] objects = bundleRequest.allAssets;
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetType() == (typeof(Texture2D)))
            {
                Texture2D texture = (Texture2D)objects[i];
                Debug.Log("TEXTURE : " + texture.name);
            }
        }

        yield break;
    }


    void OnBundleProgress(float value)
    {
        Debug.Log("Progress : " + value);
    }
    void OnBundleReceived(AssetBundle bundle)
    {
        if(bundle != null)
        {
            mAssetBundle = bundle;
            StartCoroutine(CreateAllRessources());
            Debug.Log("AssetBundle : " + bundle.name);
        }
        else
            Debug.Log("FAIL AssetBundle");
    }
}
