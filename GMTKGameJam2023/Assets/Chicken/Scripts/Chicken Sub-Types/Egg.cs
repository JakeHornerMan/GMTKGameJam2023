using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject chickenPrefab;
    [SerializeField] private GameObject featherParticles;
    // Destroy feather particle gameobject after delay for cleaner hierarchy
    [SerializeField] private float featherParticleLifetime = 2f;  
    [SerializeField] private float objectLifetime = 6f;

    private GameObject chickenContainer;

    private void Start()
    {
        chickenContainer = FindObjectOfType<ChickenManager>().gameObject;

        transform.parent = chickenContainer.transform;
    }

    // Run by animation event
    public void Hatch()
    {
        GameObject newFeather = Instantiate(featherParticles, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);
        Destroy(newFeather, featherParticleLifetime);

        Instantiate(chickenPrefab, transform.position, Quaternion.identity, chickenContainer.transform);
        Destroy(gameObject, objectLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Car>())
            Destroy(gameObject);
    }
}
