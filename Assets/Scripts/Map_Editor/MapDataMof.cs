using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataMof : MonoBehaviour
{
    public Vector2Int sizeMap;

    public List<TileData> mapData;
    public bool isMapEditor = true;
    private int mTypeMap = 0;
    

    public Mesh CreateQuad()
    {
        Mesh meshQuad = new Mesh();

        Vector3[] vertices = new Vector3[4];//4 corners
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(1, 0, 0);
        vertices[2] = new Vector3(0, 0, 1);
        vertices[3] = new Vector3(1, 0, 1);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);

        triangles[0] = 1;
        triangles[1] = 0;
        triangles[2] = 2;

        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;

        meshQuad.vertices = vertices;
        meshQuad.uv = uv;
        meshQuad.triangles = triangles;

        meshQuad.RecalculateNormals();
        return meshQuad;
    }

    public void CreateMap(Vector2Int sizeMap, Material tilesetMat)
    {
        this.sizeMap = sizeMap;
        //this.mTypeMap = mapType;

        mapData = new List<TileData>();
        Mesh meshQuad = CreateQuad();

        if (mapData != null)
        {
            /*
                        for (int i = 0; i < mapData.Count; i++)
                        {
                            Destroy(mapData[i].gameObject);
                        }
             */
            mapData.Clear();
        }

        for (int y = 0; y < sizeMap.y; y++)
        {
            for (int x = 0; x < sizeMap.x; x++)
            {
                int index = y * sizeMap.x + x;

                GameObject go = new GameObject("Tile_" + index);
                go.transform.localPosition = new Vector3(x, 0, y);
                go.transform.parent = this.transform;

                TileData tile = go.AddComponent<TileData>();
                tile.SetTile(index, 0, tilesetMat);
                tile.meshFilter.mesh = meshQuad;

                mapData.Add(tile);
            }
        }
    }

    public void CreateEditorMap(Vector2Int sizeMap, int mapType, Material tilesetMat)
    {
        this.sizeMap = sizeMap;
        mTypeMap = mapType;
        mapData = new List<TileData>();

        Mesh meshQuad = CreateQuad();

        switch (mapType)
        {
            case 1:
                CreateMap(sizeMap, tilesetMat);
                break;
            case 2:
                //LATER
                break;
            case 3:
                CreateMap(sizeMap, tilesetMat);
                /*
                GameObject go = new GameObject("FullMap");
                go.transform.localPosition = new Vector3(0, 0, 0);
                go.transform.localScale = new Vector3(sizeMap.x, 1, sizeMap.y);
                go.transform.parent = this.transform;

                TileData tile = go.AddComponent<TileData>();
                tile.SetTile(0, 0, tilesetMat);
                tile.meshFilter.mesh = meshQuad;

                mapData.Add(tile);
                 */
                break;
            default:
                break;
        }
    }

    public void GenerateMap(JsonMapData jsonMapData, Material tilesetMat)
    {
        sizeMap = jsonMapData.sizeMap;

        GameObject chunck = new GameObject("Chunck");
        chunck.transform.parent = this.transform;
        MeshFilter meshFilter = chunck.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = chunck.AddComponent<MeshRenderer>();

        int numberVertices = (jsonMapData.sizeMap.x) * (jsonMapData.sizeMap.y) * 4;
        int numberTriangles = (jsonMapData.sizeMap.x) * (jsonMapData.sizeMap.y) * 6;

        Vector3[] vertices = new Vector3[numberVertices];
        Vector2[] uvs = new Vector2[numberVertices];
        int[] triangles = new int[numberTriangles];

        int indexTriangles = 0;

        mapData = new List<TileData>();
        for (int y = 0; y < jsonMapData.sizeMap.y; y++)
        {
            for (int x = 0; x < jsonMapData.sizeMap.x; x++)
            {
                int index = (jsonMapData.sizeMap.x * 2) * (y * 2) + (x * 2);
                int indexMap = (jsonMapData.sizeMap.x) * y + x;

                int indexTile = jsonMapData.tileData[indexMap].tileLayer.layer1;

                if (indexTile != -1 && indexTile != 255)
                {
                    TilesetManager.Tileset tileset = TilesetManager.instance.LayerTileset(0);
                    TilesetManager.Tileset.Tile tile = tileset.tiles[indexTile];

                    GameObject goTile = new GameObject("Tile " + indexMap);
                    goTile.transform.parent = this.transform;

                    TileData tileData = goTile.AddComponent<TileData>();
                    tileData.data.indexMap = indexMap;
                    tileData.data.tileLayer.layer1 = indexTile;
                    mapData.Add(tileData);

                    //UL
                    int newIndex = index;
                    vertices[newIndex] = new Vector3(x, 0, y);
                    uvs[newIndex] = tile.uv_UL;
                    //uvs[newIndex] = new Vector2(x, y);
                    //UR
                    newIndex = index + 1;
                    vertices[newIndex] = new Vector3(x + 1, 0, y);
                    uvs[newIndex] = tile.uv_UR;
                    //uvs[newIndex] = new Vector2((x + 1), y);
                    //DL
                    newIndex = index + (jsonMapData.sizeMap.x * 2);
                    vertices[newIndex] = new Vector3(x, 0, y + 1);
                    uvs[newIndex] = tile.uv_DL;
                    //uvs[newIndex] = new Vector2(x, (y + 1));
                    //DR
                    newIndex = index + (jsonMapData.sizeMap.x * 2) + 1;
                    vertices[newIndex] = new Vector3(x + 1, 0, y + 1);
                    uvs[newIndex] = tile.uv_DR;
                    //uvs[newIndex] = new Vector2((x + 1), (y + 1));

                }
                triangles[indexTriangles + 0] = index;
                triangles[indexTriangles + 1] = index + (jsonMapData.sizeMap.x * 2);
                triangles[indexTriangles + 2] = index + 1;

                triangles[indexTriangles + 3] = index + (jsonMapData.sizeMap.x * 2);
                triangles[indexTriangles + 4] = index + (jsonMapData.sizeMap.x * 2) + 1;
                triangles[indexTriangles + 5] = index + 1;

                indexTriangles += 6;
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "ChunckMesh";
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;
        meshRenderer.sharedMaterial = tilesetMat;
    }


    public string ToJson
    {
        get
        {
            JsonMapData data = new JsonMapData();
            data.tileData = new List<JsonTileData>();
            data.sizeMap = sizeMap;

            for (int i = 0; i < mapData.Count; i++)
            {
                JsonTileData jsonTile = new JsonTileData();
                jsonTile.indexMap = mapData[i].data.indexMap;
                jsonTile.tileLayer.layer1 = mapData[i].data.tileLayer.layer1;
                data.tileData.Add(jsonTile);
            }
            string json = JsonUtility.ToJson(data);
            return json;
        }
    }

    public byte[] GetTextureData
    {
        get
        {
            Texture2D finalTexture = new Texture2D(sizeMap.x, sizeMap.y);
            Color32[] pixelsData = finalTexture.GetPixels32();

            for (int y = 0; y < sizeMap.y; y++)
            {
                for (int x = 0; x < sizeMap.x; x++)
                {
                    int index = y * sizeMap.x + x;
                    byte r = (byte)mapData[index].data.tileLayer.layer1;
                    byte g = (byte)mapData[index].data.tileLayer.layer2;
                    byte b = (byte)mapData[index].data.tileLayer.layer3;
                    byte a = (byte)mapData[index].data.tileLayer.layer4;
                    pixelsData[index] = new Color32(r, g, b, a);
                }
            }

            finalTexture.SetPixels32(pixelsData);
            finalTexture.Apply();

            byte[] buffer = finalTexture.EncodeToPNG();
            return buffer;
        }
    }

    public void LoadMap(JsonMapData jsonData)
    {
        for (int i = 0; i < jsonData.tileData.Count; i++)
        {
            mapData[i].data.indexMap = jsonData.tileData[i].indexMap;
            mapData[i].data.tileLayer.layer1 = jsonData.tileData[i].tileLayer.layer1;
            mapData[i].SetIndex(mapData[i].data.tileLayer.layer1);
        }
    }
}

[System.Serializable]
public class JsonMapData
{
    public Vector2Int sizeMap;
    public List<JsonTileData> tileData;
}

[System.Serializable]
public class JsonTileData
{
    public int indexMap = 0;//0 -> (size map * size map)
    public TileLayer tileLayer = new TileLayer();

    public class TileLayer
    {
        public int layer1 = 0;//Tileset_1
        public int layer2 = 0;//Tileset_2
        public int layer3 = 0;//Light => ?
        public int layer4 = 0;//Navigation
    }
}
