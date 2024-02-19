using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceChicken : ChickenHealth
{
    protected override void HandleDeath()
    {
        Vector3 particlePos = new(transform.position.x, transform.position.y, featherParticlesZPos);
        Instantiate(featherParticles, particlePos, Quaternion.identity);


        //FreezeCar(0.5f);


        Destroy(gameObject);
    }
}
