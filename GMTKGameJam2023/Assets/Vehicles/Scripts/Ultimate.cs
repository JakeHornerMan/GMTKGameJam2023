using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Ultimate : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] public Car correspondingUltimate;
    [SerializeField] public Image correspUltimateIcon;

    [Header("Ultimate Info")]
    [SerializeField] public float resetTime;
    [HideInInspector] public bool isReady = true;

    private VehicleSpawner vehicleSpawner;
    private GameManager gameManager;

    private void Awake()
    {
        vehicleSpawner = FindObjectOfType<VehicleSpawner>();
        gameManager = FindObjectOfType<GameManager>(); 
        if(gameManager.ultimateInLevel.isUltimate){
            correspondingUltimate = gameManager.ultimateInLevel;
            correspUltimateIcon.sprite = correspondingUltimate.GetComponent<ObjectInfo>().objectIcon;
            resetTime = correspondingUltimate.ultimateResetTime;
        }
    }

    private void Start()
    {
        
    }
}
