using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonChickenMovement : ChickenMovement
{
    [Header("Wheelbarrow Chicken References")]
    [SerializeField] private GameObject slowSubstancePrefab;
    [SerializeField] private Transform dropPoint;

    [Header("Wheelbarrow Chicken Values")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float substanceDurationSeconds = 20f;
    private List<RoadHighlight> affectedRoads;

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void Start()
    {
        affectedRoads = new List<RoadHighlight>();
    }

    protected override void StartMovement()
    {
        soundManager.PlayWagonChicken();
    }

    protected override Vector2 ChooseNextDirection()
    {
        return new Vector2(1, 0);
    }

    private void Update()
    {
        // Move horizontally unless Stuck
        isStuck = chickenCollider.IsTouchingLayers(cementLayer);
        if (!isStuck || ignoreCement)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }

        DropOnRoadCenter();
    }

    private void DropOnRoadCenter()
    {
        // Raycast down from drop point to find the road that the chicken is currently on
        Collider2D raycastHit = Physics2D.Raycast(dropPoint.position, Vector2.zero).collider;
        RoadHighlight road = raycastHit?.GetComponent<RoadHighlight>();
        if (
            raycastHit != null // Hit something
            && road != null // Is a road
            && !affectedRoads.Contains(road) // Not already placed upon
        )
        {
            // Drop horizontally centered on road
            DropSubstance(new Vector2(road.transform.position.x, transform.position.y));
            // Add to exclusion list
            affectedRoads.Add(road);
        }
    }

    private void DropSubstance(Vector2 dropPos)
    {
        GameObject substance = Instantiate(
            slowSubstancePrefab,
            // dropPoint.position,
            dropPos,
            transform.rotation
        );
        Destroy(substance, substanceDurationSeconds);
    }

    private void OnDestroy()
    {
        soundManager.PlayWagonChickenDeath();
    }
}
