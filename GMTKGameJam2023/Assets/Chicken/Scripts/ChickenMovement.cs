using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject hopController;
    [SerializeField] private GameObject chickenSprite;

    [Header("Movement Values")]
    [SerializeField] private float minMoveTime = 0.5f;
    [SerializeField] private float maxMoveTime = 3f;
    [SerializeField] private float laneDistance = 2f;

    [Header("Animation")]
    [SerializeField] private string animatorHopBool = "Hop";
    [SerializeField] private float animationFinishTime = 0.5f;

    private float moveTime;

    private Vector2 targetPoint;
    private Vector2 directionVector;
    private Vector2 desiredDirection;

    private Rigidbody2D rb;
    private GameManager gameManager;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        anim = hopController.GetComponent<Animator>();
    }

    private void Start()
    {
        moveTime = Random.Range(minMoveTime, maxMoveTime);
        StartMovement();
    }

    private void StartMovement()
    {
        switch (gameManager.intensitySetting)
        {
            case 1:
                maxMoveTime = 2.5f;
                break;
            case 2:
            case 3:
                maxMoveTime = 2f;
                break;
            case 4:
                maxMoveTime = 1.5f;
                break;
            case 5:
                maxMoveTime = 1f;
                break;
        }

        if (!gameManager.isGameOver || gameManager == null)
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

        if (directionVector != Vector2.zero)
            RotateChicken();

        targetPoint = rb.position + desiredDirection;

        if (desiredDirection != Vector2.zero)
            StartCoroutine(AnimateChicken());
        else
            MoveChicken();
    }

    private void MoveChicken()
    {
        ResetChickenRotation();

        transform.position = new Vector3(targetPoint.x, targetPoint.y, transform.position.z);

        hopController.transform.localPosition = Vector3.zero;
    }

    private IEnumerator AnimateChicken()
    {
        anim.SetBool(animatorHopBool, true);

        // Wait for the animation to finish
        yield return new WaitForSeconds(animationFinishTime);

        anim.SetBool(animatorHopBool, false);

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
        gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        hopController.transform.rotation = Quaternion.Euler(Vector3.zero);
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
}
