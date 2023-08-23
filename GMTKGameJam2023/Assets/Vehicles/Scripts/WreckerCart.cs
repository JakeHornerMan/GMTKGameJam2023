using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckerCart : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 60;
    private Vector3 currentEulerAngles; 
    [SerializeField] private float left = 1;
    [SerializeField] private Transform pivotPoint;

    private void Update(){
        currentEulerAngles += new Vector3(0,0, left) * Time.deltaTime * rotationSpeed;
        pivotPoint.transform.localEulerAngles = currentEulerAngles;
    }

}
