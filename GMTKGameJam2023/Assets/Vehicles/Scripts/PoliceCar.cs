using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCar : Car
{
    [Header("Camera Shake")]
    [SerializeField] private float policeShakeDuration = 3f;
    [SerializeField] private float policeShakeMagnitude = 0.25f;

    [Header("Which method of placing spikes?")]
    [SerializeField] private bool placeMultipleInLine = false;
    [SerializeField] private bool placeOnClickLocation = false;

    [Header("Spike Strip Parameters")]
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private float numberOfSpikes = 3f;
    private float numberOfSpikesDeployed = 1f;
    [SerializeField] private float laneLength = 17f;

    void Start()
    {
        SetCarSpeed();

        numberOfSpikesDeployed = 1f;

        soundManager.PlayNewPoliceCar();

        // Shake Camera
        StartCoroutine(cameraShaker.Shake(policeShakeDuration, policeShakeMagnitude));

        //StartCoroutine(WaitForBombStart());
    }

    private void Update()
    {
        if (numberOfSpikesDeployed <= numberOfSpikes)
        {
            if (gameObject.transform.position.y > ((laneLength * -1) / 2) + (((laneLength / numberOfSpikes) * numberOfSpikesDeployed) - (laneLength / 8)))
            {
                DeploySpikes();
            }
        }
    }

    private void DeploySpikes()
    {

        float spikeY = transform.position.y;

        float spikeX = transform.position.x;


        Vector3 spikePosition = new Vector3(spikeX, spikeY, 1);

        GameObject currentSpike = Instantiate(spikePrefab, spikePosition, Quaternion.identity);

        numberOfSpikesDeployed++;

    }
}
