using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilesetManager : MonoBehaviour
{
    public static TilesetManager instance;

    public Texture2D tileset;
    [Tooltip("Row n Column")]
    public Vector2Int sizeMap;

    public ButtonTile buttonSample;
    public Transform listButton;
    [System.Serializable]
    public class Tile
    {
        public int index;
        public Texture2D texture;
        public Vector2 uv_UL;
        public Vector2 uv_UR;
        public Vector2 uv_DL;
        public Vector2 uv_DR;
    }
    public Tile[] tiles;

    public bool deployTileset = false;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Instance already assigned"); 
        instance = this;
    }
    void Start()
    {
        SplitTexture();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateButtons();

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SplitTexture();
        }
    }

    public void CreateButtons()
    {
        if (deployTileset)
        {
            Button[] buttons = listButton.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }
        }
        else
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                ButtonTile button = Instantiate(buttonSample, listButton);
                Texture2D tex = tiles[i].texture;
                button.indexTexture = i;
                button.SetImage(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f)));
            }
        }
        deployTileset = !deployTileset;
    }
    public void SplitTexture()
    {
        int numberTile = sizeMap.x * sizeMap.y;
        tiles = new Tile[numberTile];
        Vector2Int sizeTexture = new Vector2Int(tileset.width / sizeMap.x, tileset.height / sizeMap.y);
        Color[] pixels = tileset.GetPixels();

        for (int y = 0; y < sizeMap.y; y++)
        {
            for (int x = 0; x < sizeMap.x; x++)
            {
                int index = y * sizeMap.x + x;
                tiles[index] = new Tile();
                tiles[index].index = index;
                tiles[index].uv_UL = new Vector2((float)x / (float)sizeMap.x, (float)y / (float)sizeMap.y);
                tiles[index].uv_UR = new Vector2((float)x / (float)sizeMap.x + (1f / (float)sizeMap.x), (float)y / (float)sizeMap.y);
                tiles[index].uv_DL = new Vector2((float)x / (float)sizeMap.x, (float)y / (float)sizeMap.y + (1f / (float)sizeMap.y));
                tiles[index].uv_DR = new Vector2((float)x / (float)sizeMap.x + (1f / (float)sizeMap.x), (float)y / (float)sizeMap.y + (1f / (float)sizeMap.y));

                Texture2D tex = new Texture2D(sizeTexture.x, sizeTexture.y);
                Color[] tex_pixels = new Color[sizeTexture.x * sizeTexture.y];

                for (int ty = 0; ty < sizeTexture.y; ty++)
                {
                    for (int tx = 0; tx < sizeTexture.x; tx++)
                    {
                        int tex_index = ty * sizeTexture.x + tx;
                        int indexTexture = (ty + (y * sizeTexture.y)) * tileset.width + (tx + (x * sizeTexture.x));
                        tex_pixels[tex_index] = pixels[indexTexture];
                    }
                }
                tex.filterMode = FilterMode.Point;
                tex.SetPixels(tex_pixels);
                tex.Apply();
                tiles[index].texture = tex;
            }
        }
    }
}
