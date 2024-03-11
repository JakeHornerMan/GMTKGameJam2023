using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeMissile : Ultimate
{
    [Header("Animation")]
    [SerializeField] private GameObject missileSprtie;
    [SerializeField] private float freezeLength;

    public void Explode()
    {
        // Destroy(missileSprtie);
        GetComponent<Animator>().SetTrigger("Explode");
        soundManager.PlayFreeze();
        soundManager.PlayUnfrost(freezeLength);
        Destroy(missileSprtie);
    }

    public override void HandleChickenCollision(ChickenHealth chickenHealth)
    {
        Debug.Log("Freezing this chciken: "+ chickenHealth.gameObject.name 
            +". For seconds: "+ freezeLength);
        chickenHealth.FreezeChicken(freezeLength);
    }
}
