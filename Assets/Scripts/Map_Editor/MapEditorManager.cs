using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using KawaSquad.Tween;
using UnityEngine.EventSystems;

public class MapEditorManager : MonoBehaviour
{
    public static MapEditorManager instance;

    MapDataMof currentMap = null;

    [Header("Tools buttons")]
    public Button bNewMap;
    public Button bSaveMap;
    public Button bLoadMap;
    [Space()]
    public Button bTileset_1;
    public Button bTileset_2;
    public Button bLighting;
    public Button bNavigation;

    [Header("Popup new map")]
    //public TweenAlphagroup alphaNewMap;
    public Inputfield_Form inpSizeX;
    public Inputfield_Form inpSizeY;

    [Header("Popup save map")]
    //public TweenAlphagroup alphaSaveMap;
    public InputField inpMapName;

    [Header("Popup load map")]
    //public TweenAlphagroup alphaLoadMap;
    public ButtonMap bMapName;
    public Transform listMaps;

    [Header("Map data")]
    public Material tilesetMat;
    public TilesetManager.Tileset tilesetTile;
    public Material chunckMat;
    public TilesetManager.Tileset tilesetChunck;
    public Material fullMat;
    public TilesetManager.Tileset tilesetFull;

    public Camera cameraControl;
    Vector3 mousePos;
    public int currentIndexTexture = 0;

