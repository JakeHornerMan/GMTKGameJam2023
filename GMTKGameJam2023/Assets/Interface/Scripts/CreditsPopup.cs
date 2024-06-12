using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPopup : MonoBehaviour
{
    [SerializeField] private GameObject creditsPopupUI, creditsPopupBorder;

    private void Start()
    {
        HideCreditsUI();
    }

    public void OnButtonClick()
    {
        creditsPopupUI.SetActive(true);
        creditsPopupBorder.SetActive(true);
        GameObject.Find("Pointer").SetActive(false);
    }

    public void HideCreditsUI()
    {
        creditsPopupBorder.SetActive(false);
        creditsPopupUI.SetActive(false);
    }
}
