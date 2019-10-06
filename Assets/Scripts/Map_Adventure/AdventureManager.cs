using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureManager : MonoBehaviour
{
    public SObject_Player sessionData;
    public MapLoader mapLoader;
    public Camera mainCamera;

    PlayerController playerSelected = null;

    /*
    void Start()
    {
        mapLoader.LoadMap(sessionData.GM_Url, false);
    }
     */

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.tag == "Player")
            {
                PlayerController player = hitInfo.collider.GetComponent<PlayerController>();
                if (player != null)
                {
                    if (playerSelected != null && playerSelected != player)
                    {
                        playerSelected.UnselectCharacter();
                        playerSelected = null;
                    }

                    player.SelectCharacter();
                    playerSelected = player;
                }
            }
        }
        else
        {
            if (playerSelected != null)
            {
                playerSelected.UnselectCharacter();
                playerSelected = null;
            }
        }
    }
}
