using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStop : MonoBehaviour
{
    public float stopTime = 3.5f;

    private void Start(){
        SetPosition();
    }

    private void SetPosition(){
        Vector3 pos = gameObject.transform.position; 
        float randomNumber = Random.Range(-4f, 6f);
        gameObject.transform.position = new Vector3(pos.x, randomNumber, pos.z);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bus")){
            Debug.Log("Bus!!!");
            StartCoroutine(StopBusForTime(collision));
        }
    }

    private IEnumerator StopBusForTime(Collider2D collision){
        float speed = collision.GetComponent<Car>().carSpeed;
        collision.GetComponent<Car>().SetCarSpeed(0);
        Debug.Log("0f");
        yield return new WaitForSeconds(stopTime); 
        Debug.Log(speed);
        collision.GetComponent<Car>().SetCarSpeed(speed);
    }
}
