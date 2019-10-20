using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KawaSquad.Network;

public class ItemSelector : MonoBehaviour
{
    public Image visual;
    public List<Sprite> sprites;

    public int id_Token;

    public void SetVisual(int id_Token)
    {
        this.id_Token = id_Token;
        if (id_Token < sprites.Count)
        {
            visual.sprite = sprites[id_Token];
        }
    }

    public void OnClick()
    {
        if(AdventureManager.Instance != null)
        {
            if (PlayerHandle.LocalPlayerHandle != null)
                AdventureManager.Instance.CreateItem(PlayerHandle.LocalPlayerHandle.connectionID, id_Token);
            else
                Debug.LogError("error localPlayer");
        }
    }
}
