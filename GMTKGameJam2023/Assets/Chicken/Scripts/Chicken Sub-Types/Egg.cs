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

    // Run by animation event
    public void Hatch()
    {
        GameObject newFeather = Instantiate(featherParticles, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);
        Destroy(newFeather, featherParticleLifetime);
        Instantiate(chickenPrefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Car>())
            Destroy(gameObject);
    }
}
