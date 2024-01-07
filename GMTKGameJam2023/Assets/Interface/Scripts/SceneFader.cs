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

    [Header("References")]
    [SerializeField] private Image fadeImage;

    [Header("Transition Values")]
    [SerializeField] private AnimationCurve fadeAnimationCurve;

    private void Start()
    {
        StartCoroutine(FadeIn());
        levelSceneName = NameFromIndex(GameProgressionValues.sceneIndex);
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
}
