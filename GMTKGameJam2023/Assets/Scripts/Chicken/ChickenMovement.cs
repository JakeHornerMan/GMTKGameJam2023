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

    private Vector2 targetPoint;
    [SerializeField] private Vector2 directionVector;
    private Vector2 desiredDirection;
    [SerializeField] private GameObject hopController;
    [SerializeField] private GameObject chickenSprite;

    [SerializeField] private ParticleSystem featherParticles;

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
        if(gameManager.intesitySetting == 1){
            maxMoveTime = 2.5f;
        }
        if(gameManager.intesitySetting == 2){
            maxMoveTime = 2f;
        }
        if(gameManager.intesitySetting == 3){
            maxMoveTime = 2f;
        }
        if(gameManager.intesitySetting == 4){
            maxMoveTime = 1.5f;
        }
        if(gameManager.intesitySetting == 5){
            maxMoveTime = 1f;
        }
        if (!gameManager.gameOver)
        {
            IEnumerator coroutine = WaitAndMove(moveTime);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator WaitAndMove(float moveTime)
    {
        BeginChickenMovement();

        yield return new WaitForSeconds(moveTime);

        // Restart Timer
        StartMovement();
    }

    private void BeginChickenMovement()
    {
        desiredDirection = ChooseNextDirection() * laneDistance;

        if (directionVector != new Vector2(0,0))
        {
            RotateChicken();
        }

        targetPoint = rb.position + desiredDirection;

        if (desiredDirection != new Vector2(0, 0))
        {
            StartCoroutine(AnimateChicken());
        }
        else
        {
            MoveChicken();
        }
    }

    private void MoveChicken()
    {
        ResetChickenRotation();

        transform.position = new Vector3(targetPoint.x, targetPoint.y, transform.position.z);

        hopController.transform.localPosition = Vector3.zero;
    }

    private IEnumerator AnimateChicken()
    {
        Animator anim = hopController.GetComponent<Animator>();

        anim.SetBool("Hop", true);

        //GetComponent<BoxCollider2D>().enabled = false;

        // Wait for the animation to finish
        yield return new WaitForSeconds(0.5f);

        anim.SetBool("Hop", false);

        //GetComponent<BoxCollider2D>().enabled = true;

        //anim.Play("Idle");

        MoveChicken();
    }

    private void RotateChicken()
    {
        ResetChickenRotation();

        if (directionVector == new Vector2(0, 1))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            hopController.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (directionVector == new Vector2(0, -1))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            hopController.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (directionVector == new Vector2(-1, 0))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            hopController.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void ResetChickenRotation()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        hopController.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private Vector2 ChooseNextDirection()
    {
        bool directionApproved = false;

        directionVector = new Vector2(0, 0);

        while (directionApproved == false)
        {
            int randomNumber = Random.Range(0, 50);

            if (randomNumber == 49)
            {
                directionVector = new Vector2(-1, 0);
                directionApproved = true;
            }
            else if (randomNumber > 45)
            {
                directionVector = new Vector2(0, 0);
                directionApproved = true;
            }
            else if (randomNumber > 40)
            {
                directionVector = new Vector2(0, 1);

                if (transform.position.y + (1 * laneDistance) < 5)
                {
                    directionApproved = true;
                }
            }
            else if (randomNumber > 35)
            {
                directionVector = new Vector2(0, -1);

                if (transform.position.y + (-1 * laneDistance) > -5)
                {
                    directionApproved = true;
                }
            }
            else
            {
                directionVector = new Vector2(1, 0);
                directionApproved = true;
            }
        }
        
        return directionVector;
    }

    public void KillChicken()
    {
        soundManager.PlaySound(SoundManager.SoundType.Death);

        Instantiate(featherParticles, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);

        Destroy(gameObject);
    }
}
