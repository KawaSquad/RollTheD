using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera cameraControl;
    Vector3 mousePos;

    private void Update()
    {
        if (MapLoader.CurrentMap != null) 
        {
            //left mouse click
            //if (Input.GetKey(KeyCode.Mouse0))
            //{
            //    Ray ray = cameraControl.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hitInfo;

            //    if (Physics.Raycast(ray, out hitInfo))
            //    {
            //        TileData mapData = hitInfo.collider.GetComponent<TileData>();
            //        if (mapData != null)
            //        {
            //            if (mapData.indexTile != currentIndexTexture)
            //                mapData.SetIndex(currentIndexTexture);
            //        }
            //    }
            //}

            //midle mouse click
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                mousePos = Input.mousePosition;
            }
            if (Input.GetKey(KeyCode.Mouse2))
            {
                Vector3 mouvement = mousePos - Input.mousePosition;
                cameraControl.transform.localPosition += new Vector3(mouvement.x, 0, mouvement.y) * Time.deltaTime;

                mousePos = Input.mousePosition;
            }


            //Scroll mouse
            Vector3 newHeight = cameraControl.transform.localPosition + new Vector3(0, -Input.mouseScrollDelta.y, 0);
            if (newHeight.y < 1)
                newHeight.y = 1;
            cameraControl.transform.localPosition = newHeight;
        }
    }

    void CenterMap()
    {
        if (MapLoader.CurrentMap == null)
            return;

        Vector3 localPos = cameraControl.transform.localPosition;
        localPos.x = MapLoader.CurrentMap.sizeMap.x / 2f;
        localPos.z = MapLoader.CurrentMap.sizeMap.y / 2f;
        cameraControl.transform.localPosition = localPos;
    }
}
