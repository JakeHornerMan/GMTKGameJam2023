using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Car : MonoBehaviour
{

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death Box")
        {
            Destroy(gameObject);
        }
    }
}
