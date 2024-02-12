using UnityEngine;
using System.Collections;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{

    public CinemachineVirtualCamera _cameraVCAM;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;


    public static CameraShaker instance;

    // keep a copy of the executing script
    private IEnumerator coroutine;

    private bool isCameraShaking = false;


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
        // if (isCameraShaking)
        // {
        //     if (cinemachineBasicMultiChannelPerlin.m_AmplitudeGain < intensity)
        //     {
        //         StopCoroutine(coroutine);
        //         coroutine = ShakeCamera(time, intensity);
        //         StartCoroutine(coroutine);
        //     }
        // }
        // else
        // {
        //     coroutine = ShakeCamera(time, intensity);
        //     StartCoroutine(coroutine);
        // }
        
    }

    public void ShakeDefault()
    {
        StartCoroutine(ShakeCamera(1, 5));
    }

    private IEnumerator ShakeCamera(float time, float intensity)
    {
        isCameraShaking = true;

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

        isCameraShaking = false;
    }

    
}
