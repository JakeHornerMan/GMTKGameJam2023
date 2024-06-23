using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLaunch : Ultimate
{
    [Header("Animation")]
    [SerializeField] private GameObject missileSprtie;

    public void Explode()
    {
        Destroy(missileSprtie);
        GetComponent<Animator>().SetTrigger("Explode");
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
