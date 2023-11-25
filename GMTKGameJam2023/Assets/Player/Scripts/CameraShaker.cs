using UnityEngine;
using System.Collections;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{

    public CinemachineVirtualCamera _cameraVCAM;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;


    public static CameraShaker instance;


    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        cinemachineBasicMultiChannelPerlin = _cameraVCAM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    //public IEnumerator Shake(float duration, float magnitude)
    //{
    //    // Store Original Camera Position
    //    Vector3 originalPos = transform.localPosition;

    //    // Store Elapsed Time
    //    float elapsed = 0f;

    //    // Repeat Until Duration 
    //    while (elapsed < duration)
    //    {
    //        // Get Random X,Y Values
    //        float x = Random.Range(-1f, 1f) * magnitude;
    //        float y = Random.Range(-1f, 1f) * magnitude;

    //        // Set Local Position
    //        transform.localPosition = new Vector3(x, y, originalPos.z);

    //        // Increased Elapsed Time
    //        elapsed += Time.deltaTime;

    //        yield return null;
    //    }

    //    // Return to Original Position
    //    transform.localPosition = originalPos;
    //}

    public void Shake(float time, float intensity)
    {
        StartCoroutine(ShakeCamera(time, intensity));
    }

    public void ShakeDefault()
    {
        StartCoroutine(ShakeCamera(1, 5));
    }

    public IEnumerator ShakeCamera(float time, float intensity)
    {

        float startingIntensity = intensity;
        float shakeTimerTotal = time;
        float shakeTimer = time;

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        while (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, (1 - shakeTimer / shakeTimerTotal));
            yield return null;
        }
    }

    
}
