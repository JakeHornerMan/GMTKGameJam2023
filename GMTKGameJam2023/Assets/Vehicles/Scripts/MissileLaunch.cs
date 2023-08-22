using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLaunch : Car
{
    [Header("Animation")]
    [SerializeField] private GameObject missileSprtie;

    public void Explode()
    {
        Destroy(missileSprtie.gameObject);
        GetComponent<Animator>().SetTrigger("Explode");
    }

    public new void DestroySelf()
    {
        if (totalPoints > 0)
            gameManager.AddPlayerScore(totalPoints);
        Destroy(gameObject);
    }
}
