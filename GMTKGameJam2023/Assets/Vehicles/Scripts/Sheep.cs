using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sheep : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager.PlaySheepNoise();
    }

    protected override void HandleChickenCollision(ChickenHealth chickenHealth)
    {
        base.HandleChickenCollision(chickenHealth);

        // Destroy after hitting chicken
        Destroy(gameObject);
    }
}
