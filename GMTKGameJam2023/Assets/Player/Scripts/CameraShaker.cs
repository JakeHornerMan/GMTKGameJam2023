using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        // Store Original Camera Position
        Vector3 originalPos = transform.localPosition;

        // Store Elapsed Time
        float elapsed = 0f;

        // Repeat Until Duration 
        while (elapsed < duration)
        {
            // Get Random X,Y Values
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Set Local Position
            transform.localPosition = new Vector3(x, y, originalPos.z);

            // Increased Elapsed Time
            elapsed += Time.deltaTime;

            yield return null;
        }

        // Return to Original Position
        transform.localPosition = originalPos;
    }
}
