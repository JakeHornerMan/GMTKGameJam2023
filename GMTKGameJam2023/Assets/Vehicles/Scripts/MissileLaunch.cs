using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLaunch : Car
{
    [Header("Animation")]
    [SerializeField] private GameObject missileSprtie;

    private void Start()
    {
        soundManager?.PlayMissileLaunch();
    }

    public void Explode()
    {
        Destroy(missileSprtie.gameObject);
        GetComponent<Animator>().SetTrigger("Explode");
    }
}
