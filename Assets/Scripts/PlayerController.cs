using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KawaSquad.Network;

public class PlayerController : MonoBehaviour
{
    public class Server_PawnData
    {
        public int ID_Character;
        public int ID_Handler;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }

    public static PlayerController activeController = null;

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    private Material material;
    private bool isSelected = false;
    private bool isActive = false;

    public int id_Character = -1;

    private void Start()
    {
        material = new Material(this.meshRenderer.sharedMaterial);
        this.meshRenderer.sharedMaterial = material;

        isSelected = true;
        isActive = false;
        UnselectCharacter();
    }
    
    #region Highlight
    public void UnselectCharacter()
    {
        if (isActive || !isSelected)
            return;
        isSelected = false;

        //Debug.Log("Unselect : " + this.name);
        material.SetInt("_IsHighlighted", 0);
    }
    public void SelectCharacter()
    {
        if (isActive || isSelected)
            return;
        isSelected = true;

        //Debug.Log("Select : " + this.name);
        material.SetInt("_IsHighlighted", 1);
        material.SetColor("_HighlightColor", AdventureManager.Instance.colorSelections.highlightedLocal);
    }
    #endregion

    #region ActiveController
    public void ActiveCharacter()
    {
        isActive = !isActive;

        if (isActive)
        {
            if (activeController != null)
                activeController.ActiveCharacter();

            activeController = this;

            material.SetInt("_IsHighlighted", 1);
            material.SetColor("_HighlightColor", AdventureManager.Instance.colorSelections.active);
        }
        else
        {
            activeController = null;

            material.SetInt("_IsHighlighted", 0);
            material.SetColor("_HighlightColor", AdventureManager.Instance.colorSelections.unselected);
        }
    }
    #endregion

    public void SetPosition(Vector3 destination, Vector3 orientation, Vector3 scale)
    {
        Transform controller = this.transform;
        destination.y = controller.position.y;
        controller.position = destination;
        controller.eulerAngles = orientation;
        controller.localScale = scale;
    }
}
