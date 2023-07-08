using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI tokenPriceText;

    [Header("Car Values")]
    [SerializeField] public Car correspondingCar;

    private VehicleSpawner vehicleSpawner;

    private void Awake()
    {
        vehicleSpawner = FindObjectOfType<VehicleSpawner>();
    }

    private void Start()
    {
        tokenPriceText.text = correspondingCar.carPrice.ToString("0");
    }

    public void SelectCorrespondingCar()
    {
        vehicleSpawner.SelectCar(this);
    }
}
