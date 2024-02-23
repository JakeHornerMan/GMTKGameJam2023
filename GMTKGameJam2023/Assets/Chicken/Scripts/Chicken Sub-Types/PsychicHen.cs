using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicHen : ChickenHealth
{
    [SerializeField] private GameObject spawnPortal;
    [HideInInspector] private GameObject capturedVehicle;
    private SoundManager soundManager;
    private GameObject lanes;
    public List<GameObject> alllanes;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        lanes = GameObject.Find("Lanes");
        List<GameObject> alllanes = new List<GameObject>();
        foreach (Transform t in lanes.transform.GetComponentsInChildren(typeof(GameObject), true))
        {
            if (t.gameObject.layer == 9)
            {
                alllanes.Add(t.gameObject);
            }
        }
    }

    public void SpawnPortal(GameObject hitcar)
    {
        Vector3 portalPos = new(GetRandomRoad(hitcar.GetComponent<Car>().placeableLaneTags).transform.position.x, transform.position.y -2, 0);
        GameObject spawnedPortal = Instantiate(spawnPortal, portalPos, Quaternion.identity);
        soundManager.PlayEnterPortal();
        spawnedPortal.GetComponent<PortalController>().capturedVehicle = hitcar;
        hitcar.SetActive(false);
    }

    private GameObject GetRandomRoad(List<string> placeableLaneTags)
    {
        // IMPLEMENTATION
        // List<GameObject> lanes = new List<GameObject>();
        // do{
        //     int randomLaneType = Random.Range(0, placeableLaneTags.Count -1);
        //     GameObject[] placableRoads = GameObject.FindGameObjectsWithTag(placeableLaneTags[randomLaneType]);
        //     lanes.AddRange(placableRoads);
        //     if(lanes.Count == 0){
        //         placeableLaneTags.RemoveAt(randomLaneType);
        //     }
        // }while(lanes.Count != 0);
        
        // int randomRoad = Random.Range(0, lanes.Count -1);
        // return lanes[randomRoad];

        // IMPLEMENTATION
        // List<GameObject> lanes = new List<GameObject>();
        // foreach(string lane in placeableLaneTags){
        //     if(GameObject.FindGameObjectsWithTag(lane).Length != 0){
        //         GameObject[] placableRoads = GameObject.FindGameObjectsWithTag(lane);
        //         lanes.AddRange(placableRoads);
        //     }
        // }
        // int randomRoad = Random.Range(0, lanes.Count -1);
        // return lanes[randomRoad];

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
