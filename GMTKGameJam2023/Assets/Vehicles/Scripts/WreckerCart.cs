using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckerCart : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 60;
    private Vector3 currentEulerAngles; 
    [SerializeField] public float left = 1;
    [SerializeField] private Transform pivotPoint;
    private bool hitStop = false;
    [SerializeField] private float carHitStopEffectMultiplier = 0.5f;

    private void Update(){
        if(!hitStop){
            currentEulerAngles += new Vector3(0,0, left) * Time.deltaTime * rotationSpeed;
            pivotPoint.transform.localEulerAngles = currentEulerAngles;
        }
    }

    public IEnumerator HandleHitStop(float hitStopLength)
    {
        hitStop = true;

        yield return new WaitForSecondsRealtime(hitStopLength * carHitStopEffectMultiplier);

        hitStop = false;
    }

}
