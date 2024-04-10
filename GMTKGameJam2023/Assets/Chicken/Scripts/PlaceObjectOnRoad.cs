using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Places an object prefab on center of every road the chicken goes on once.
/// X-coordinate of placed item is center of road, y-coordinate is chicken's y coordinate
/// </summary>

public class PlaceObjectOnRoad : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject objectToPlace;
    [SerializeField] private GameObject objectToPlace2;

    [Header("Drop Settings")]
    [SerializeField] private bool markPlacedObjectForDestruction = false;
    [Tooltip("Probablity for drop 1/inputAmount (leave at 1 for constant)")]
    [SerializeField] private int probabilityOfSpawn = 1;
    [Tooltip("Probablity for drop 1/inputAmount (default 50% == 2)")]
    [SerializeField] private int probabilityOfSecondObject = 2;
    // [Header("Will only drop on lanes based on Equiped Vehicles")]
    // [SerializeField] private bool dropOnVehicleLanes = false;

    private List<RoadHighlight> affectedRoads;

    private void Start()
    {
        affectedRoads = new List<RoadHighlight>();
    }

    private void Update()
    {
        DropOnRoadCenter();
    }

    private void DropOnRoadCenter()
    {
        // Raycast down from drop point to find the road that the chicken is currently on
        Collider2D raycastHit = Physics2D.Raycast(transform.position, Vector2.zero).collider;
        if (!raycastHit) return;
        
        RoadHighlight road = raycastHit.GetComponent<RoadHighlight>();
        if (
            road != null // Is a road
            && !affectedRoads.Contains(road) // Not already placed upon
        )
        {
            GameObject gameObject;
            if(objectToPlace2 != null){
                int randomNumber = Random.Range(1, probabilityOfSecondObject);
                if(randomNumber == 1){
                    gameObject = objectToPlace2;
                }
                else{
                    gameObject = objectToPlace;
                }
            }
            else{
                gameObject = objectToPlace;
            }
            // Drop horizontally centered on road
            InstantiateObject(new Vector2(road.transform.position.x, transform.position.y), gameObject);
            // Add to exclusion list
            affectedRoads.Add(road);
        }
    }

    private void InstantiateObject(Vector2 dropPos, GameObject gameObject)
    {
        int randomNumber = Random.Range(1, probabilityOfSpawn);
        if(randomNumber == 1){
            GameObject newlyPlacedObject = Instantiate(
                gameObject,
                dropPos,
                Quaternion.identity
            );
        }
    }
}
