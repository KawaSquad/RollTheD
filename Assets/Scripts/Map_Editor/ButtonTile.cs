using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTile : MonoBehaviour
{
    public Image imageView;
    public int indexTexture = 0;

    public void OnClick()
    {
        ED_MapManager.instance.currentIndexTexture = indexTexture;
    }

    public void SetImage(Sprite img)
    {
        imageView.sprite = img;
    }
}
