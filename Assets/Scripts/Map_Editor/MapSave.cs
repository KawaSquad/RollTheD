using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSave : MonoBehaviour
{
    public Vector2Int sizeMap;

    public Image preview_R;
    public Image preview_G;
    public Image preview_B;
    public Image preview_A;

    [SerializeField]
    public class PixelData
    {
        public int indexL1 = 0;
        public int indexL2 = 0;
        public int indexL3 = 0;
        public int dataNav = 0;
    }

    public void ResetDatas()
    {
        mPixelDatas = new PixelData[sizeMap.x * sizeMap.y];
        for (int i = 0; i < mPixelDatas.Length; i++)
        {
            mPixelDatas[i] = new PixelData();
        }
    }

    public PixelData[] PixelDatas
    {
        get
        {
            if(mPixelDatas == null)
            {
                ResetDatas();
            }
            return mPixelDatas;
        }
    }
    private PixelData[] mPixelDatas;

    public void GenerateMap()
    {
        if (PixelDatas.Length != (sizeMap.x * sizeMap.y))
            return;

        Texture2D newTex_R = new Texture2D(sizeMap.x, sizeMap.y);
        Texture2D newTex_G = new Texture2D(sizeMap.x, sizeMap.y);
        Texture2D newTex_B = new Texture2D(sizeMap.x, sizeMap.y);
        Texture2D newTex_A = new Texture2D(sizeMap.x, sizeMap.y);

        newTex_R.filterMode = FilterMode.Point;
        newTex_R.wrapMode = TextureWrapMode.Clamp;

        newTex_G.filterMode = FilterMode.Point;
        newTex_G.wrapMode = TextureWrapMode.Clamp;

        newTex_B.filterMode = FilterMode.Point;
        newTex_B.wrapMode = TextureWrapMode.Clamp;

        newTex_A.filterMode = FilterMode.Point;
        newTex_A.wrapMode = TextureWrapMode.Clamp;

        Color32[] pixels_R = newTex_R.GetPixels32();
        Color32[] pixels_G = newTex_G.GetPixels32();
        Color32[] pixels_B = newTex_B.GetPixels32();
        Color32[] pixels_A = newTex_A.GetPixels32();

        for (int y = 0; y < sizeMap.y; y++)
        {
            for (int x = 0; x < sizeMap.x; x++)
            {
                int index = y * sizeMap.x + x;

                PixelData pixelData = mPixelDatas[index];

                pixels_R[index] = new Color32((byte)pixelData.indexL1, (byte)pixelData.indexL1, (byte)pixelData.indexL1,(byte)255);
                pixels_G[index] = new Color32((byte)pixelData.indexL2, (byte)pixelData.indexL2, (byte)pixelData.indexL2, (byte)255);
                pixels_B[index] = new Color32((byte)pixelData.indexL3, (byte)pixelData.indexL3, (byte)pixelData.indexL3, (byte)255);
                pixels_A[index] = new Color32((byte)pixelData.dataNav, (byte)pixelData.dataNav, (byte)pixelData.dataNav, (byte)255);
            }
        }

        newTex_R.SetPixels32(pixels_R);
        newTex_G.SetPixels32(pixels_G);
        newTex_B.SetPixels32(pixels_B);
        newTex_A.SetPixels32(pixels_A);

        newTex_R.Apply();
        newTex_G.Apply();
        newTex_B.Apply();
        newTex_A.Apply();

        preview_R.sprite = Sprite.Create(newTex_R, new Rect(0, 0, newTex_R.width, newTex_R.height), Vector2.one / 2f);
        preview_G.sprite = Sprite.Create(newTex_G, new Rect(0, 0, newTex_G.width, newTex_G.height), Vector2.one / 2f);
        preview_B.sprite = Sprite.Create(newTex_B, new Rect(0, 0, newTex_B.width, newTex_B.height), Vector2.one / 2f);
        preview_A.sprite = Sprite.Create(newTex_A, new Rect(0, 0, newTex_A.width, newTex_A.height), Vector2.one / 2f);
    }
}
