using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataMof : MonoBehaviour
{
    public Vector2Int sizeMap;

    public List<TileData> mapData;
    public bool isMapEditor = true;
    [System.Serializable]
    public class JsonMapData
    {
        public Vector2Int sizeMap;
        [System.Serializable]
        public class JsonTileData
        {
            public int indexMap = 0;
            public int indexTile = 0;
        }
        public List<JsonTileData> mapData;
    }

    public void CreateMap(Vector2Int sizeMap,Material tilesetMat)
    {
        this.sizeMap = sizeMap;
        mapData = new List<TileData>();

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

        if (mapData != null)
        {
            for (int i = 0; i < mapData.Count; i++)
            {
                Destroy(mapData[i].gameObject);

            }
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
                tile.SetTile(index,0,tilesetMat);
                tile.meshFilter.mesh = meshQuad;

                mapData.Add(tile);
            }
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
                int indexTile = jsonMapData.mapData[indexMap].indexTile;

                if (indexTile != -1)
                {

                    TilesetManager.Tile tile = TilesetManager.instance.tiles[indexTile];

                    TileData tileData = new TileData();
                    tileData.indexMap = indexMap;
                    tileData.indexTile = indexTile;
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
            data.mapData = new List<JsonMapData.JsonTileData>();
            data.sizeMap = sizeMap;

            for (int i = 0; i < mapData.Count; i++)
            {
                JsonMapData.JsonTileData jsonTile = new JsonMapData.JsonTileData();
                jsonTile.indexMap = mapData[i].indexMap;
                jsonTile.indexTile = mapData[i].indexTile;
                data.mapData.Add(jsonTile);
            }
            string json = JsonUtility.ToJson(data);
            return json;
        }
    }

    public void LoadMap(JsonMapData jsonData)
    {
        for (int i = 0; i < jsonData.mapData.Count; i++)
        {
            mapData[i].indexMap = jsonData.mapData[i].indexMap;
            mapData[i].indexTile = jsonData.mapData[i].indexTile;
            mapData[i].SetIndex(mapData[i].indexTile);
        }
    }
}
