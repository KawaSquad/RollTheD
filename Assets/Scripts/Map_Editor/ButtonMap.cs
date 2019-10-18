using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KawaSquad.Network;

public class ButtonMap : MonoBehaviour
{
    public Text textMapName;
    public string mapName = "Map_01";
    public string mapPath = "Map_01.json";

    public void OnClick()
    {
        if(ED_MapManager.instance != null)
            ED_MapManager.instance.LoadMap(mapPath);
        if (MapLoader.instance != null)
        {
            MapLoader.instance.LoadMap(mapPath,false);

            if(NetworkManager.instance != null)
            {
                DataSender.SendLoadMap(mapPath);
            }
        }
    }
}
