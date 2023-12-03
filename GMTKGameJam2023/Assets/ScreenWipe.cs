using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenWipe : MonoBehaviour
{

    [SerializeField] private Image screenWipeImage;
    [SerializeField] private float startingScreenWipeX;
    [SerializeField] private float timeToWipe;

    private void Start()
    {
        startingScreenWipeX = screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition.x;

        ScreenWipeOut();
    }
    public void ScreenWipeIn()
    {
        StartCoroutine(MoveInScreenWiper());
    }

    public void ScreenWipeOut()
    {
        StartCoroutine(MoveOutScreenWiper());
    }

    IEnumerator MoveInScreenWiper()
    {

        screenWipeImage.gameObject.SetActive(true);

        screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(startingScreenWipeX, 0f, 0f);

        // Loop until the alpha reaches 1
        float elapsedTime = 0f;

        float currentX = screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition.x;

        while (screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition.x > 0)
        {
            float newX = Mathf.Lerp(currentX, 0.0f, elapsedTime / timeToWipe);

            screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(newX, 0f, 0f);

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator MoveOutScreenWiper()
    {

        screenWipeImage.gameObject.SetActive(true);

        screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);

        // Loop until the alpha reaches 1
        float elapsedTime = 0f;

        float currentX = screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition.x;

        while (screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition.x > -startingScreenWipeX)
        {
            float newX = Mathf.Lerp(currentX, -startingScreenWipeX, elapsedTime / timeToWipe);

            screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(newX, 0f, 0f);

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}
