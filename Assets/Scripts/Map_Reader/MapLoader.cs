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

    GameObject chunck = null;
    MapDataMof currentMap = null;
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

    public void LoadMap(string mapPath)
    {
        if (!File.Exists(mapPath))
        {
            return;
        }
        string json = File.ReadAllText(mapPath);
        MapDataMof.JsonMapData jsonMapData = JsonUtility.FromJson<MapDataMof.JsonMapData>(json);

        if (currentMap != null)
        {
            Debug.Log("Existing map");
            Destroy(currentMap.gameObject);
        }
        GameObject inst = new GameObject("MapDataModif");
        inst.transform.parent = this.transform;
        currentMap = inst.AddComponent<MapDataMof>();
        currentMap.isMapEditor = false;
        //currentMap.LoadMap(jsonMapData);
        currentMap.GenerateMap(jsonMapData, tilesetMat);
        //currentMap.CreateMap(new Vector2Int(jsonMapData.sizeMap.x, jsonMapData.sizeMap.y), tilesetMat);

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

        string pathFolder = Path.Combine(Application.persistentDataPath, "Saves");
        string[] files = Directory.GetFiles(pathFolder);
        for (int i = 0; i < files.Length; i++)
        {
            ButtonMap bMap = Instantiate(bMapName, listMaps);
            bMap.mapName = Path.GetFileNameWithoutExtension(files[i]);
            bMap.mapPath = files[i];
            bMap.textMapName.text = bMap.mapName;
        }
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
