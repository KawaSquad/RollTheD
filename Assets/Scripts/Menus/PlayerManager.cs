using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Player")]
    public Inputfield_Form inpPlayerName;
    public Inputfield_Form inpPlayerPassword;

    [Header("New Player Account")]
    public Inputfield_Form inpNewPlayerName;
    public Inputfield_Form inpNewPlayerPassword;

    [Header("SObject Player Data")]
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
        inpPlayerName.DisplayError("");
        inpPlayerPassword.DisplayError("");

        if (inpPlayerName.Validate() && inpPlayerPassword.Validate())
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
        forms.AddField("loginPost", inpPlayerName.Content);
        string encodedPswd = inpPlayerPassword.Content.GetHashCode().ToString();
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
        float maxTime = 1f;
        if(timeIn< maxTime)
            yield return new WaitForSeconds(maxTime - timeIn);

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
            else
            {
                inpPlayerPassword.DisplayError(request.error);
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
        inpNewPlayerName.DisplayError("");
        inpNewPlayerPassword.DisplayError("");

        if (inpNewPlayerName.Validate() && inpNewPlayerPassword.Validate())
        {
            if (coWebReq != null)
                StopCoroutine(coWebReq);
            coWebReq = StartCoroutine(AddOnWeb());
        }
    }

    IEnumerator AddOnWeb()
    {
        //Check Database
        string url = DATABASE + "NewAccount.php";

        WWWForm forms = new WWWForm();
        forms.AddField("loginPost", inpNewPlayerName.Content);
        string encodedPswd = inpNewPlayerPassword.Content.GetHashCode().ToString();
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
        float maxTime = 1f;
        if (timeIn < maxTime)
            yield return new WaitForSeconds(maxTime - timeIn);


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
                LoadingScreen.ActiveLoading("New Account Created",false);
                yield return new WaitForSeconds(1f);

                MenuManager.Instance.ActiveState(EMenuState.Player_Connection);
            }
            else
            {
                inpNewPlayerPassword.DisplayError(request.error);
            }
        }

        LoadingScreen.StopLoading();


        yield break;
    }
    #endregion

    public void ResetForms_NewAccount()
    {
        inpNewPlayerName.ResetField();
        inpNewPlayerPassword.ResetField();
    }
    public void ResetForms_Connection()
    {
        inpPlayerName.ResetField();
        inpPlayerPassword.ResetField();
    }
}
