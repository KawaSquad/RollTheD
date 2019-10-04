using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureManager : MonoBehaviour
{
    public SObject_Player sessionData;
    public MapLoader mapLoader;
    public Camera mainCamera;

    PlayerController playerSelected = null;

    void Start()
    {
        mapLoader.LoadMap(sessionData.Save_Url, false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (playerSelected != null)
                playerSelected.UnselectCharacter();

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.tag == "Player")
                {
                    PlayerController player = hitInfo.collider.GetComponent<PlayerController>();
                    if (player != null)
                    {
                        player.SelectCharacter();
                        playerSelected = player;
                    }
                }
            }
        }
    }
}
