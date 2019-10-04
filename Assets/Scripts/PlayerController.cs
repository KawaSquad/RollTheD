using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;

    private Material material;
    private bool isSelected = false;
    private void Start()
    {
        material = new Material(this.meshRenderer.sharedMaterial);
    }

    public void UnselectCharacter()
    {
        isSelected = false;
        Debug.Log("Unselect : " + this.name);
        material.SetFloat("_HighlightColor", 0);
    }
    public void SelectCharacter()
    {
        isSelected = true;
        Debug.Log("Select : " + this.name);
        material.SetFloat("_HighlightColor", 0.5f);
    }
}
