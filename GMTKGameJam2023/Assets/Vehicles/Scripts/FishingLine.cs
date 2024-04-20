using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    private GameObject chickenContainer;
    public GameObject specialChicken;
    private LineRenderer lineRemderer;
    private List<Transform> points;

    private void Awake()
    {
        lineRemderer = GetComponent<LineRenderer>();
        chickenContainer = GameObject.Find("SpecialChickenContainer");
        FindTarget();
    }

    public void FindTarget(){
        // foreach(Transform child in chickenContainer.transform)
        // {
        //     if(child.transform.GetComponentInChildren<SpriteRenderer>().sortingLayerName.Contains("Special")){
        //         specialChickens.Add(child);
        //     }
        // }
        specialChicken = GetClosestSpecialChicken();
        if(specialChicken != null){
            CastLineToChicken();
        }
    }

    private void Update(){
        if(specialChicken != null)
            CastLineToChicken();
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
                // if(child.GetComponentInChildren<SpriteRenderer>().sortingLayerName.Contains("Special")){
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = child.gameObject;
                // }
            }
        }
        return bestTarget.GetComponent<ChickenMovement>().chickenSprite;
    }

    private void CastLineToChicken(){
        lineRemderer.positionCount = 2;
        lineRemderer.SetPosition(0, gameObject.transform.position);
        lineRemderer.SetPosition(1, specialChicken.transform.position);
    }

}
