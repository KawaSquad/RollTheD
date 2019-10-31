using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssetsBundles : MonoBehaviour
{
    AssetBundle mAssetBundle;
    public string mBundleType = "tileschunck";
    private void Start()
    {
        LoadAssetBundle(Path.GetFullPath(Path.Combine(Application.dataPath, "StreamingAssets", mBundleType)));
        if(mAssetBundle != null)    
            OpenAssetBundle();
    }

    void LoadAssetBundle(string bundleUrl)
    {
        mAssetBundle = AssetBundle.LoadFromFile(bundleUrl);
        Debug.Log("Progress : " + (mAssetBundle != null));
    }


    void OpenAssetBundle()
    {
        Object[] objects =  mAssetBundle.LoadAllAssets();
        for (int i = 0; i < objects.Length; i++)
        {
            Texture2D tex = (Texture2D)objects[i];
            if(tex != null)
                Debug.Log("Object : " + objects[i].name);
        }
    }
}
