using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ultimate lets user pick between 3 cards randomly, which will kill random number of chicken.
/// Either 0%, 25%, or 50%.
///
/// When clicked, pauses the game and shows its UI.
/// Has List of 3 Cards, assigns scriptable object randomCard to each of them randomly.
/// Scriptable Object contains text and fraction of chicken to kill
/// When one is clicked, it flips over to show its scriptable info on UI,
/// and tells this script to read its corresponding scriptableObject values and kill chicken accordingly.
///
/// Kills chicken by iterating through the list and killing at regular intervals.
/// For example, 25% kill => kill every 4th chicken, 50% => kill every second chicken
/// </summary>
public class CardDraw : Ultimate
{
    [Header("Settings")]
    [SerializeField] List<UltimateSelectableCard> cardsList; // The 3 Cards (UI Elements)
    [SerializeField] List<UltimateCardSO> cardConfigs; // The Scriptable Objects for Possible Card Values
    [SerializeField] GameObject[] ultimateUIs; // Disabled when resume button clicked
    [SerializeField] private float killDelay = 0.3f; // Delay between each chicken kill
    [SerializeField] private float cardCreationInterval = 0.5f; // Delay between card creation
    [Space]
    [SerializeField] private bool hideUIOnUltimateActivation = true;

    [Header("Card Draw UI References")]
    [SerializeField] private GameObject resumeBtn;
    private int chosenKillInterval = 0;

    private Transform normalChickenContainer;
    private Transform specialChickenContainer;

    private Pause pause;

    // private AudioSource musicAudio;
    // private AudioSource sfxAudio;

    private List<UltimateCardSO> shuffledConfigs;

    private void Awake()
    {
        normalChickenContainer = GameObject.Find("ChickenContainer").transform;
        specialChickenContainer = GameObject.Find("SpecialChickenContainer").transform;
        soundManager = FindObjectOfType<SoundManager>();

        // musicAudio = FindObjectOfType<GameManager>().GetComponent<AudioSource>();
        // sfxAudio = FindObjectOfType<GameManager>().GetComponent<AudioSource>();
        // Debug.Log(musicAudio.name + "" + sfxAudio.name);

        pause = FindObjectOfType<Pause>();
        soundManager?.PlaySound(0,spawnSound[0]);
    }

    private void Start()
    {
        resumeBtn.SetActive(false);

        //Hide UI If Setting is enabled
        if (hideUIOnUltimateActivation)
        {
            // Find "GameCanvas" Objectk
            GameObject.Find("GameCanvas").SetActive(false);
        }

        // Pause Game and  Audio
        pause.PauseGame(showUI: false);

        // Time.timeScale = 0f;
        // musicAudio.Pause();
        // sfxAudio.Pause();

        // Shuffle the card configs and assign them to the cards
        shuffledConfigs = Shuffle(cardConfigs);

        // Set all cards to inactive initially
        foreach (var card in cardsList)
        {
            card.gameObject.SetActive(false);
            soundManager?.PlaySound(0f ,spawnSound[1]);
        }

        // Start coroutine to enable cards one by one with delay
        StartCoroutine(ActivateCardsWithDelay());
    }

    private List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }

        return _list;
    }

    private IEnumerator ActivateCardsWithDelay()
    {
        for (int i = 0; i < cardsList.Count; i++)
        {
            var card = cardsList[i];
            card.correspondingConfig = shuffledConfigs[i];
            card.FlipCard(front: false); // Set to back side
            card.gameObject.SetActive(true); // Enable the card
            yield return new WaitForSecondsRealtime(cardCreationInterval);
        }
    }

    public void HandleSelectedCard(UltimateSelectableCard cardClicked)
    {
        resumeBtn.SetActive(true);

        // Flip every card to front
        foreach (UltimateSelectableCard card in cardsList)
        {
            card.GetComponent<Animator>().SetTrigger("Flip");
            card.FlipCard(front: true);
            card.SetUIActivation(true);
            card.GetComponent<Button>().interactable = false;
            soundManager?.PlaySound(0f ,spawnSound[5]);
        }

        // Show on UI that this card is "SELECTED"
        cardClicked.ShowSelectedLabel();

        if (cardClicked.correspondingConfig.cardType == UltimateCardSO.UltimateCardType.KillChickenFraction)
        {
            chosenKillInterval = cardClicked.correspondingConfig.chickenKillInterval;
            if(chosenKillInterval == 0) soundManager?.PlaySound(0f ,spawnSound[2]);
            if(chosenKillInterval == 4) soundManager?.PlaySound(0f ,spawnSound[3]);
            if(chosenKillInterval == 2) soundManager?.PlaySound(0f ,spawnSound[4]);
        }
    }

    /// <summary>
    /// Run by Resume Button
    /// Unpause Game, Re-enable UI, and Destroy Self
    /// Kill the chicken
    /// </summary>
    public void ResumeAndEnd()
    {
        GameObject.Find("GameCanvas").SetActive(true);

        // Unpause Game and Restart Music
        pause.UnpauseGame();
        // Time.timeScale = 1f;
        // musicAudio.UnPause();
        // sfxAudio.UnPause();

        // Deactivate the card draw UI
        foreach (GameObject obj in ultimateUIs)
        {
            obj.SetActive(false);
        }

        if (chosenKillInterval != 0)
        {
            // Start coroutine to kill chickens with delay
            StartCoroutine(KillChickensWithDelay());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator KillChickensWithDelay()
    {
        // SoundManager soundManager = FindObjectOfType<SoundManager>();

        // Combine normal and special chickens
        ChickenHealth[] normalChickens = normalChickenContainer.GetComponentsInChildren<ChickenHealth>();
        ChickenHealth[] specialChickens = specialChickenContainer.GetComponentsInChildren<ChickenHealth>();
        ChickenHealth[] allChickens = normalChickens.Concat(specialChickens).ToArray();
        soundManager?.PlaySound(0f ,spawnSound[6]);

        int totalChickenPoints = 0;
        int numberOfChickens = 0;

        gameManager = FindObjectOfType<GameManager>();
        for (int i = 0; i < allChickens.Length; i++)
        {
            // Use Remainder to kill using interval
            if ((i + 1) % chosenKillInterval == 0)
            {
                if (allChickens[i] == null) continue;
                allChickens[i].TakeDamage(1000);
                numberOfChickens++;
                int pointsToAdd = allChickens[i].pointsReward + ((numberOfChickens - 1) * 5);
                totalChickenPoints += pointsToAdd;
                ShowPopup(
                    allChickens[i].transform.position,
                    $"{pointsToAdd}"
                );
                //gameManager.AddPlayerScore(allChickens[i].pointsReward);
                yield return new WaitForSecondsRealtime(killDelay);
            }
        }

        gameManager.AddPlayerScore(totalChickenPoints);

        Destroy(gameObject);
    }

    private void ShowPopup(Vector3 position, string msg)
    {
        // Point Indicator
        ScorePopup newPopUp = Instantiate(
            scorePopUp,
            position,
            Quaternion.identity
        ).GetComponent<ScorePopup>();
        newPopUp.SetText(msg);
        Destroy(newPopUp.gameObject, 0.7f);
    }
}
