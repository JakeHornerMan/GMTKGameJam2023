using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    private float currentTimeScale;

    [HideInInspector] public static HitStop instance;

    private void Start()
    {
        instance = this;
    }

    public void StartHitStop(float hitStopLength)
    {
        StartCoroutine(HitStopCoroutine(hitStopLength));
    }

    private IEnumerator HitStopCoroutine(float hitStopLength)
    {
        currentTimeScale = Time.timeScale;

        Time.timeScale = 0.0f;

        yield return new WaitForSecondsRealtime(hitStopLength);

        Time.timeScale = currentTimeScale;
    }


}
