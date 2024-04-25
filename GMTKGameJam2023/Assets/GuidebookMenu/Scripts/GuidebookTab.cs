using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidebookTab : MonoBehaviour
{
    [Header("Tab Settings")]
    [SerializeField] private GuidebookSelector.ViewType tabType;
    [SerializeField] private Sprite inactiveBtnImg;
    [SerializeField] private Sprite activeBtnImg;

    private UnityEngine.UI.Image btnSprite; 
    private GuidebookSelector guidebookSelector;

    private void Awake()
    {
        guidebookSelector = FindObjectOfType<GuidebookSelector>();
        btnSprite = GetComponent<UnityEngine.UI.Image>();

        // Cars tab is active by default
        if (tabType == GuidebookSelector.ViewType.Cars)
            btnSprite.sprite = activeBtnImg;
        else
            btnSprite.sprite = inactiveBtnImg;
    }

    private void Update()
    {
        // If button tab is currently active, set sprite accordingly
        if (guidebookSelector.currentViewType == tabType)
            btnSprite.sprite = activeBtnImg;
        else
            btnSprite.sprite = inactiveBtnImg;
    }

    public void OnClick()
    {
        // Run corresponding function based on Type of button
        switch (tabType)
        {
            case GuidebookSelector.ViewType.Cars:
                guidebookSelector.ShowCarButtons();
                break;
            case GuidebookSelector.ViewType.Chicken:
                guidebookSelector.ShowChickenButtons();
                break;
            case GuidebookSelector.ViewType.Ultimates:
                guidebookSelector.ShowUltimateButtons();
                break;
        }
    }
}
