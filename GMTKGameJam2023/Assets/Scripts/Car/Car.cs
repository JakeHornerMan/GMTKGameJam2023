using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Car : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string objectBoundsTag = "Deathbox";

    [Header("Speed")]
    [SerializeField] protected float carSpeed = 5f;

    protected virtual void SetCarSpeed()
    {
        gameObject.TryGetComponent(out Rigidbody2D rb);

        if (rb != null)
        {
            rb.velocity = transform.up * carSpeed;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chicken"))
        {
            Destroy(collision.gameObject);
            //collision.gameObject.GetComponent<ChickenMovement>().KillChicken();
        }

        if (collision.gameObject.CompareTag(objectBoundsTag))
        {
            Destroy(gameObject);
            
        }
    }
}
