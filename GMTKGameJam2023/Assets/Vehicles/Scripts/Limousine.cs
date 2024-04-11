using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Limousine : Car
{

    [SerializeField] private GameObject cameraObject;

    [SerializeField] private float flashDuration;

    public override void Start()
    {
        base.Start();
        SetCarSpeed(carSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        // Check if Hit Chicken
        ChickenHealth chickenHealth = collision.gameObject.GetComponent<ChickenHealth>();
        if (chickenHealth == null && collision.transform.parent != null)
            chickenHealth = collision.transform.parent.GetComponent<ChickenHealth>();


        // Check if Hit Token
        TokenController token = collision.gameObject.GetComponent<TokenController>();

        if (token != null & !ignoreTokens)
            HandleTokenCollision(token);



        if (chickenHealth != null)
            if (gameObject.transform.position.x - collision.transform.position.x > 1)
            {
                chickenHealth.FlashChicken(flashDuration);
            }
            else
            {
                HandleChickenCollision(chickenHealth);
            }
            
    }
}
