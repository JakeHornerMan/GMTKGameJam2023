using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmouredChicken : ChickenHealth
{
    [Header("Health Criteria")]
    [SerializeField] private int noArmorHealth = 100;
    [SerializeField] private int stage1ArmorHealth = 200;

    [Header("Aniamtion Values")]
    [SerializeField] private string noArmorKey = "NoArmour";
    [SerializeField] private string armor1Key = "Armour1";
    [SerializeField] private string armor2Key = "Armour2";

    private void Start()
    {
        SetAnimation();
    }

    protected override void HandleHit()
    {
        SetAnimation();

        if (!isInvinsible)
        {
            isInvinsible = true;
            StartCoroutine(StartInvinsibleTime());
        }
    }

    private void SetAnimation()
    {
        if (health <= stage1ArmorHealth && health > noArmorHealth)
            anim.Play(armor1Key);
        else if (health <= noArmorHealth)
            anim.Play(noArmorKey);
        else
            anim.Play(armor2Key);
    }
}
