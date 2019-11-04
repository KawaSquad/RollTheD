using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilesetManager : MonoBehaviour
{
    public static TilesetManager instance;

    [System.Serializable]
    public class Tileset
    {
        public Texture2D tileset;
        [Tooltip("Row n Column")]
        public Vector2Int sizeMap;

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

        //public bool deployTileset = false;

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

    public Tileset tileset_1;
    public Tileset tileset_2;
    public Tileset lighting;
    public Tileset navigation;

    public static Tileset tileset_deployed;


    public ButtonTile buttonSample;
    public Transform listButton;

    public Tileset LayerTileset(int layer)
    {
        Tileset tilesetSelected = null;
        switch (layer)
        {
            case 0:
                tilesetSelected = tileset_1;
                break;
            case 1:
                tilesetSelected = tileset_2;
                break;
            case 2:
                tilesetSelected = lighting;
                break;
            case 3:
                tilesetSelected = navigation;
                break;
            default:
                break;
        }
        return tilesetSelected;
    }

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Instance already assigned"); 
        instance = this;
    }
    void Start()
    {
        tileset_1.SplitTexture();
        tileset_2.SplitTexture();
        lighting.SplitTexture();
        navigation.SplitTexture();
    }

    public void SetTileset(int layerTielset, Tileset newTileset)
    {
        switch (layerTielset)
        {
            case 0:
                tileset_1 = newTileset;
                break;
            case 1:
                tileset_2 = newTileset;
                break;
            case 2:
                lighting = newTileset;
                break;
            case 3:
                navigation = newTileset;
                break;
            default:
                break;
        }
        newTileset.SplitTexture();
    }

    public void CreateButtons(int layer)
    {
        Tileset tilesetSelected = null;
        switch (layer)
        {
            case 0:
                tilesetSelected = tileset_1;
                break;
            case 1:
                tilesetSelected = tileset_2;
                break;
            case 2:
                tilesetSelected = lighting;
                break;
            case 3:
                tilesetSelected = navigation;
                break;
            default:
                break;
        }

        bool isDeployed = true;
        if (tileset_deployed != null)
        {
            Button[] buttons = listButton.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }

            if (tileset_deployed == tilesetSelected)
                isDeployed = false;

            tileset_deployed = null;
        }

        if (isDeployed)
        {
            for (int i = 0; i < tilesetSelected.tiles.Length; i++)
            {
                ButtonTile button = Instantiate(buttonSample, listButton);
                Texture2D tex = tilesetSelected.tiles[i].texture;
                button.indexTexture = i;
                button.SetImage(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f)));
            }

            tileset_deployed = tilesetSelected;
        }
    }

        /*
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
    */

}
