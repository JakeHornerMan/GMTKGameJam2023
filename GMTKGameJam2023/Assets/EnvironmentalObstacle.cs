using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Car;

public class EnvironmentalObstacle : MonoBehaviour
{
    public bool isCollidable = true;

    public bool isKnockable = false;

    private Rigidbody2D rb;

    [SerializeField] private float launchSpeed = 0.66f;

    [SerializeField] private bool canSpin = false;
    [SerializeField] private float degreesPerSecond = 540f;
    private bool isSpinning;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isKnockable)
        {
            LaunchObject();
        }
    }

    private IEnumerator Spin()
    {
        while (isSpinning)
        {
            transform.Rotate(Vector3.forward * degreesPerSecond * Time.deltaTime);
            yield return null; // This makes the coroutine wait for the next frame
        }
    }

    // Example method to start spinning
    public void StartSpinning()
    {
        isSpinning = true;
        StartCoroutine(Spin());
    }

    // Example method to stop spinning
    public void StopSpinning()
    {
        isSpinning = false;
    }

    public void LaunchObject()
    {
        // Generate random x and y components between -10 and 10
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomY = UnityEngine.Random.Range(-1f, 1f);

        // Round to -0.5 or 0.5 if the number is between -0.5 and 0.5
        if (randomX > -0.5f && randomX < 0.5f)
        {
            randomX = (randomX > 0) ? 0.5f : -0.5f;
        }

        if (randomY > -0.5f && randomY < 0.5f)
        {
            randomY = (randomY > 0) ? 0.5f : -0.5f;
        }

        // Create a Vector2 with the random components
        Vector2 randomVector = new Vector2(randomX, randomY);

        // Normalize the Vector2
        Vector2 normalizedVector = randomVector.normalized;

        GetComponent<Collider2D>().enabled = false;

        HitStop.instance.StartHitStop(0.05f);
        CameraShaker.instance.ShakeCamera(0.1f, 0.5f);

        if (rb != null)
        {
            rb.velocity = normalizedVector * 25;
        }
        

        //if (carType == CarType.Heavy || carType == CarType.Light)
        //{
        //    GameObject currentBomb = Instantiate(DeathExplosion, transform.position, Quaternion.identity);

        //    Destroy(currentBomb, 0.85f);
        //}

        StartCoroutine(LaunchObjectCoroutine(new Vector3(15, 15, 1), new Vector3(normalizedVector.x, normalizedVector.y, gameObject.transform.position.z)));
    }

    private IEnumerator LaunchObjectCoroutine(Vector3 targetScale, Vector3 targetPos)
    {
        Vector3 initialScale = gameObject.transform.localScale;
        Vector3 initialPos = gameObject.transform.position;
        float t = 0;

        float launchSpeed = 1f / 1.5f;

        
        if (canSpin == true)
        {
            StartSpinning();
        }

        while (t < 1)
        {
            t += Time.deltaTime * launchSpeed;

            // Linearly interpolate vehicle scale
            gameObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            yield return null;
        }
    }
}
