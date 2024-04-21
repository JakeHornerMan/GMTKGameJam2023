using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    private GameObject chickenContainer;
    public GameObject specialChicken;
    public GameObject chickenSprite;
    private LineRenderer lineRenderer;
    private List<Transform> points;
    [SerializeField] private int damage = 20;
    private bool chipDamageChicken = false;

    private bool isAttached = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        chickenContainer = GameObject.Find("SpecialChickenContainer");
        FindTarget();
    }

    public void FindTarget(){
        specialChicken = GetClosestSpecialChicken();
        chickenSprite = null;
        if(specialChicken.GetComponent<ChickenMovement>() != null){
            chickenSprite = specialChicken.GetComponent<ChickenMovement>().chickenSprite;
        }
        if(specialChicken.GetComponent<AlternativeChickenMovement>() != null){
            chickenSprite = specialChicken.GetComponent<AlternativeChickenMovement>().chickenSprite;
        }
    }

    private void FixedUpdate() {
        FishingForChickens();
    }

    private void FishingForChickens(){
        if(chickenSprite != null){
            CastLineToChicken();
        }
        else{
            Debug.Log("This is being called");
            lineRenderer.positionCount = 0;
            chipDamageChicken = false;
            isAttached = false;
            specialChicken = null;
            chickenSprite = null;
            FindTarget();
        }
    }

    private GameObject GetClosestSpecialChicken()
    {
        GameObject bestTarget = null;
        Vector3 currentPosition = transform.position;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (Transform child in chickenContainer.transform)
        {
            Vector3 directionToTarget = child.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = child.gameObject;
            }
        }


        return bestTarget;
    }

    private void CastLineToChicken(){

        if (isAttached){
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, chickenSprite.transform.position);
        }
        else{
            lineRenderer.positionCount = 2;
            StartCoroutine(AnimateLineOut());
        }
    }

    private void ChipDamageChicken(){
        if(!chipDamageChicken){
            StartCoroutine(specialChicken.GetComponent<ChickenHealth>().ChipDamage(damage));
            chipDamageChicken = true;
        }
    }

    private IEnumerator AnimateLineOut(){
        lineRenderer.SetPosition(0, gameObject.transform.position);
        float startTime = Time.time;
        float animationTime = 0.75f;

        Vector3 startPosition = gameObject.transform.position;

        Vector3 pos = startPosition;
        while(pos != chickenSprite.transform.position){
            float t = (Time.time - startTime) / animationTime;
            pos = Vector3.Lerp(gameObject.transform.position, chickenSprite.transform.position, t);
            lineRenderer.SetPosition(1, pos);
            if(pos == chickenSprite.transform.position){
                isAttached = true;
                ChipDamageChicken();
            }
            yield return null;
        }

    }

    private IEnumerator AnimateLineIn(){
        lineRenderer.SetPosition(0, gameObject.transform.position);
        float startTime = Time.time;
        float animationTime = 0.5f;

        Vector3 startPosition = gameObject.transform.position;

        Vector3 pos = startPosition;
        while(pos != chickenSprite.transform.position){
            float t = (Time.time - startTime) / animationTime;
            pos = Vector3.Lerp(gameObject.transform.position, chickenSprite.transform.position, t);
            lineRenderer.SetPosition(1, pos);
            if(pos == chickenSprite.transform.position){
                isAttached = true;
                ChipDamageChicken();
            }
            yield return null;
        }

    }

}
