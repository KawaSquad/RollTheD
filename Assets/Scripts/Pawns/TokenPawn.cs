using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KawaSquad.Network;

public class TokenPawn : Pawn
{
    public static TokenPawn activeToken = null;

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    protected Material material;
    protected bool isSelected = false;
    protected bool isActive = false;

    protected Vector2[] offsetsTexture = { new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 0f), new Vector2(0.5f, 0f) };

    public override void Initialize()
    {
        base.Initialize();

        material = new Material(this.meshRenderer.sharedMaterial);
        this.meshRenderer.sharedMaterial = material;

        isSelected = true;
        isActive = false;
        UnselectToken();
    }

    #region Highlight
    public void UnselectToken()
    {
        if (isActive || !isSelected)
            return;
        isSelected = false;

        //Debug.Log("Unselect : " + this.name);
        material.SetInt("_IsHighlighted", 0);
    }
    public void SelectToken()
    {
        if (isActive || isSelected)
            return;
        isSelected = true;

        //Debug.Log("Select : " + this.name);
        material.SetInt("_IsHighlighted", 1);
        material.SetColor("_HighlightColor", AdventureManager.Instance.colorSelections.highlightedLocal);
    }
    #endregion

    #region activeToken
    public void ActiveToken()
    {
        isActive = !isActive;

        if (isActive)
        {
            if (activeToken != null)
                activeToken.ActiveToken();

            activeToken = this;

            material.SetInt("_IsHighlighted", 1);
            material.SetColor("_HighlightColor", AdventureManager.Instance.colorSelections.active);
        }
        else
        {
            activeToken = null;

            material.SetInt("_IsHighlighted", 0);
            material.SetColor("_HighlightColor", AdventureManager.Instance.colorSelections.unselected);
        }
    }

    public void SetActiveToken(bool isActive)
    {
        this.isActive = isActive;

        if (isActive)
        {
            activeToken = this;

            material.SetInt("_IsHighlighted", 1);
            material.SetColor("_HighlightColor", AdventureManager.Instance.colorSelections.active);
        }
        else
        {
            activeToken = null;

            material.SetInt("_IsHighlighted", 0);
            material.SetColor("_HighlightColor", AdventureManager.Instance.colorSelections.unselected);
        }
    }

    #endregion

}
