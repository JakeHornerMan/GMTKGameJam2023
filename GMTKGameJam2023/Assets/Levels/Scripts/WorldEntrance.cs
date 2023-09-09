using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEntrance : MonoBehaviour
{
    [Header("World Entering")]
    [SerializeField] private WorldConfigSO worldToEnter;

    private WorldSelect worldSelect;

    private void Awake()
    {
        worldSelect = FindObjectOfType<WorldSelect>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.GetComponent<Car>()) return;
        WorldSelect.selectedWorld = worldToEnter;
        worldSelect.LoadLevelSelect();
    }
}
