using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Tags")]
    [SerializeField] private string musicTag = "Music";

    private AudioSource musicAudioSource;

    [SerializeField] private GameObject backButton;

    [SerializeField] private SceneFader sceneFader;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        musicAudioSource = GameObject.FindGameObjectWithTag(musicTag).GetComponent<AudioSource>();
    }

    public void ShowBackButton()
    {
        backButton.SetActive(true);

        backButton.GetComponent<Animator>().Play("ShowBackButton");
    }

    public void HideBackButton()
    {
        //backButton.SetActive(false);

        backButton.GetComponent<Animator>().Play("HideBackButton");
    }

    public void EnterGame()
    {
        StartCoroutine(WipeAndLoadGame());   
    }

    private IEnumerator WipeAndLoadGame()
    {
        yield return new WaitForSeconds(2.5f);

        sceneFader.ScreenWipeOut("Level01");
    }
}
