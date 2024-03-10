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

    [Header("Drop Settings")]
    [SerializeField] private bool markPlacedObjectForDestruction = false;
    [SerializeField] private float objectLifetime = 10f;

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
            // Drop horizontally centered on road
            InstantiateObject(new Vector2(road.transform.position.x, transform.position.y));
            // Add to exclusion list
            affectedRoads.Add(road);
        }
    }

    private void InstantiateObject(Vector2 dropPos)
    {
        GameObject newlyPlacedObject = Instantiate(
            objectToPlace,
            // dropPoint.position,
            dropPos,
            transform.rotation
        );
        if (markPlacedObjectForDestruction)
            Destroy(newlyPlacedObject, objectLifetime);
    }
}
