using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for single meteor, destroys itself at the end of animation and spawns in small explosion.
/// </summary>
public class ExplodingMeteor : MonoBehaviour
{
    [SerializeField] private GameObject smallExplosionPrefab;

    public void Explode()
    {
        Instantiate(
            smallExplosionPrefab,
            transform.position,
            Quaternion.identity
        );
        Destroy(gameObject);
    }
}
