using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [Header("Scene Loading")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField]private string levelSceneName;
    [SerializeField] private string worldSelectSceneName = "WorldSelect";
    [SerializeField] private string levelSelectSceneName = "LevelSelect";
    [SerializeField] private string tutorialSceneName = "Tutorial";
    [SerializeField] private string creditsSceneName = "Credits";
    [SerializeField] private string settingsSceneName = "Settings";
    [SerializeField] private string resultsSceneName = "Results";

    [Header("Fade References")]
    [SerializeField] private Image fadeImage;

    [Header("Wipe References")]
    [SerializeField] private GameObject screenWipeObject;
    [SerializeField] private float offScreenXcoord = 2163f;
    [SerializeField] private float screenWipeDuration;

    [Header("Transition Values")]
    [SerializeField] private AnimationCurve fadeAnimationCurve;
    [SerializeField] private AnimationCurve wipeAnimationCurve;

    private void Start()
    {
        if (screenWipeObject != null)
        {
            ScreenWipeIn();
        }
        else
        {
            StartCoroutine(FadeIn());
        }
        
        levelSceneName = NameFromIndex(Points.sceneIndex);
    }

    public void FadeTo(string targetScene)
    {
        // Smoothly Transition Between Scenes
        StartCoroutine(FadeOut(targetScene));
    }

    // Scene Loading Functions
    public void ReloadScene() => FadeTo(SceneManager.GetActiveScene().name);
    public void RestartLevel() => FadeTo(levelSceneName);
    public void FadeToMainMenu() => FadeTo(mainMenuSceneName);
    public void FadeToWorlds() => FadeTo(worldSelectSceneName);
    public void FadeToLevelSelect() => FadeTo(levelSelectSceneName);
    public void FadeToTutorial() => FadeTo(tutorialSceneName);
    public void FadeToCredits() => FadeTo(creditsSceneName);
    public void FadeToSettings() => FadeTo(settingsSceneName);
    public void FadeToResults() => FadeTo(resultsSceneName);

    private IEnumerator FadeIn()
    {
        // Fade In Using Animation Curve
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = fadeAnimationCurve.Evaluate(t);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return 0;
        }
    }

    private IEnumerator FadeOut(string targetScene)
    {
        // Fade Out Using Reversed Animation Curve
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = fadeAnimationCurve.Evaluate(t);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return 0;
        }

        // Load Target Scene
        SceneManager.LoadScene(targetScene);
    }

    private string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
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

        screenWipeObject.gameObject.SetActive(true);
        float t = 0f; // Start at the beginning of the animation curve

        screenWipeObject.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0f, 0f);

        while (t < 1f)
        {
            t += Time.deltaTime / screenWipeDuration;
            float newX = Mathf.Lerp(0f, offScreenXcoord, wipeAnimationCurve.Evaluate(t));
            screenWipeObject.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(newX, 0f, 0f);
            yield return null;
        }

        // Optionally deactivate the screen wiper at the end
        screenWipeObject.gameObject.SetActive(false);

        //screenWipeImage.gameObject.SetActive(true);

        //screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);

        //// Loop until the alpha reaches 1
        //float elapsedTime = 0f;

        //float currentX = screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition.x;

        //while (screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition.x > -startingScreenWipeX)
        //{
        //    float newX = Mathf.Lerp(currentX, -startingScreenWipeX, elapsedTime / timeToWipe);

        //    screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(newX, 0f, 0f);

        //    elapsedTime += Time.deltaTime;

        //    yield return null;
        //}
    }

    IEnumerator MoveOutScreenWiper()
    {
        screenWipeObject.gameObject.SetActive(true);
        float t = 0f; // Start at the beginning of the animation curve

        screenWipeObject.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-offScreenXcoord, 0f, 0f);

        while (t < 1f)
        {
            t += Time.deltaTime / screenWipeDuration;
            float newX = Mathf.Lerp(offScreenXcoord, 0f, wipeAnimationCurve.Evaluate(t));
            screenWipeObject.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(newX, 0f, 0f);
            yield return null;
        }

        //screenWipeImage.gameObject.SetActive(true);

        //screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(startingScreenWipeX, 0f, 0f);

        //// Loop until the alpha reaches 1
        //float elapsedTime = 0f;

        //float currentX = screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition.x;

        //while (screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition.x > 0)
        //{
        //    float newX = Mathf.Lerp(currentX, 0.0f, elapsedTime / timeToWipe);

        //    screenWipeImage.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(newX, 0f, 0f);

        //    elapsedTime += Time.deltaTime;

        //    yield return null;
        //}
    }

    
}
