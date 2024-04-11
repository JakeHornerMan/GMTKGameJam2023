using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] private float removeTime = 3f;
    public GameObject capturedVehicle;
    private float portalDisappearAnimLength = 1f;
    private float spawnAfterTime = 1f;
    private Animator anim;
    private SoundManager soundManager;
    void Start()
    {
        anim = GetComponent<Animator>();
        soundManager = FindObjectOfType<SoundManager>();
        // Debug.Log("Captured: " + capturedVehicle.name);
        StartCoroutine(WaitAndDie(removeTime));

        StartCoroutine(WaitAndSpawn(spawnAfterTime));
    }

    private IEnumerator WaitAndDie(float dieTime)
    {
        yield return new WaitForSeconds(dieTime - portalDisappearAnimLength);

        anim.Play("PortalDisappear");

        yield return new WaitForSeconds(portalDisappearAnimLength);

        Destroy(gameObject);
    }

    private IEnumerator WaitAndSpawn(float spawnAfterTime)
    {
        yield return new WaitForSeconds(spawnAfterTime);

        Vector3 spawnPos = new(transform.position.x, transform.position.y, 0);
        GameObject spawnedVehicle = Instantiate(capturedVehicle, spawnPos, Quaternion.identity);
        soundManager.PlayExitPortal();
        spawnedVehicle.SetActive(true);
        spawnedVehicle.GetComponent<Car>().carTeleporting = false;
    }
}
