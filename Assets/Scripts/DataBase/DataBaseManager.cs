﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataBaseManager : MonoBehaviour
{
    private static DataBaseManager instance;
    public static DataBaseManager Instance
    {
        get
        {
            if(instance == null)
                Debug.LogError("Instance not set yet");
            return instance;
        }
    }


    private const string DATABASE = "https://steven-sternberger.be/RollTheD/";



    static Coroutine coWebReq;
    public delegate void OnRequestEnd(JsonRequest requested);
    public delegate void OnImageLoaded(Texture2D textureRequested);
    public delegate void OnDownloadProgress(float value);

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Instance already assigned");
        instance = this;
    }

    public void Login_Account(string accountName, string accountPassword, OnRequestEnd onRequestEnd)
    {
        string url = DATABASE + "Login.php";

        WWWForm forms = new WWWForm();
        forms.AddField("loginPost", accountName.ToLower());
        string encodedPswd = accountPassword.GetHashCode().ToString();
        forms.AddField("passwordPost", encodedPswd);

        if (coWebReq != null)
            StopCoroutine(coWebReq);
        coWebReq = StartCoroutine(RequestWeb(url, forms, 10f, "Check account", onRequestEnd));
    }
    public void Create_Account(string accountName, string accountPassword, OnRequestEnd onRequestEnd)
    {
        string url = DATABASE + "NewAccount.php";

        WWWForm forms = new WWWForm();
        forms.AddField("loginPost", accountName.ToLower());
        string encodedPswd = accountPassword.GetHashCode().ToString();
        forms.AddField("passwordPost", encodedPswd);

        if (coWebReq != null)
            StopCoroutine(coWebReq);
        coWebReq = StartCoroutine(RequestWeb(url, forms, 10f, "Check account", onRequestEnd));
    }
    public void Session_List(OnRequestEnd onRequestEnd)
    {
        string url = DATABASE + "SessionList.php";

        WWWForm forms = new WWWForm();
        //forms.AddField("loginPost", accountName);
        //string encodedPswd = accountPassword.GetHashCode().ToString();
        //forms.AddField("passwordPost", encodedPswd);

        if (coWebReq != null)
            StopCoroutine(coWebReq);
        coWebReq = StartCoroutine(RequestWeb(url, forms, 10f, "Refresh session list", onRequestEnd));
    }
    public void CreateNewSession(string nameSession, string masterSession, string ipSession, OnRequestEnd onRequestEnd)
    {
        string url = DATABASE + "NewSession.php";

        WWWForm forms = new WWWForm();
        forms.AddField("nameSessionPost", nameSession);
        forms.AddField("masterSessionPost", masterSession);
        forms.AddField("ipSessionPost", ipSession);

        if (coWebReq != null)
            StopCoroutine(coWebReq);
        coWebReq = StartCoroutine(RequestWeb(url, forms, 10f, "Create new session", onRequestEnd));
    }
    public void CharacterList(int idSession,OnRequestEnd onRequestEnd)
    {
        string url = DATABASE + "CharactersLobby.php";

        WWWForm forms = new WWWForm();
        forms.AddField("SessionIDPost", idSession);

        if (coWebReq != null)
            StopCoroutine(coWebReq);
        coWebReq = StartCoroutine(RequestWeb(url, forms, 10f, "Refresh character", onRequestEnd));
    }

    IEnumerator RequestWeb(string url, WWWForm forms, float timeOut,string loadingFeedback,OnRequestEnd onRequestEnd)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(url, forms);
        UnityWebRequestAsyncOperation handler = webRequest.SendWebRequest();

        float timeIn = 0f;
        LoadingScreen.ActiveLoading(loadingFeedback);
        while (!handler.isDone)
        {
            timeIn += Time.deltaTime;
            if (timeIn > timeOut)
            {
                //Security
                webRequest.Abort();
                LoadingScreen.ActiveLoading("TIME OUT");
                yield return new WaitForSeconds(2f);
                break;
            }
            yield return null;
        }

        //FakeDelay
        float minTime = 1f;
        if (timeIn < minTime)
            yield return new WaitForSeconds(minTime - timeIn);
        LoadingScreen.StopLoading();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            //Call end
            string content = webRequest.downloadHandler.text;
            JsonRequest request = JsonUtility.FromJson<JsonRequest>(content);
            onRequestEnd(request);
        }

        yield break;
    }



    public void DownloadImage(string url, OnImageLoaded onImageLoaded, OnDownloadProgress onDownloadProgress)
    {
        //if (coWebReq != null)
        //    StopCoroutine(coWebReq);
        //coWebReq = 
        StartCoroutine(RequestPicture(url, 60f, onImageLoaded, onDownloadProgress));
    }

    IEnumerator RequestPicture(string url, float timeOut, OnImageLoaded onImageLoaded, OnDownloadProgress onDownloadProgress)
    {
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);
        UnityWebRequestAsyncOperation handler = webRequest.SendWebRequest();

        float timeIn = 0f;
        while (!handler.isDone)
        {
            timeIn += Time.deltaTime;
            onDownloadProgress(handler.progress);
            if (timeIn > timeOut)
            {
                //Security
                webRequest.Abort();
                break;
            }
            yield return null;
        }

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
            onImageLoaded(null);
        }
        else
        {
            //Call end
            Texture2D textureRequested = DownloadHandlerTexture.GetContent(webRequest);
            onImageLoaded(textureRequested);
        }

        yield break;
    }

}
[System.Serializable]
public class JsonRequest
{
    public string success;
    public string error;
    public string content;
}

[System.Serializable]
public class Content_Account
{
    public int ID_Account = 0;
    public string Name_Account = "name";
    public string Password_Account = "psw";
}
[System.Serializable]
public class Json_Content_Session
{
    public List<Content_Session> sessions;
}
[System.Serializable]
public class Content_Session
{
    public int ID_Session = 0;
    public string Name_Session = "Name";
    public string Master_Session = "Name";
    public int Number_Player = 0;
    public int Number_Player_Max = 8;
    public string Password_Session = "psw";
    public string IP_Session = "127.0.0.1";
}

[System.Serializable]
public class Json_Content_Lobby
{
    public List<Content_Lobby> characters;
}
[System.Serializable]
public class Content_Lobby
{
    public int ID_Character = 0;
    public int ID_Account = 0;
    public int ID_Session = 0;
    public string Picture_Url = "";
    public string Name_Character = "Name";
    public string Class_Character = "Name";
    public int HP_Character = 10;
}
