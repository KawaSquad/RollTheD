using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KawaSquad.Network;

public class PlayerController : Pawn
{
    public static PlayerController activeController = null;

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    private Material material;
    private bool isSelected = false;
    private bool isActive = false;

    [System.Serializable]
    public class PlayerController_Data
    {
        public int id_Character = 0;
        public int id_Token = 0;
        public string className = "novice";
    }
    public PlayerController_Data playerController_Data;

    private Vector2[] offsetsTexture = { new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 0f), new Vector2(0.5f, 0f) };

    public override void Initialize()
    {
        base.Initialize();

        if (serverData.pawnType != Server_PawnData.PawnPackets.P_Player)
        {
            Debug.LogError("try to instance pawn with wrong type");
            return;
        }

        playerController_Data = JsonUtility.FromJson<PlayerController_Data>(serverData.classParsed);

        material = new Material(this.meshRenderer.sharedMaterial);
        if (playerController_Data.id_Token < offsetsTexture.Length)
            material.SetTextureOffset("_MainTex", offsetsTexture[playerController_Data.id_Token]);
        this.meshRenderer.sharedMaterial = material;

        isSelected = true;
        isActive = false;
        UnselectCharacter();
    }
    public override string GetClassParsed()
    {
        return JsonUtility.ToJson(playerController_Data);
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

    public void SetActiveCharacter(bool isActive)
    {
        this.isActive = isActive;

        if (isActive)
        {
            //if (activeController != null && activeController != this)
            //    activeController.SetActiveCharacter(false);

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

}
