using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureManager : MonoBehaviour
{
    public SObject_Player sessionData;
    public MapLoader mapLoader;

    void Start()
    {
        mapLoader.LoadMap(sessionData.Save_Url, false);
    }

}
