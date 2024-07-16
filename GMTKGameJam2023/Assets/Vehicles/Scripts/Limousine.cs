using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Limousine : Car, ISpecialLaunch
{

    [SerializeField] private GameObject cameraObject;

    [SerializeField] public float flashDuration;

    private Collider2D mainTrigger;
    private Collider2D childTrigger;

    public override void Start()
    {
        base.Start();
        SetCarSpeed(carSpeed);

        // Assign the colliders in Awake
        mainTrigger = GetComponent<Collider2D>();
        childTrigger = GetComponentInChildren<Collider2D>();

        // Ensure both are set to be triggers
        if (mainTrigger != null) mainTrigger.isTrigger = true;
        if (childTrigger != null) childTrigger.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Determine which of our triggers was activated
        bool isMainTrigger = mainTrigger != null && mainTrigger.bounds.Intersects(collision.bounds);
        bool isChildTrigger = childTrigger != null && childTrigger.bounds.Intersects(collision.bounds);

        // Check if Hit Chicken
        ChickenHealth chickenHealth = collision.gameObject.GetComponent<ChickenHealth>();
        if (chickenHealth == null && collision.transform.parent != null)
            chickenHealth = collision.transform.parent.GetComponent<ChickenHealth>();


        // Check if Hit Token
        TokenController token = collision.gameObject.GetComponent<TokenController>();

        // Check if Hit Wall
        WallController wall = collision.gameObject.GetComponent<WallController>();

        // Handle collisions based on which trigger was activated and what was hit
        if (isMainTrigger)
        {
            Debug.Log("Main trigger activated by: " + collision.gameObject.name);

            if (token != null && !ignoreTokens)
            {
                HandleTokenCollision(token);
            }
            else if (wall != null)
            {
                HandleWallCollision(wall);
            }
            else if (chickenHealth != null)
            {
                HandleChickenCollision(chickenHealth);
                // Add any specific logic for main trigger hitting chicken
            }
        }
        else if (isChildTrigger)
        {
            Debug.Log("Child trigger activated by: " + collision.gameObject.name);

            if (chickenHealth != null)
            {
                chickenHealth.FlashChicken(flashDuration);
            }
            // Add any other specific logic for child trigger collisions
        }
        else
        {
            Debug.Log("Unknown trigger activated by: " + collision.gameObject.name);
        }

        //if (gameObject.transform.position.x - collision.transform.position.x > 1)
        //{
        //    chickenHealth.FlashChicken(flashDuration);
        //}
        //else
        //{
        //    HandleChickenCollision(chickenHealth);
        //}



    }

    public void PerformSpecialLaunch()
    {
        cameraObject.SetActive(false);
    }
}
