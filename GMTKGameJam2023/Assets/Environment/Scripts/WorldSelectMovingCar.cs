using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WorldSelectMovingCar : MonoBehaviour
{
    [Header("Upwards Movement Values")]
    [SerializeField] float bottomY = -10f;
    [SerializeField] float topY = 9.8f;
    [SerializeField] float moveSpeed = 2f;

    [Header("Looping Delay")]
    [SerializeField] float loopDelaySeconds = 2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = transform.up * moveSpeed;

        StartCoroutine(LoopMovement());
    }

    private IEnumerator LoopMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(loopDelaySeconds);

            // Reset the car's position when it reaches the top
            Vector3 pos = transform.position;
            if (pos.y > topY)
                pos.y = bottomY;
            transform.position = pos;
        }
    }
}
