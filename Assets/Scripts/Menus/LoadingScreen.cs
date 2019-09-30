using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;

    public Text textLoading;
    CanvasGroup canvasFade;

        private void Awake()
    {
        if (instance != null)
            Debug.LogError("Instance already assigned");
        instance = this;
    }
    void Start()
    {
        canvasFade = this.GetComponent<CanvasGroup>();
        StopLoading();
    }

    public static void ActiveLoading(string content)
    {
        if (instance == null)
            return;

        if (instance.canvasFade != null)
        {
            instance.canvasFade.alpha = 1f;
            instance.canvasFade.blocksRaycasts = true;

            instance.textLoading.text = "Loading : " + content;
        }
    }
    public static void StopLoading()
    {
        if (instance == null)
            return;

        if (instance.canvasFade != null)
        {
            instance.canvasFade.alpha = 0f;
            instance.canvasFade.blocksRaycasts = false;

            instance.textLoading.text = "";
        }
    }
}
