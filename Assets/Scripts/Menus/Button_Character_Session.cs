using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Character_Session : MonoBehaviour
{
    [SerializeField]
    private Text mFieldName = null;
    [SerializeField]
    private Text mFieldClass = null;
    [SerializeField]
    private Text mFieldAccount = null;
    [SerializeField]
    private Image mImgPreview = null;

    [SerializeField]
    private Image mImgLoading = null;

    private string urlImage = "https://steven-sternberger.be/RollTheD/Ressources/unknow.png";
    private bool isDestroyed = false;

    public void SetCharacter(string characterName, string characterClass, string characterAccount, string characterImage = "")
    {
        mFieldName.text = characterName;
        mFieldClass.text = characterClass;
        mFieldAccount.text = characterAccount;
        if (characterImage != "")
        {
            urlImage = characterImage;
        }

        mImgLoading.enabled = true;
        mImgLoading.fillAmount = 0f;

        DataBaseManager.Instance.DownloadImage(urlImage, new DataBaseManager.OnImageLoaded(OnImageLoaded), new DataBaseManager.OnDownloadProgress(OnDownloadProgress));
    }

    void OnImageLoaded(Texture2D textureRequested)
    {
        if (isDestroyed)
            return;

        if (textureRequested == null)
        {
            Debug.Log("Icon null");
            return;
        }

        Rect rectSize = new Rect(0, 0, textureRequested.width, textureRequested.height);
        mImgPreview.sprite = Sprite.Create(textureRequested, rectSize, Vector2.one / 2f);

        mImgLoading.enabled = false;
        mImgLoading.fillAmount = 1f;
    }
    void OnDownloadProgress(float value)
    {
        if(mImgLoading != null)
            mImgLoading.fillAmount = value;
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }
}
