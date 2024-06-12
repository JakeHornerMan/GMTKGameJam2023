using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private GameObject confirmationDialog;

    private void Start()
    {
        confirmationDialog.SetActive(false);
    }

    public void ToggleConfirmation()
    {
        confirmationDialog.SetActive(!confirmationDialog.activeInHierarchy);
    }

    public void ConfirmExit()
    {
        Application.Quit();
        Debug.Log("GAME EXIT");
    }

    public void HideConfirmation()
    {
        confirmationDialog.SetActive(false);
    }
}
