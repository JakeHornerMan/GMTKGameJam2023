using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string objectBoundsTag = "Death Box";

    [Header("Scoring")]
    [SerializeField] public int pointsReward = 100;

    private Rigidbody2D rb;
    private GameManager gameManager;
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
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        moveTime = Random.Range(minMoveTime, maxMoveTime);
        StartMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy Chickens When They Reach Off-Screen
        if (collision.gameObject.CompareTag(objectBoundsTag))
        {
            gameManager.safelyCrossedChickens++;
            Destroy(gameObject);
        }
    }

    private void StartMovement()
    {
        IEnumerator coroutine = WaitAndMove(moveTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndMove(float moveTime)
    {
        yield return new WaitForSeconds(moveTime);
        MoveChicken();

        // Restart Timer
        StartMovement();
    }

    private void MoveChicken()
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
