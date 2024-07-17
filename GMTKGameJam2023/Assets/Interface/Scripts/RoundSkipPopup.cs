using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundSkipPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject popupUI;
    [SerializeField] private TextMeshProUGUI headerText = null;
    [SerializeField] public Button skipRoundButton = null;
    [SerializeField] public Button roundOneButton = null;
    [SerializeField] private TextMeshProUGUI skipRoundButtonText = null;

    [Header("Text Settings")]
    [SerializeField] private string headerTextTemplate = "You have already made it to <color=#3DDD7D>round $</color>.";
    [SerializeField] private string skipButtonTextTemplate = "Skip to round $";
    [SerializeField] private char numberKey = '$';

    private void Start()
    {
        // Hide UI by default
        popupUI.SetActive(false);
    }

    public void OpenPopupUI(int roundToSkip)
    {
        // Update text with round number.
        headerText.text = headerTextTemplate.Replace(numberKey.ToString(), roundToSkip.ToString());
        skipRoundButtonText.text = skipButtonTextTemplate.Replace(numberKey.ToString(), roundToSkip.ToString());

        popupUI.SetActive(true);
    }

    public void HidePopupUI()
    {
        popupUI.SetActive(false);
    }
}
