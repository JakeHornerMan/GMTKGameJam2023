using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Drops meteors at random locations on level, while moving horizontally.
/// </summary>
public class RoadRage : Ultimate
{

    [Header("Road Rage Settings")]
    public List<GameObject> laneTypes;

    private List<GameObject> lanes;

    [SerializeField] private List<Car> cars;

    private GameObject laneContainer;

    [SerializeField] private float carCount;

    [SerializeField] private float carSpawnDelay;

    private void Start()
    {
        laneContainer = GameObject.Find("Lanes");
        lanes = new List<GameObject>(); // Initialize the lanes list
        int childIndex = 0;
        foreach (Transform child in laneContainer.transform)
        {
            if (childIndex > 0)
            {
                lanes.Add(child.gameObject);
            }
            childIndex++;
        }

        soundManager?.PlaySound(0f ,spawnSound[0]);

        StartCoroutine(SpawnVehiclesOnRoad());
    }

    private IEnumerator SpawnVehiclesOnRoad()
    {
        for (int i = 0; i < carCount; i++)
        {
            GameObject randomLane = lanes[Random.Range(0, lanes.Count)];

            Vector3 spawnPos = randomLane.transform.position;
            spawnPos.y = spawnPos.y - 8f;

            Car car = DetermineVehicleType(randomLane);

            Instantiate(car, spawnPos, Quaternion.identity, gameObject.transform);

            yield return new WaitForSeconds(carSpawnDelay);
        }
    }

    private Car DetermineVehicleType(GameObject lane)
    {
        if (lane.tag == "Road" || lane.tag == "Bus Lane")
        {
            int randomNum = Random.Range(0, 100);

            if (randomNum < 75)
            {
                return cars[0];
            }
            else
            {
                return cars[4]; //Supercar
            }
        }
        else if (lane.tag == "Grass")
        {
            return cars[1];
        }
        else if (lane.tag == "Pavement")
        {
            return cars[2];
        }
        else if (lane.tag == "Water")
        {
            return cars[3];
        }

        return cars[0];
    }
}
