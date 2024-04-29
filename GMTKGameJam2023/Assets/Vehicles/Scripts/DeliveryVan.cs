using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryVan : Car
{

    [SerializeField] private GameObject[] obstaclePrefabs;



    [SerializeField] private float dropInterval;

    private float t = 0f;
    private Vector3 startPosition;
    private Vector3 target;

    [Header("Sway Values")]
    public GameObject closestRoad;
    [SerializeField] public float timeToReachTarget = 1f;

    public override void Start()
    {
        base.Start();
        SetCarSpeed(carSpeed);

        InvokeRepeating("DropObstacle", 0.25f, dropInterval);
    }

    private void DropObstacle()
    {
        int rand = Random.Range(0, 2);
        GameObject obstaclePrefab = obstaclePrefabs[rand];

        GameObject obstacle = Instantiate(obstaclePrefab, transform.position, Quaternion.identity);

    }
}