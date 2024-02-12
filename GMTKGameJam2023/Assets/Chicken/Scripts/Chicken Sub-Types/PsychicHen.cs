using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicHen : ChickenHealth
{
    [SerializeField] private GameObject spawnPortal;
    [HideInInspector] private GameObject capturedVehicle;
// [HideInInspector] private GameObject spawnedPortal;
    public void SpawnPortal(GameObject hitcar)
    {
        Vector3 portalPos = new(GetRandomRoad().transform.position.x, transform.position.y -3, 0);
        GameObject spawnedPortal = Instantiate(spawnPortal, portalPos, Quaternion.identity);
        spawnedPortal.GetComponent<PortalController>().capturedVehicle = hitcar;
        hitcar.SetActive(false);
    }

    private GameObject GetRandomRoad()
    {
        float distanceToClosestRoad = Mathf.Infinity;
        GameObject[] allRoads = GameObject.FindGameObjectsWithTag("Road");

        int randomRoad = Random.Range(0, allRoads.Length-1);
        return allRoads[randomRoad];
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     // Check if Hit Car
    //     if (collision.gameObject.GetComponent<Car>()){
    //         capturedVehicle = collision.gameObject;
    //         Debug.Log("We hit a car");
    //         spawnPortal.GetComponent<PortalController>().capturedVehicle = capturedVehicle;
    //     }
    // }
}
