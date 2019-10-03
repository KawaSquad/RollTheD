using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuPanel : MonoBehaviour
{
    public EMenuState menuState;

    [SerializeField]
    private CanvasGroup alphaGroup;
    [SerializeField]
    private RectTransform rectTransform;

    public bool mIsPopup = false;

    public UnityEvent onActivePanel;


    public void ResetPosition()
    {
        if (rectTransform == null)
            rectTransform = this.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.localPosition = Vector3.zero;
        }
    }
    public void ActiveAlpha(bool isActive)
    {
        if (alphaGroup == null)
            alphaGroup = this.GetComponent<CanvasGroup>();

        if (alphaGroup != null)
        {
            alphaGroup.alpha = (isActive) ? 1f : 0f;
            alphaGroup.blocksRaycasts = (isActive) ? true : false;
            alphaGroup.interactable = (isActive) ? true : false;

            if (onActivePanel != null && isActive)
                onActivePanel.Invoke();
        }
    }
}
