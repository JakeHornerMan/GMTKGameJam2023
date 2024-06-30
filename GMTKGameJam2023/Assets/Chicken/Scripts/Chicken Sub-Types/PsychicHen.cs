using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicHen : ChickenHealth
{
    [SerializeField] private GameObject spawnPortal;
    [HideInInspector] private GameObject capturedVehicle;
    private SoundManager soundManager;
    private GameObject lanes;
    private GameObject[] roadLanes;
    private GameObject[] grassLanes;
    private GameObject[] busLanes;
    private GameObject[] waterLanes;
    private GameObject[] pavementLanes;
    public List<GameObject> allLanes;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void SpawnPortal(GameObject hitcar)
    {
        Vector3 portalPos = new(GetRandomRoad(hitcar.GetComponent<Car>().placeableLaneTags, hitcar).transform.position.x, transform.position.y -2, 0);
        GameObject spawnedPortal = Instantiate(spawnPortal, portalPos, Quaternion.identity);
        soundManager.PlayEnterPortal();
        spawnedPortal.GetComponent<PortalController>().capturedVehicle = hitcar;
        hitcar.SetActive(false);
    }

    private GameObject GetRandomRoad(List<string> placeableLaneTags)
    {
        if(placeableLaneTags.Contains("Road")){
            roadLanes = GameObject.FindGameObjectsWithTag("Road");
            AddToAllLanes(roadLanes);
        }
            
        if(placeableLaneTags.Contains("Grass")){
            grassLanes = GameObject.FindGameObjectsWithTag("Grass");
            AddToAllLanes(grassLanes);
        }
            
        if(placeableLaneTags.Contains("Bus Lane")){
            busLanes = GameObject.FindGameObjectsWithTag("Bus Lane");
            AddToAllLanes(busLanes);
        }
            
        if(placeableLaneTags.Contains("Water")){
            waterLanes = GameObject.FindGameObjectsWithTag("Water");
            AddToAllLanes(waterLanes);
        }
            
        if(placeableLaneTags.Contains("Pavement")){
            pavementLanes = GameObject.FindGameObjectsWithTag("Pavement");
            AddToAllLanes(pavementLanes);
        }

        int randomRoad = Random.Range(0, allLanes.Count-1);
        return allLanes[randomRoad];
    }

    private void AddToAllLanes(GameObject[] laneArray)
    {
        if(laneArray.Length > 0){
            foreach (GameObject value in laneArray)
            {
                allLanes.Add(value);
            }
        }
    }

    private void RemoveClosestLane()
    {
        
    }
}
