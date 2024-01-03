using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UltimateManager : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] public Car correspondingUltimate;
    [SerializeField] public Image correspUltimateIcon;

    [Header("Ultimate Info")]
    [SerializeField] public float refillDelaySeconds;
    [HideInInspector] public float timeUntilRefill = 0f;
    [HideInInspector] public bool isReady;

    private VehicleSpawner vehicleSpawner;
    private GameManager gameManager;
    private InterfaceManager interfaceManager;

    private void Awake()
    {
        vehicleSpawner = FindObjectOfType<VehicleSpawner>();
        gameManager = FindObjectOfType<GameManager>(); 
        interfaceManager = GetComponent<InterfaceManager>();
    }

    private void Start()
    {
        if(gameManager.ultimateInLevel.isUltimate){
            correspondingUltimate = gameManager.ultimateInLevel;
            correspUltimateIcon.sprite = correspondingUltimate.GetComponent<ObjectInfo>().objectIcon;
            refillDelaySeconds = correspondingUltimate.ultimateResetTime;
        }   
    }

    private void Update()
    {
        if (!isReady)
        {
            StartCoroutine(RefillUltimate());
        }
    }

    private IEnumerator RefillUltimate()
    {
        while (!isReady)
        {
            yield return new WaitForSeconds(refillDelaySeconds);
            timeUntilRefill = refillDelaySeconds;
            Debug.Log("Refillultimate: " + timeUntilRefill);
        }

        isReady = true;
    }
}
