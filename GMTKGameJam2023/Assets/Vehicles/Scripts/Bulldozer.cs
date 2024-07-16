using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulldozer : Car
{
    public override void Start()
    {
        base.Start();
        SetCarSpeed(carSpeed);
    }

    protected override void HandleSlowSubstanceCollision(GameObject slowSubstance)
    {
        // Bulldozer ignores slow substance
        // You can add any special effects or logic here
        Debug.Log("Bulldozer plows through slow substance!");

        Destroy(slowSubstance);
    }

    protected override void HandleWallCollision(WallController wall)
    {
        // Implement special bulldozer behavior for wall collision
        // For example, destroy the wall or push it
        Debug.Log("Bulldozer smashes through wall!");

        wall.WallHit();
        carHealth -= 10;
    }


}
