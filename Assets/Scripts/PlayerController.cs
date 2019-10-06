using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer = null;

    private Material material;
    private bool isSelected = false;

    private void Start()
    {
        material = new Material(this.meshRenderer.sharedMaterial);
        this.meshRenderer.sharedMaterial = material;

        isSelected = true;
        UnselectCharacter();
    }

    public void UnselectCharacter()
    {
        if (!isSelected)
        {
            return;
        }
        isSelected = false;

        //Debug.Log("Unselect : " + this.name);
        material.SetInt("_IsHighlighted", 0);
    }
    public void SelectCharacter()
    {
        if (isSelected)
        {
            return;
        }
        isSelected = true;

        //Debug.Log("Select : " + this.name);
        material.SetInt("_IsHighlighted", 1);
    }
}
