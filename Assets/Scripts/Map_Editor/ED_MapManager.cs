using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KawaSquad.Tween;

public class ED_MapManager : MonoBehaviour
{
    public static ED_MapManager instance;

    MapDataMof currentMap = null;

    [Header("Popup new map")]
    public TweenAlphagroup alphaNewGroup;
    public InputField inpSizeX;
    public InputField inpSizeY;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Instance already assigned");
        instance = this;
    }


    public void OpenNewMap()
    {
        alphaNewGroup.PlayForward();
        alphaNewGroup.AlphaGroup.interactable = true;
        alphaNewGroup.AlphaGroup.blocksRaycasts = true;
    }
    public void CloseNewMap()
    {
        alphaNewGroup.PlayBackward();
        alphaNewGroup.AlphaGroup.interactable = false;
        alphaNewGroup.AlphaGroup.blocksRaycasts = false;
    }

    public void CreateNewMap()
    {

        bool isChecked = true;
        if (inpSizeX.text == "")
            isChecked = false;
        if (inpSizeY.text == "")
            isChecked = false;


        if (isChecked)
        {
            if (currentMap != null)
            {
                Debug.Log("Existing map");
                Destroy(currentMap.gameObject);
            }
            GameObject inst = new GameObject("MapDataModif");
            inst.transform.parent = this.transform;

            currentMap = inst.AddComponent<MapDataMof>();
            int sizeX = 1;
            int sizeY = 1;

            int.TryParse(inpSizeX.text, out sizeX);
            int.TryParse(inpSizeY.text, out sizeY);

            currentMap.sizeMap = new Vector2Int(sizeX, sizeY);
            CloseNewMap();
        }
    }
    public void LoadMap()
    {

    }
    public void SaveMap()
    {

    }
}
