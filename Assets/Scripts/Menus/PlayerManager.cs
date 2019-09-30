using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Player")]
    public InputField inpPlayerName;
    public InputField inpPlayerPassword;

    [Header("New Player Account")]
    public InputField inpNewPlayerName;
    public InputField inpNewPlayerPassword;

    [Header("New Player Account")]
    [SerializeField]
    private SObject_Player playerData;

    private const string DATABASE = "https://steven-sternberger.be/RollTheD/";

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
        public string Password_Account = "pswd";
    }


    #region Player account
    Coroutine coWebReq;
    public void CheckPlayerAccount()
    {
        if (inpPlayerName.text != "" && inpPlayerPassword.text != "")
        {
            if (coWebReq != null)
                StopCoroutine(coWebReq);
            coWebReq = StartCoroutine(CheckOnWeb());
        }
    }

    IEnumerator CheckOnWeb()
    {            
        //Check Database
        string url = DATABASE + "Login.php";

        WWWForm forms = new WWWForm();
        forms.AddField("loginPost", inpPlayerName.text);
        string encodedPswd = inpPlayerPassword.text.GetHashCode().ToString();
        forms.AddField("passwordPost", encodedPswd);

        UnityWebRequest webRequest = UnityWebRequest.Post(url, forms);

        UnityWebRequestAsyncOperation handler = webRequest.SendWebRequest();

        float timeIn = 0f;
        float timeOut = 10f;
        LoadingScreen.ActiveLoading("Check account");
        while (!handler.isDone)
        {
            timeIn += Time.deltaTime;
            if (timeIn > timeOut)
            {
                //Security
                break;
            }
            //Animate here
            yield return null;
        }

        //fakeDelay
        float maxTime = 3f;
        if(timeIn< maxTime)
            yield return new WaitForSeconds(maxTime - maxTime);

        LoadingScreen.StopLoading();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            string content = webRequest.downloadHandler.text;
            JsonRequest request = JsonUtility.FromJson<JsonRequest>(content);

            if(request.success == "true")
            {
                Content_Account contentAccount = JsonUtility.FromJson<Content_Account>(request.content);

                playerData.ID_Account = contentAccount.ID_Account;
                playerData.Name_Account = contentAccount.Name_Account;
                playerData.Password_Account = contentAccount.Password_Account;

                MenuManager.Instance.ActiveState(EMenuState.Player_Session);
            }
        }


        yield break;
    }

    #endregion

    #region New Player
    public void OpenNewPlayer()
    {
        MenuManager.Instance.ActiveState(EMenuState.Player_Account);
    }
    public void CreateNewPlayer()
    {
        if(inpNewPlayerName.text != "" && inpNewPlayerPassword.text != "")
        {
            if (coWebReq != null)
                StopCoroutine(coWebReq);
            coWebReq = StartCoroutine(CheckOnWeb());
        }
    }

    IEnumerator AddOnWeb()
    {
        //Check Database
        string url = DATABASE + "NewAccount.php";

        WWWForm forms = new WWWForm();
        forms.AddField("loginPost", inpPlayerName.text);
        string encodedPswd = inpPlayerPassword.text.GetHashCode().ToString();
        forms.AddField("passwordPost", encodedPswd);

        UnityWebRequest webRequest = UnityWebRequest.Post(url, forms);

        UnityWebRequestAsyncOperation handler = webRequest.SendWebRequest();

        float timeIn = 0f;
        float timeOut = 10f;
        LoadingScreen.ActiveLoading("Create new account");
        while (!handler.isDone)
        {
            timeIn += Time.deltaTime;
            if (timeIn > timeOut)
            {
                //Security
                break;
            }
            //Animate here
            yield return null;
        }

        //fakeDelay
        float maxTime = 3f;
        if (timeIn < maxTime)
            yield return new WaitForSeconds(maxTime - maxTime);

        LoadingScreen.StopLoading();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            string content = webRequest.downloadHandler.text;
            JsonRequest request = JsonUtility.FromJson<JsonRequest>(content);

            if (request.success == "true")
            {
                /*
                Content_Account contentAccount = JsonUtility.FromJson<Content_Account>(request.content);

                playerData.ID_Account = contentAccount.ID_Account;
                playerData.Name_Account = contentAccount.Name_Account;
                playerData.Password_Account = contentAccount.Password_Account;
                 */

                MenuManager.Instance.ActiveState(EMenuState.Player_Connection);
            }
        }


        yield break;
    }

    #endregion
}
