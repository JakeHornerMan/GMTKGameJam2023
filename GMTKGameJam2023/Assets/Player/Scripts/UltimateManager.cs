using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UltimateManager : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] public Ultimate correspondingUltimate;
    [SerializeField] public Image correspUltimateIcon;

    [Header("Ultimate Info")]
    [SerializeField] public float refillDelaySeconds;
    [HideInInspector] public float timeUntilRefill = 0f;
    [HideInInspector] public bool ultimateEnabled = false;

    private VehicleSpawner vehicleSpawner;
    private GameManager gameManager;
    private TutorialManager tutorialManager;
    private InterfaceManager interfaceManager;

    private void Awake()
    {
        vehicleSpawner = FindObjectOfType<VehicleSpawner>();
        gameManager = FindObjectOfType<GameManager>(); 
        tutorialManager = FindObjectOfType<TutorialManager>();
        interfaceManager = GetComponent<InterfaceManager>();
    }

    private void Start()
    {
        if(gameManager != null && gameManager.ultimateInLevel){
            SetUltimate(gameManager.ultimateInLevel);
        }
        if(tutorialManager != null && tutorialManager.ultimateInLevel){
            SetUltimate(tutorialManager.ultimateInLevel);
        }
    }
    
    public void SetUltimate(Ultimate ultimateInLevel){
        correspondingUltimate = ultimateInLevel;
        correspUltimateIcon.sprite = correspondingUltimate.GetComponent<ObjectInfo>().objectIcon;
        timeUntilRefill = correspondingUltimate.ultimateResetTime;
    }

    private void Update()
    {
        if (!ultimateEnabled)
        {
            StartCoroutine(RefillUltimate());
            timeUntilRefill -= Time.deltaTime;
            // Debug.LogWarning(timeUntilRefill);
        }

    }

    private IEnumerator RefillUltimate()
    {
        // infinite loop, be careful with these!
        while (ultimateEnabled == false)
        {
            // wait for the refill delay
            refillDelaySeconds = correspondingUltimate.ultimateResetTime;
            yield return new WaitForSeconds(refillDelaySeconds);
            // reset timeUntilRefill
            timeUntilRefill = correspondingUltimate.ultimateResetTime;
            ultimateEnabled = true;
        }
    }
}
