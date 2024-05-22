using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyTrain : Ultimate
{
    [Header("Train Movement")]
    [SerializeField] private Vector2 startPos = new(0, 0);
    // private Vector3 endPos;
    [SerializeField] private float shakeIntensity = 5f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float xDestroyThreshold = -505f;

    [Header("Train Ref")]
    private VehicleSpawner vehicleSpawner;

    private void Start()
    {
        Debug.Log("Train Spawn!");
        vehicleSpawner = FindObjectOfType<VehicleSpawner>();
        transform.position = CalculateStartPosition();
        // endPos = CalculateEndPosition();
        CameraShaker.instance.Shake(7.5f, shakeIntensity);
    }

    // Update is called once per frame
    void Update()
    {
        MoveTrain();
    }

    public void MoveTrain(){
        Vector3 direction = Vector3.left;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // CameraShaker.instance.Shake(7.5f, transform.position.y * shakeIntensity);

        if (transform.position.x <= xDestroyThreshold)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        Debug.Log(transform.gameObject.name);
    }

    public Vector3 CalculateStartPosition(){
        Vector3 inputPos = vehicleSpawner.inputPos;
        return new Vector3(startPos.x, inputPos.y, 0);
    }

    // public Vector3 CalculateEndPosition(){
    //     Vector3 inputPos = vehicleSpawner.inputPos;
    //     return new Vector3(xDestroyThreshold, inputPos.y, 0);
    // }
}
