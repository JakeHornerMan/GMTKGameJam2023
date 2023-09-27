using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuMove : MonoBehaviour
{
    public Vector3 targetPos = new Vector3(0, 0, -10);
    private float speed = 4f;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}
