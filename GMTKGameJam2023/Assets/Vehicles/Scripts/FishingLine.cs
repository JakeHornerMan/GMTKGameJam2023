using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    private GameObject specialChickenContainer;
    private GameObject chickenContainer;
    public GameObject chicken;
    public GameObject chickenSprite;
    public GameObject hook;
    private LineRenderer lineRenderer;
    private List<Transform> points;
    [SerializeField] private int damage = 30;
    private bool chipDamageChicken = false;

    [SerializeField] private Car car;
    private ChickenHealth chickenHealth;

    private bool isAttached = false;
    private bool startDamage = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        specialChickenContainer = GameObject.Find("SpecialChickenContainer");
        chickenContainer = GameObject.Find("ChickenContainer");
        // hook.SetActive(false);
        FindTarget();
    }

    public void FindTarget()
    {
        // if(gameObject.transform.position.y > 6f){
        //     //car.DestroySelf();
        //     Destroy(gameObject);
        // }
        if(car.carInAction){
            StartCoroutine(WaitAndFindTargetChicken());
        }
        else{
            Destroy(gameObject);
            Destroy(hook);
        }
    }

    private IEnumerator WaitAndFindTargetChicken()
    {
        yield return new WaitForSeconds(0.6f);
        chicken = GetClosestSpecialChicken();
        if(chicken == null){
            chicken = GetClosestChicken();
        }
        chickenHealth = chicken.GetComponent<ChickenHealth>();
        chickenHealth.isAttached = true;
        chickenSprite = chicken.GetComponent<ChickenHealth>().chickenSprite;
        StartCoroutine(AnimateLineOut());
    }

    private void Update()
    {
        if(car.carInAction){
            FishingForChickens();
        }
        else{
            Destroy(gameObject);
            Destroy(hook);
            if(chickenHealth != null){
                chickenHealth.isAttached = false;
            }
        }
    }

    private void FishingForChickens()
    {
        //while hooks are attached
        if (isAttached && chickenSprite != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, chickenSprite.transform.position);
            hook.transform.position = chickenSprite.transform.position;
            if(startDamage){
                StartCoroutine(DamageAndKillChicken());
            }
        }

        //when Chicken has died
        if (chickenSprite == null && isAttached)
        {
            isAttached = false;
            StartCoroutine(AnimateLineIn());
        }
    }

    private IEnumerator DamageAndKillChicken()
    {
        startDamage = false;
        if (chickenHealth.health - damage <= 0)
        {
            car.KillChicken(chickenHealth);
        }
        chickenHealth.TakeDamage(damage);
        if (chickenHealth.health > 0)
        {
            yield return new WaitForSeconds(1f);
            RedoDamageAndKillChicken();
        }
        yield return new WaitForSeconds(0f);
    }

    public void RedoDamageAndKillChicken(){
        StartCoroutine(DamageAndKillChicken());
    }

    private GameObject GetClosestSpecialChicken()
    {
        GameObject bestTarget = null;
        Vector3 currentPosition = transform.position;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (Transform child in specialChickenContainer.transform)
        {
            Vector3 directionToTarget = child.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                if(!child.GetComponent<ChickenHealth>().isAttached){
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = child.gameObject;
                }
            }
        }
        return bestTarget;
    }

    private GameObject GetClosestChicken()
    {
        GameObject bestTarget = null;
        Vector3 currentPosition = transform.position;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (Transform child in chickenContainer.transform)
        {
            Vector3 directionToTarget = child.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                if(!child.GetComponent<ChickenHealth>().isAttached){
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = child.gameObject;
                }
            }
        }
        return bestTarget;
    }

    // private void ChipDamageChicken()
    // {
    //     if (!chipDamageChicken)
    //     {
    //         StartCoroutine(chicken.GetComponent<ChickenHealth>().ChipDamage(damage));
    //         chipDamageChicken = true;
    //     }
    // }

    private IEnumerator AnimateLineOut()
    {
        lineRenderer.positionCount = 2;
        // hook.SetActive(true);
        lineRenderer.SetPosition(0, gameObject.transform.position);
        float startTime = Time.time;
        float animationTime = 0.5f;

        Vector3 startPosition = gameObject.transform.position;

        Vector3 pos = startPosition;
        while (pos != chickenSprite.transform.position)
        {
            float t = (Time.time - startTime) / animationTime;
            pos = Vector3.Lerp(gameObject.transform.position, chickenSprite.transform.position, t);
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, pos);
            hook.transform.position = pos;
            if (pos == chickenSprite.transform.position)
            {
                isAttached = true;
                chickenHealth.isAttached = isAttached;
                // ChipDamageChicken();
                startDamage = true;
            }
            yield return null;
        }

    }

    private IEnumerator AnimateLineIn()
    {
        Debug.Log("FishingLine In");
        chipDamageChicken = false;
        isAttached = false;
        chicken = null;
        chickenSprite = null;

        lineRenderer.SetPosition(0, gameObject.transform.position);
        float startTime = Time.time;
        float animationTime = 0.5f;

        Vector3 startPosition = lineRenderer.GetPosition(1);

        Vector3 pos = startPosition;
        while (pos != gameObject.transform.position)
        {
            float t = (Time.time - startTime) / animationTime;
            pos = Vector3.Lerp(startPosition, gameObject.transform.position, t);
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, pos);
            hook.transform.position = pos;
            if (pos == gameObject.transform.position)
            {
                Debug.Log("FindingTarget");
                FindTarget();
                break;
            }
            yield return null;
        }
    }

}
