using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;

    public Text textLoading;
    CanvasGroup canvasFade;

    static bool sWaitToStop = false;
    static float sTimeToStop = 10f;

    public delegate void OnDurationStop();
    private static OnDurationStop onDurationStop = null;

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
    private void Update()
    {
        if (sWaitToStop)
        {
            sTimeToStop -= Time.deltaTime;
            if (sTimeToStop < 0f)
            {
                if (onDurationStop != null)
                    onDurationStop();
                StopLoading();
                sWaitToStop = false;
            }
        }
    }

    private static void SetDurationStop(float duration)
    {
        sWaitToStop = true;
        sTimeToStop = duration;
    }


    public static void ActiveLoading(string content , bool isLoading = true, float duration = 0f, OnDurationStop onDurationStop = null)//isloading to keep the text "loading :"
    {
        if (instance == null)
            return;

        if (instance.canvasFade != null)
        {
            instance.canvasFade.alpha = 1f;
            instance.canvasFade.blocksRaycasts = true;

            if(isLoading)
                instance.textLoading.text = "Loading : " + content;
            else
                instance.textLoading.text = content;
        }

        if (duration != 0f)
            SetDurationStop(duration);

        LoadingScreen.onDurationStop = onDurationStop;
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
