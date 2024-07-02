using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Car;

public class DeliveryObstacle : EnvironmentalObstacle
{

    private float t = 0f;
    private Vector3 startPosition;
    private Vector3 target;

    [Header("Sway Values")]
    public GameObject closestRoad;
    [SerializeField] public float timeToReachTarget = 1f;

    private bool obstacleHasLanded = false;

    public override void Start()
    {
        base.Start();

        GetComponent<BoxCollider2D>().enabled = false;

        obstacleHasLanded = false;

        ChooseRandomRoad();
        SetDestination();
    }

    private void Update()
    {
        if (obstacleHasLanded == false)
        {
            if (transform.position != target)
            {
                TravelToRoad();
            }
            else
            {
                LandObstacle();
                
            }
        }
        
    }

    private void LandObstacle()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        obstacleHasLanded = true;
    }

    private void SetDestination()
    {
        startPosition = this.transform.position;

        float randomXPos = ChooseRandomRoad();

        float randomYPos = Random.Range(-3.00f, -0.5f);
        //target = closestRoad.transform.position;

        target = new Vector3(transform.position.x + randomXPos, transform.position.y + randomYPos, 1);
    }

    private void TravelToRoad()
    {
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);
    }

    private float ChooseRandomRoad()
    {
        // Generate a random integer between 0 and 2 (inclusive)
        int randomIndex = Random.Range(0, 3);

        // Define an array to hold the possible values
        float[] options = { -2.5f, 0f, 2.5f };

        // Access the randomly chosen value from the array based on the random index
        return options[randomIndex];
    }

    private void FindClosestRoad()
    {
        //float distanceToClosestRoad = Mathf.Infinity;
        //GameObject[] allRoads = GameObject.FindGameObjectsWithTag("Road");

        //foreach (GameObject road in allRoads)
        //{
        //    float distanceToRoad = (road.transform.position - this.transform.position).sqrMagnitude;
        //    if (distanceToRoad < distanceToClosestRoad)
        //    {
        //        distanceToClosestRoad = distanceToRoad;
        //        closestRoad = road;
        //    }
        //}
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent<Car>(out Car car) || collision.gameObject.TryGetComponent<Ultimate>(out Ultimate ultimate))
    //    {
    //        LaunchObject();
    //    }
    //}
}
