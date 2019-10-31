using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class AssetBundle_Creator : MonoBehaviour
{
    public AssetBundleContainer sampleContainer;
    public string mBundleName = "tileschunk";

    public Inputfield_Form titleField;
    public Image contentSample;
    public Texture2D assetContentSample;

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



    public void CreateNewBundle()
    {
        if (titleField.Validate() == false)
        {
            Debug.LogError("EmptyField");
            return;
        }

        AssetBundleBuild[] bundleMap = new AssetBundleBuild[1];

        AssetBundleBuild assetBundle = new AssetBundleBuild();
        assetBundle.assetBundleName = titleField.Content;
        string assetPath = AssetDatabase.GetAssetPath(assetContentSample);
        string[] assetsContent = new string[] { assetPath };
        assetBundle.assetNames = assetsContent;

        bundleMap[0] = assetBundle;

        CreateNewBundle(bundleMap);
    }


    public void CreateNewBundle(AssetBundleBuild[] assets)
    {
        string output = Path.GetFullPath(Path.Combine(Application.dataPath, "AssetsBundles"));
        BuildPipeline.BuildAssetBundles(output, assets, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
    }
}
