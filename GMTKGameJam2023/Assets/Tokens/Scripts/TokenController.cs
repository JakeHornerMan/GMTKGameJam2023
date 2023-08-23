using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenController : MonoBehaviour
{
    [Header("Token Type Values")]
    [SerializeField] private bool tokenBag = false;
    [SerializeField] public int tokenValue = 1;
    
    [Header("Sway Values")]
    [SerializeField] private bool isSwaying = true;

    [SerializeField] private float removeTime = 3f;
    [Tooltip("How long token lasts")]

    [SerializeField] private float amplitude = 0.2f;
    [Tooltip("The maximum distance of sway")]

    [SerializeField] private float frequency = 20f;
    [Tooltip("The frequency of the sway motion")]

    [SerializeField] private float speed = 2f;
    [Tooltip("The speed at which the object moves horizontally")]

    private SoundManager soundManager;
    private Animator anim;

    private Vector3 initialPosition;

    private float tokenShrinkAnimLength = 1.5f;

    private float t = 0f;
    private Vector3 startPosition;
    private Vector3 target;

    [Header("Sway Values")]
    public GameObject closestRoad;
    [SerializeField] public float timeToReachTarget = 1f;

    void Awake()
    {
        initialPosition = transform.position;
        soundManager = FindObjectOfType<SoundManager>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(WaitAndDie(removeTime));

        if(tokenBag){
            this.GetComponent<BoxCollider2D>().enabled = false;
            FindClosestRoad();
            SetDestination();
            // anim.Play("Token Shrink");
        }
        else {
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void Update()
    {
        if(isSwaying){
            Swaying();
        }
        
        if(closestRoad != null && tokenBag){
            if(this.transform.position != closestRoad.transform.position){
            TravelToRoad();
            }
            else{
                this.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    private void Swaying(){
        float verticalDisplacement = amplitude * Mathf.Sin(Time.time * frequency);
        float horizontalDisplacement = speed * Time.deltaTime;
        Vector3 newPosition = initialPosition + new Vector3(horizontalDisplacement, verticalDisplacement, 0f);
        transform.Translate(newPosition - transform.position);
    }

    private void SetDestination()
    {
        startPosition = this.transform.position;
        target = closestRoad.transform.position; 
    }

    private void TravelToRoad(){
        t += Time.deltaTime/timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);
    }

    private void FindClosestRoad()
    {
        float distanceToClosestRoad = Mathf.Infinity;
        GameObject[] allRoads = GameObject.FindGameObjectsWithTag("Road");

        foreach (GameObject road in allRoads){
            float distanceToRoad = (road.transform.position - this.transform.position).sqrMagnitude;
            if(distanceToRoad < distanceToClosestRoad){
                distanceToClosestRoad = distanceToRoad;
                closestRoad = road;
            }
        }
    }

    private IEnumerator WaitAndDie(float dieTime)
    {
        yield return new WaitForSeconds(dieTime - tokenShrinkAnimLength);

        anim.Play("Token Shrink");

        yield return new WaitForSeconds(tokenShrinkAnimLength);

        RemoveToken();
    }

    public void TokenCollected()
    {
        RemoveToken();
    }

    public void RemoveToken()
    {
        Destroy(gameObject);
    }
}
