using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetsBundlesManager : MonoBehaviour
{
    public static AssetsBundlesManager instance;

    public CanvasGroup maskLoader;
    public Text textFeedback;

    private void Awake()
    {
        instance = this;
        ActiveLoading(false);
    }

    void ActiveLoading(bool isActive)
    {
        maskLoader.alpha = (isActive) ? 1f : 0f;
        maskLoader.blocksRaycasts = isActive;
        maskLoader.interactable = isActive;
    }
    void SetLoadingContent(string content = "")
    {
        textFeedback.text = content;
    }

    public void LoadAssetBundle(string bundleUrl, DataBaseManager.OnAssetBundleLoaded onAssetBundleLoaded)
    {
        ActiveLoading(true);
        DataBaseManager.OnAssetBundleLoaded isLoaded = new DataBaseManager.OnAssetBundleLoaded(OnBundleReceived);
        isLoaded += onAssetBundleLoaded;
        DataBaseManager.Instance.DownloadBundle(bundleUrl, isLoaded, new DataBaseManager.OnDownloadProgress(OnBundleProgress));
    }


    void OnBundleReceived(AssetBundle bundle)
    {
        ActiveLoading(false);
        SetLoadingContent();
    }
    void OnBundleProgress(float value)
    {
        int percent = (int)(value * 100);
        SetLoadingContent("Download asset bundle : " + percent.ToString("D2") + "%");
    }
}
