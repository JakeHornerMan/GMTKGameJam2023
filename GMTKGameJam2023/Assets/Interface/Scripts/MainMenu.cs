using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string musicTag = "Music";

    private AudioSource musicAudioSource;
    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    private void Start()
    {
        musicAudioSource = GameObject.FindGameObjectWithTag(musicTag).GetComponent<AudioSource>();
    }

    private void Update()
    {
        musicAudioSource.mute = !Settings.musicAllowed;
    }

    public void EnterWorldSelect() => sceneFader.FadeToWorlds();
    public void EnterTutorial() => sceneFader.FadeToTutorial();
    public void EnterCredits() => sceneFader.FadeToCredits();
    public void EnterSettings() => sceneFader.FadeToSettings();

    public void QuitGame() => Application.Quit();
}
