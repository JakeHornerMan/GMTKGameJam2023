using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmouredChicken : ChickenHealth
{
    private void Start()
    {
        SetAnimation();
    }

    protected override void HandleHit()
    {
        SetAnimation();

        if(!isInvinsible){
            isInvinsible = true;
            StartCoroutine(StartInvinsibleTime());
        }
    }

    private void SetAnimation(){
        if(health <= 200 && health > 100){
            anim.Play("Armour1");
        }
        else if(health <= 100){
            anim.Play("NoArmour");
        }
        else{
            anim.Play("Armour2");
        }
    }
}
