using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilesetManager : MonoBehaviour
{
    public Texture2D tileset;
    [Tooltip("Row n Column")]
    public Vector2Int sizeMap;

    public Button buttonSample;
    public Transform listButton;
    [System.Serializable]
    public class Tile
    {
        public int index;
        public Texture2D texture;
    }
    public Tile[] tiles;

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
        for (int i = 0; i < tiles.Length; i++)
        {
            Button button = Instantiate(buttonSample, listButton);
            Texture2D tex = tiles[i].texture;
            button.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
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

                Texture2D tex = new Texture2D(sizeTexture.x, sizeTexture.y);
                Color[] tex_pixels = new Color[sizeTexture.x * sizeTexture.y];

                for (int ty = 0; ty < sizeTexture.y; ty++)
                {
                    for (int tx = 0; tx < sizeTexture.x; tx++)
                    {
                        int tex_index = ty * sizeTexture.x + tx;
                        int indexTexture = (ty + (y* sizeTexture.y)) * tileset.width + (tx + (x * sizeTexture.x));
                        tex_pixels[tex_index] = pixels[indexTexture];
                    }
                }
                tex.SetPixels(tex_pixels);
                tex.Apply();
                tiles[index].texture = tex;
            }
        }
    }
}
