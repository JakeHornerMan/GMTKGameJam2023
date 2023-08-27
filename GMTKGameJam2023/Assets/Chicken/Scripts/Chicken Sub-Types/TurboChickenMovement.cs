using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboChickenMovement : ChickenMovement
{
    [Header("Turbo Chicken Values")]
    [SerializeField] private float speed = 1f; // The speed at which the chicken moves

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Override the StartMovement method
    protected override void StartMovement()
    {
        // We do nothing in this override since this chicken will continuously move
        soundManager.PlayTurboChicken();
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
        if (chickenCollider.IsTouchingLayers(cementLayer))
        {
            isStuck = true;  // Chicken is stuck
        }
        else
        {
            isStuck = false; // Chicken is not stuck
        }

        if (!isStuck || ignoreCement)
        {
            // Move the chicken continuously to the right at the speed defined
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    private void OnDestroy()
    {
        soundManager.PlayTurboChickenDeath();
    }
}