    public EventSystem ui_Eventsystem;
    public static int mTypeMap = 0;

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
        bTileset_1.gameObject.SetActive(mapExist);
        bTileset_2.gameObject.SetActive(mapExist);
        bLighting.gameObject.SetActive(mapExist);
        bNavigation.gameObject.SetActive(mapExist);
    }

    private void Update()
    {
        if (currentMap != null)
        {
            //left mouse click
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                if (ui_Eventsystem.IsPointerOverGameObject())
                    return;
                bool isLeft = Input.GetMouseButton(1);
                Ray ray = cameraControl.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray,out hitInfo))
                {
                    TileData mapData = hitInfo.collider.GetComponent<TileData>();
                    if(mapData != null)
                    {
                        if (isLeft || mapData.data.tileLayer.layer1 != currentIndexTexture)
                            mapData.SetIndex((isLeft) ? -1 : currentIndexTexture);
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


    public void OpenNewMap(int typeMap)
    {
        if(MenuManager.Instance != null)
        {
            mTypeMap = typeMap;
            MenuManager.Instance.ActiveState(EMenuState.Editor_Size_Map);
        }
    }
    public void CreateNewMap()
    {
        bool isChecked = true;
        if (!inpSizeX.Validate())
            isChecked = false;
        if (!inpSizeY.Validate())
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

            int.TryParse(inpSizeX.Content, out sizeX);
            int.TryParse(inpSizeY.Content, out sizeY);

            if(mTypeMap == 2)
            {
                Debug.Log("NotDoneYet");
            }

            Material materialSelected = null;
            TilesetManager.Tileset tilesetTarget = null;
            switch (mTypeMap)
            {
                case 1:
                    materialSelected = tilesetMat;
                    tilesetTarget = tilesetTile;
                    break;
                case 2:
                    materialSelected = chunckMat;
                    tilesetTarget = tilesetChunck;
                    break;
                case 3:
                    materialSelected = fullMat;
                    tilesetTarget = tilesetFull;
                    break;
                default:
                    break;
            }
            //currentMap.CreateMap(new Vector2Int(sizeX, sizeY), tilesetMat);
            TilesetManager.instance.SetTileset(0,tilesetTarget);
            currentMap.CreateEditorMap(new Vector2Int(sizeX, sizeY), mTypeMap, materialSelected);
            MenuManager.Instance.ActiveState(EMenuState.Editor_Menu);

            UpdateToolsButtons();
            CenterMap();
        }
    }

    public void SaveMap()
    {
        string pathFolder = Path.Combine(Application.persistentDataPath, "Saves");
        string pathFile = Path.Combine(pathFolder, inpMapName.text + ".json");
        string pathData = Path.Combine(pathFolder, inpMapName.text + ".png");

        if (!Directory.Exists(pathFolder))
        {
            Directory.CreateDirectory(pathFolder);
        }

        string json = currentMap.ToJson;
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(json);
        byte[] bufferData = currentMap.GetTextureData;

        if (File.Exists(pathFile))
            File.Delete(pathFile);


        FileStream stream = File.Create(pathFile);
        stream.Write(buffer,0, buffer.Length);
        stream.Close();

        if (File.Exists(pathData))
            File.Delete(pathData);

        FileStream streamData = File.Create(pathData);
        streamData.Write(bufferData, 0, bufferData.Length);
        streamData.Close();

        MenuManager.Instance.PreviousPanel();
    }

    public void OpenLoadMap()
    {
        ButtonMap[] bMaps = listMaps.GetComponentsInChildren<ButtonMap>();
        for (int i = 0; i < bMaps.Length; i++)
        {
            Destroy(bMaps[i].gameObject);
        }

        string pathFolder = Path.Combine(Application.persistentDataPath, "Saves");
        if (!Directory.Exists(pathFolder))
            Directory.CreateDirectory(pathFolder);

        string[] files = Directory.GetFiles(pathFolder,"*.json");
        for (int i = 0; i < files.Length; i++)
        {
            ButtonMap bMap = Instantiate(bMapName,listMaps);
            bMap.mapName = Path.GetFileNameWithoutExtension(files[i]);
            bMap.filePath = files[i];
            bMap.dataPath = files[i].Replace("json", "png");
            bMap.textMapName.text = bMap.mapName;
        }
    }

    public void LoadMap(string filePath,string dataPath)
    {
        if(!File.Exists(filePath) || !File.Exists(dataPath))
        {
            return;
        }

        string json = File.ReadAllText(filePath);
        JsonMapData jsonMapData = JsonUtility.FromJson<JsonMapData>(json);

        byte[] bufferData = File.ReadAllBytes(dataPath);
        Texture2D textureData = new Texture2D(1, 1);
        textureData.LoadImage(bufferData);
        Color32[] pixelsData = textureData.GetPixels32();

        for (int y = 0; y < jsonMapData.sizeMap.y; y++)
        {
            for (int x = 0; x < jsonMapData.sizeMap.x; x++)
            {
                int index = y * jsonMapData.sizeMap.x + x;
                JsonTileData tile = new JsonTileData();
                tile.indexMap = index;
                tile.tileLayer.layer1 = pixelsData[index].r;
                tile.tileLayer.layer2 = pixelsData[index].g;
                tile.tileLayer.layer3 = pixelsData[index].b;
                tile.tileLayer.layer4 = pixelsData[index].a;
                jsonMapData.tileData[index] = tile;
            }
        }

        if (currentMap != null)
        {
            Debug.Log("Existing map");
            Destroy(currentMap.gameObject);
        }
        GameObject inst = new GameObject("MapDataModif");
        inst.transform.parent = this.transform;
        currentMap = inst.AddComponent<MapDataMof>();

        Material materialSelected = null;
        switch (mTypeMap)
        {
            case 1:
                materialSelected = tilesetMat;
                break;
            case 2:
                materialSelected = chunckMat;
                break;
            case 3:
                materialSelected = fullMat;
                break;
            default:
                break;
        }
        //currentMap.CreateMap(new Vector2Int(jsonMapData.sizeMap.x, jsonMapData.sizeMap.y), tilesetMat);
        currentMap.CreateEditorMap(new Vector2Int(jsonMapData.sizeMap.x, jsonMapData.sizeMap.y),mTypeMap, materialSelected);
        currentMap.LoadMap(jsonMapData);

        CenterMap();
        UpdateToolsButtons();
        MenuManager.Instance.PreviousPanel();
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
