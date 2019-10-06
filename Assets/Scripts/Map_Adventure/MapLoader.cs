using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KawaSquad.Tween;

public class MapLoader : MonoBehaviour
{
    public static MapLoader instance;

    [Header("Map data")]
    public Material tilesetMat;
    public Camera cameraControl;

    [Header("Popup load map")]
    public TweenAlphagroup alphaLoadMap;
    public ButtonMap bMapName;
    public Transform listMaps;

    private MapDataMof currentMap = null;

    public SObject_Player sessionData;

    public static MapDataMof CurrentMap
    {
        get
        {
            if (instance == null)
                return null;
            else
                return instance.currentMap;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void LoadMap(string mapPath, bool isLocalSave = true)
    {
        if (isLocalSave)
        {
            if (!File.Exists(mapPath))
            {
                return;
            }
            string json = File.ReadAllText(mapPath);
            MapDataMof.JsonMapData jsonMapData = JsonUtility.FromJson<MapDataMof.JsonMapData>(json);
            GenrateMap(jsonMapData);
        }
        else
        {
            DataBaseManager.Instance.DownloadSave(mapPath,new DataBaseManager.OnTextLoaded(MapDownloaded),null);
        }
    }
    void MapDownloaded(string json)
    {
        MapDataMof.JsonMapData jsonMapData = JsonUtility.FromJson<MapDataMof.JsonMapData>(json);
        GenrateMap(jsonMapData);
    }

    public void GenrateMap(MapDataMof.JsonMapData jsonMapData)
    {
        if (currentMap != null)
        {
            Debug.Log("Existing map");
            Destroy(currentMap.gameObject);
        }

        GameObject inst = new GameObject("MapDataModif");
        inst.transform.parent = this.transform;
        currentMap = inst.AddComponent<MapDataMof>();
        currentMap.isMapEditor = false;
        currentMap.GenerateMap(jsonMapData, tilesetMat);

        CenterMap();
        //UpdateToolsButtons();
        CloseLoadMap();
    }

    public void OpenLoadMap()
    {
        alphaLoadMap.ResetAtBeginning();
        alphaLoadMap.PlayForward();
        alphaLoadMap.AlphaGroup.interactable = true;
        alphaLoadMap.AlphaGroup.blocksRaycasts = true;

        ButtonMap[] bMaps = listMaps.GetComponentsInChildren<ButtonMap>();
        for (int i = 0; i < bMaps.Length; i++)
        {
            Destroy(bMaps[i].gameObject);
        }


        for (int i = 0; i < sessionData.Maps.Length; i++)
        {
            string url_Map = sessionData.GM_Url + "Maps/" + sessionData.Maps[i];
            string fileName = sessionData.Maps[i].Split('.')[0];

            ButtonMap bMap = Instantiate(bMapName, listMaps);
            bMap.mapName = fileName;
            bMap.mapPath = url_Map;
            bMap.textMapName.text = bMap.mapName;
        }

        /*
        string pathFolder = Path.Combine(Application.persistentDataPath, "Saves");
        string[] files = Directory.GetFiles(pathFolder);
        for (int i = 0; i < files.Length; i++)
        {
            ButtonMap bMap = Instantiate(bMapName, listMaps);
            bMap.mapName = Path.GetFileNameWithoutExtension(files[i]);
            bMap.mapPath = files[i];
            bMap.textMapName.text = bMap.mapName;
        }
         */
    }
    public void CloseLoadMap()
    {
        alphaLoadMap.ResetAtTheEnd();
        alphaLoadMap.PlayBackward();
        alphaLoadMap.AlphaGroup.interactable = false;
        alphaLoadMap.AlphaGroup.blocksRaycasts = false;
    }

    void CenterMap()
    {
        if (currentMap == null)
            return;

        Vector3 localPos = cameraControl.transform.localPosition;
        localPos.x = currentMap.sizeMap.x / 2f;
        localPos.z = currentMap.sizeMap.y / 2f;
        cameraControl.transform.localPosition = localPos;
    }

}
