using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimoFlash : MonoBehaviour
{
    private Limousine limo;

    private void Start()
    {
        limo = transform.parent.GetComponent<Limousine>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Check if Hit Chicken
        ChickenHealth chickenHealth = collision.gameObject.GetComponent<ChickenHealth>();
        if (chickenHealth == null && collision.transform.parent != null)
            chickenHealth = collision.transform.parent.GetComponent<ChickenHealth>();

        if (chickenHealth != null)
        {
            chickenHealth.FlashChicken(limo.flashDuration);
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
}
