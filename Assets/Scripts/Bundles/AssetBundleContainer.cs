using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleContainer : MonoBehaviour
{
    public enum ContainerType
    {
        Textures,
    }
    public ContainerType containerType;

    public List<Texture2D> textures;
}
