using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    private float currentTimeScale;
    private Coroutine hitStopRoutine;

    [HideInInspector] public static HitStop instance;

    public Pause pause;

    private void Start()
    {
        instance = this;
    }

    public void StartHitStop(float hitStopLength)
    {
        if (hitStopRoutine != null)
        {
            StopCoroutine(hitStopRoutine);
        }
        hitStopRoutine = StartCoroutine(HitStopCoroutine(hitStopLength));
    }

    private IEnumerator HitStopCoroutine(float hitStopLength)
    {
        // If GetMeOut coroutine is active, kill it, then start it up from the beginning
        if (hitStopRoutine != null)
        {
            StopCoroutine(hitStopRoutine);
        }
        hitStopRoutine = StartCoroutine(GetMeOutOfTheHitStop());

        currentTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;

        yield return new WaitForSecondsRealtime(hitStopLength);

        Time.timeScale = currentTimeScale;
    }

    private IEnumerator GetMeOutOfTheHitStop()
    {
        float timeInHitStop = 0.0f;

        while (Time.timeScale == 0.0f && !pause.isPaused)
        {
            timeInHitStop += Time.unscaledDeltaTime;

            // If time.timeScale has been zero for over a second, force it to 1.0f unless the game is paused
            if (timeInHitStop > 1.0f)
            {
                Time.timeScale = 1.0f;
                yield break; // exit the coroutine once time scale is restored
            }

            yield return null;
        }
    }
}