using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin_Display_2 : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad2))
        {
            ActiveTwoDisplay();
        }
    }

    void ActiveTwoDisplay()
    {
        Display mainDisplay = Display.main;
        if (Display.displays.Length < 2)
            return;

        Display.displays[0].Activate();
        Display.displays[1].Activate();

        Display.displays[0].SetParams(512, 512, 10, 10);
        Display.displays[1].SetParams(512, 512, -600, 10);
    }
}
