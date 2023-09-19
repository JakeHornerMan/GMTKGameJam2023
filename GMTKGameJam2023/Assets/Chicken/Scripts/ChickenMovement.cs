using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject hopController;
    [SerializeField] private GameObject chickenSprite;
    [SerializeField] public Collider2D chickenCollider;

    [Header("Effects")]
    [SerializeField] private GameObject splashParticles;

    [Header("Movement Values")]
    [SerializeField] public int chickenIntesity = 0;
    [SerializeField] private float minMoveTime = 0.5f;
    [SerializeField] private float maxMoveTime = 3.5f;
    [SerializeField] private float laneDistance = 2f;

    [Header("Detection Info")]
    [SerializeField] public bool ignoreCement = false;
    [HideInInspector] public bool isStuck = false;
    [HideInInspector] public LayerMask cementLayer;
    [HideInInspector] public LayerMask waterLayer;
    [HideInInspector] private string cementLayerName = "Cement";
    [HideInInspector] private string waterLayerName = "Water";

    [Header("Animation")]
    [SerializeField] private string animatorHopBool = "Hop";
    [SerializeField] private float animationFinishTime = 0.5f;

    private float moveTime;

    [SerializeField] private float hitStopLength = 0.0f;

    private Vector2 targetPoint;
    private Vector2 directionVector;
    private Vector2 desiredDirection;

    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = hopController.GetComponent<Animator>();

        if (hopController != null)
        {
            anim = hopController.GetComponent<Animator>();
        }

        int cementLayerIndex = LayerMask.NameToLayer(cementLayerName);
        int waterLayerIndex = LayerMask.NameToLayer(waterLayerName);
        cementLayer = 1 << cementLayerIndex; //Convert layer index to layer mask, to avoid setting in inspector
        waterLayer = 1 << waterLayerIndex;
    }

    private void Start()
    {
        moveTime = Random.Range(minMoveTime, maxMoveTime);
        StartMovement();
    }

    protected virtual void StartMovement()
    {
        switch (chickenIntesity)
        {
            case 0:
                maxMoveTime = 3.5f;
                break;
            case 1:
                maxMoveTime = 3f;
                break;
            case 2:
                maxMoveTime = 2.5f;
                break;
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

        // Rewrite code to use a general "stuck" check, instead of checking for cement specifically
        if (!chickenCollider.IsTouchingLayers(cementLayer) || ignoreCement)
        {
            IEnumerator coroutine = WaitAndMove(moveTime);
            StartCoroutine(coroutine);
        }
        // The chicken is on cement, so wait for the cement to disappear and then restart movement
        else
            StartCoroutine(WaitForDryCement());
    }

    private IEnumerator WaitForDryCement()
    {
        while (chickenCollider.IsTouchingLayers(cementLayer))
        {
            yield return null; // Wait until the chicken is no longer on the cement
        }

        IEnumerator coroutine = WaitAndMove(moveTime);
        StartCoroutine(coroutine);
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

        if (chickenCollider.IsTouchingLayers(waterLayer))
        {
            Instantiate(
                splashParticles,
                transform.position + splashParticles.transform.localPosition,
                splashParticles.transform.rotation
            );
        }
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

    protected virtual Vector2 ChooseNextDirection()
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


    public void PlayChickenHitstop()
    {
        HitStop.instance.StartHitStop(hitStopLength);
    }

    public float GetChickenHitstop()
    {
        return hitStopLength;
    }
}
