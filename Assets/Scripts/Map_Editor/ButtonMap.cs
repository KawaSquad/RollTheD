using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KawaSquad.Network;

public class ButtonMap : MonoBehaviour
{
    public Text textMapName;
    public string mapName = "Map_01";
    public string filePath = "Map_01.json";
    public string dataPath = "Map_01.png";

    public void OnClick()
    {
        if(MapEditorManager.instance != null)
            MapEditorManager.instance.LoadMap(filePath, dataPath);
        if (MapLoader.instance != null)
        {
            MapLoader.instance.LoadMap(filePath, dataPath, false);

            if(NetworkManager.instance != null)
            {
                DataSender.SendLoadMap(filePath, dataPath);
            }
        }
    }
}
