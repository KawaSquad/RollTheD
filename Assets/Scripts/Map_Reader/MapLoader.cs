using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public Material tileset;
    public Vector2 sizeTileset = Vector2.one;
    public Vector2Int sizeMap = Vector2Int.one;
    GameObject chunck = null;

    void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (chunck != null)
                Destroy(chunck);
            GenerateMap();
        }
    }

    public void GenerateMap()
    {
        chunck = new GameObject("Chunck");
        MeshFilter meshFilter = chunck.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = chunck.AddComponent<MeshRenderer>();

        int numberVertices = (sizeMap.x) * (sizeMap.y) * 4;
        int numberTriangles = (sizeMap.x) * (sizeMap.y) * 6;

        Vector3[] vertices = new Vector3[numberVertices];
        Vector2[] uvs = new Vector2[numberVertices];
        int[] triangles = new int[numberTriangles];

        int indexTriangles = 0;
        for (int y = 0; y < sizeMap.y; y++)
        {
            for (int x = 0; x < sizeMap.x; x++)
            {
                int index = (sizeMap.x) * (y * 2) + (x * 2);

                //UL
                int newIndex = index;
                vertices[newIndex] = new Vector3(x, 0, y);
                uvs[newIndex] = new Vector2(x / sizeTileset.x, y / sizeTileset.y);
                //UR
                newIndex = index + 1;
                vertices[newIndex] = new Vector3(x + 1, 0, y);
                uvs[newIndex] = new Vector2((x + 1) / sizeTileset.x, y / sizeTileset.y);
                //DL
                newIndex = index + (sizeMap.x * 2);
                vertices[newIndex] = new Vector3(x, 0, y + 1);
                uvs[newIndex] = new Vector2(x / sizeTileset.x, (y + 1) / sizeTileset.y);
                //DR
                newIndex = index + (sizeMap.x * 2) + 1;
                vertices[newIndex] = new Vector3(x + 1, 0, y + 1);
                uvs[newIndex] = new Vector2((x + 1) / sizeTileset.x, (y + 1) / sizeTileset.y);

                triangles[indexTriangles + 0] = index;
                triangles[indexTriangles + 1] = index + (sizeMap.x * 2);
                triangles[indexTriangles + 2] = index + 1;

                triangles[indexTriangles + 3] = index + (sizeMap.x * 2);
                triangles[indexTriangles + 4] = index + (sizeMap.x * 2) + 1;
                triangles[indexTriangles + 5] = index + 1;

                indexTriangles += 6;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;
        meshRenderer.sharedMaterial = tileset;
    }
}
