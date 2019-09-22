using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public int indexMap = 0;
    public int indexTile = 0;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Material material;

    public void SetTile(int indexMap, int indexTile,Material material)
    {
        this.indexMap = indexMap;
        this.indexTile = indexTile;

        this.meshFilter = this.gameObject.AddComponent<MeshFilter>();
        this.meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        this.material = new Material(material);
        this.meshRenderer.sharedMaterial = this.material;
        SetIndex(this.indexTile);

        BoxCollider boxCollider = this.gameObject.AddComponent<BoxCollider>();
        boxCollider.center = new Vector3(0.5f, 0, 0.5f);
        boxCollider.size = new Vector3(1, 0.1f, 1);
    }

    public void SetIndex(int indexTile)
    {
        this.indexTile = indexTile;
        material.SetTexture("_MainTex", TilesetManager.instance.tiles[indexTile].texture);
    }
}
