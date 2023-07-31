using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboChickenMovement : ChickenMovement
{
    [SerializeField] private float speed = 1f; // The speed at which the chicken moves

    // Override the StartMovement method
    protected override void StartMovement()
    {
        // We do nothing in this override since this chicken will continuously move
    }

    // Override the ChooseNextDirection method
    protected override Vector2 ChooseNextDirection()
    {
        // This chicken always moves right
        return new Vector2(1, 0);
    }

    // We add a new Update method for this chicken to continuously move
    private void Update()
    {
        // Move the chicken continuously to the right at the speed defined
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
