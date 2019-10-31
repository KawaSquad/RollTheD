using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BundleBuilder : Editor
{
    [MenuItem("KawaSquad/Rebuild Alll AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string output = Path.GetFullPath(Path.Combine(Application.dataPath, "AssetsBundles"));
        BuildPipeline.BuildAssetBundles(output,BuildAssetBundleOptions.ChunkBasedCompression,BuildTarget.StandaloneWindows64);
    }


    public void CreateNewBundle(AssetBundleBuild[] assets)
    {
        string output = Path.GetFullPath(Path.Combine(Application.dataPath, "AssetsBundles"));
        BuildPipeline.BuildAssetBundles(output, assets, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
    }
}
