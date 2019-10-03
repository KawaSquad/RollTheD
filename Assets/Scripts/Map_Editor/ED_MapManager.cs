using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KawaSquad.Tween;
using UnityEngine.EventSystems;

public class ED_MapManager : MonoBehaviour
{
    public static ED_MapManager instance;

    MapDataMof currentMap = null;

    [Header("Tools buttons")]
    public Button bNewMap;
    public Button bSaveMap;
    public Button bLoadMap;
    [Space()]
    public Button bTileset;

    [Header("Popup new map")]
    public TweenAlphagroup alphaNewMap;
    public InputField inpSizeX;
    public InputField inpSizeY;

    [Header("Popup save map")]
    public TweenAlphagroup alphaSaveMap;
    public InputField inpMapName;

    [Header("Popup load map")]
    public TweenAlphagroup alphaLoadMap;
    public ButtonMap bMapName;
    public Transform listMaps;

    [Header("Map data")]
    public Material tilesetMat;
    public Camera cameraControl;
    Vector3 mousePos;
    public int currentIndexTexture = 0;

    public EventSystem ui_Eventsystem;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Instance already assigned");
        instance = this;

        UpdateToolsButtons();
    }

    void UpdateToolsButtons()
    {
        bool mapExist = (currentMap != null);
        bSaveMap.gameObject.SetActive(mapExist);
        bTileset.gameObject.SetActive(mapExist);
    }

    private void Update()
    {
        if (currentMap != null)
        {
            //left mouse click
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (ui_Eventsystem.IsPointerOverGameObject())
                    return;

                Ray ray = cameraControl.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray,out hitInfo))
                {
                    TileData mapData = hitInfo.collider.GetComponent<TileData>();
                    if(mapData != null)
                    {
                        if(mapData.indexTile != currentIndexTexture)
                            mapData.SetIndex(currentIndexTexture);
                    }
                }
            }

            //midle mouse click
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                mousePos = Input.mousePosition;
            }
            if (Input.GetKey(KeyCode.Mouse2))
            {
                Vector3 mouvement = mousePos - Input.mousePosition;
                cameraControl.transform.localPosition += new Vector3(mouvement.x, 0, mouvement.y) * Time.deltaTime;

                mousePos = Input.mousePosition;
            }


            //Scroll mouse
            cameraControl.transform.localPosition += new Vector3(0, -Input.mouseScrollDelta.y, 0);
        }
    }

    public void OpenNewMap()
    {
        alphaNewMap.PlayForward();
        alphaNewMap.AlphaGroup.interactable = true;
        alphaNewMap.AlphaGroup.blocksRaycasts = true;
    }
    public void CloseNewMap()
    {
        alphaNewMap.PlayBackward();
        alphaNewMap.AlphaGroup.interactable = false;
        alphaNewMap.AlphaGroup.blocksRaycasts = false;
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
            currentMap.isMapEditor = true;
            int sizeX = 1;
            int sizeY = 1;

            int.TryParse(inpSizeX.text, out sizeX);
            int.TryParse(inpSizeY.text, out sizeY);

            currentMap.CreateMap(new Vector2Int(sizeX, sizeY), tilesetMat);
            CloseNewMap();
        }

        UpdateToolsButtons();
        CenterMap();
    }

    public void OpenSaveMap()
    {
        alphaSaveMap.PlayForward();
        alphaSaveMap.AlphaGroup.interactable = true;
        alphaSaveMap.AlphaGroup.blocksRaycasts = true;
    }
    public void CloseSaveMap()
    {
        alphaSaveMap.PlayBackward();
        alphaSaveMap.AlphaGroup.interactable = false;
        alphaSaveMap.AlphaGroup.blocksRaycasts = false;
    }
    public void SaveMap()
    {
        string pathFolder = Path.Combine(Application.persistentDataPath, "Saves");
        string pathFile = Path.Combine(pathFolder, inpMapName.text + ".json");

        if(!Directory.Exists(pathFolder))
        {
            Directory.CreateDirectory(pathFolder);
        }

        string json = currentMap.ToJson;
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(json);

        if (File.Exists(pathFile))
            File.Delete(pathFile);
        FileStream stream = File.Create(pathFile);
        stream.Write(buffer,0, buffer.Length);
        stream.Close();

        CloseSaveMap();
    }

    public void OpenLoadMap()
    {
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
            ButtonMap bMap = Instantiate(bMapName,listMaps);
            bMap.mapName = Path.GetFileNameWithoutExtension(files[i]);
            bMap.mapPath = files[i];
            bMap.textMapName.text = bMap.mapName;
        }
    }
    public void CloseLoadMap()
    {
        alphaLoadMap.PlayBackward();
        alphaLoadMap.AlphaGroup.interactable = false;
        alphaLoadMap.AlphaGroup.blocksRaycasts = false;
    }
    public void LoadMap(string mapPath)
    {
        if(!File.Exists(mapPath))
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
        currentMap.CreateMap(new Vector2Int(jsonMapData.sizeMap.x, jsonMapData.sizeMap.y), tilesetMat);
        currentMap.LoadMap(jsonMapData);

        CenterMap();
        UpdateToolsButtons();
        CloseLoadMap();
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
