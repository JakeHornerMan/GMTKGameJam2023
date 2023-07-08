using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SoundManager soundManager;

    float moveTime;

    public float minMoveTime = 0.5f;
    public float maxMoveTime = 3f;

    public float chickenSpeed = 1f;
    public float laneDistance = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Start()
    {
        moveTime = Random.Range(minMoveTime, maxMoveTime);
        StartMovement();
    }

    void StartMovement()
    {
        IEnumerator coroutine = WaitAndMove(moveTime);
        StartCoroutine(coroutine);
    }

    IEnumerator WaitAndMove(float moveTime)
    {
        yield return new WaitForSeconds(moveTime);
        MoveChicken();

        // Restart Timer
        StartMovement();
    }

    void MoveChicken()
    {
        Vector2 targetPoint = rb.position + new Vector2(laneDistance, 0f);
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, chickenSpeed);
    }

    public void KillChicken()
    {
        soundManager.PlaySound(SoundManager.SoundType.Death);
        Destroy(gameObject);
    }
}
