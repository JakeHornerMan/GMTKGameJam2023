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
    [SerializeField] private float dropInterval = 1f;
    [SerializeField] private float substanceDurationSeconds = 20f;

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void Start()
    {
        // TODO Drop at Road Position (check remainder)
        InvokeRepeating(nameof(DropSubstance), dropInterval, dropInterval);
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
        isStuck = chickenCollider.IsTouchingLayers(cementLayer);

        if (!isStuck || ignoreCement)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    private void DropSubstance()
    {
        GameObject substance = Instantiate(
            slowSubstancePrefab,
            dropPoint.position,
            transform.rotation
        );
        Destroy(substance, substanceDurationSeconds);
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(DropSubstance));
        soundManager.PlayWagonChickenDeath();
    }
}
