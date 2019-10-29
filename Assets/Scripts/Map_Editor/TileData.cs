using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public JsonTileData data = new JsonTileData();

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Material material;

    public void SetTile(int indexMap, int indexTile,Material material)
    {
        this.data.indexMap = indexMap;
        this.data.tileLayer.layer1 = indexTile;

        this.meshFilter = this.gameObject.AddComponent<MeshFilter>();
        this.meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        this.material = new Material(material);
        this.meshRenderer.sharedMaterial = this.material;
        SetIndex(this.data.tileLayer.layer1);

        BoxCollider boxCollider = this.gameObject.AddComponent<BoxCollider>();
        boxCollider.center = new Vector3(0.5f, 0, 0.5f);
        boxCollider.size = new Vector3(1, 0.1f, 1);
    }

    public void SetIndex(int indexTile)
    {
        this.data.tileLayer.layer1 = indexTile;

        bool isEnabled = !(indexTile == -1 || indexTile == 255);
        meshRenderer.enabled = (isEnabled);
        if (isEnabled)
        {
            try
            {
                TilesetManager.Tileset tileset = TilesetManager.instance.LayerTileset(0);
                material.SetTexture("_MainTex", tileset.tiles[indexTile].texture);
            }
            catch (System.Exception)
            {

                throw;
            }   
        }
    }
}
