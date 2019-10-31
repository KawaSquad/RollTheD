using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundle_Creator : MonoBehaviour
{
    public AssetBundleContainer sampleContainer;
    public string mBundleName = "tileschunk";

    void TEST_LOAD()
    {
        string urlContent = DataBaseManager.DataBase + "Sessions/NovaStorm/Bundles/"+ mBundleName;
        AssetsBundlesManager.instance.LoadAssetBundle(urlContent, new DataBaseManager.OnAssetBundleLoaded(OnBundleReceived));
    }

    IEnumerator CreateAllRessources(AssetBundle bundle)
    {
        AssetBundleContainer container = Instantiate(sampleContainer);
        container.name = "Container_" + mBundleName;

        AssetBundleRequest bundleRequest = bundle.LoadAllAssetsAsync();
        while (!bundleRequest.isDone)
        {
            //Progress
            float progress = bundleRequest.progress;
            yield return 0;
        }
        Object[] objects = bundleRequest.allAssets;
        container.textures = new List<Texture2D>();

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetType() == (typeof(Texture2D)))
            {
                container.textures.Add((Texture2D)objects[i]);
            }
        }

        yield break;
    }

    void OnBundleReceived(AssetBundle bundle)
    {
        if (bundle != null)
            StartCoroutine(CreateAllRessources(bundle));
        else
            Debug.Log("FAIL AssetBundle");
    }
}
