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

    private List<UltimateCardSO> shuffledConfigs;

    private void Awake()
    {
        normalChickenContainer = GameObject.Find("ChickenContainer").transform;
        specialChickenContainer = GameObject.Find("SpecialChickenContainer").transform;
    }

    private void Start()
    {
        resumeBtn.SetActive(false);

        // Pause Game and Hide UI If Setting is enabled
        if (hideUIOnUltimateActivation)
        {
            // Find "GameCanvas" Object
            GameObject.Find("GameCanvas").SetActive(false);
        }
        Time.timeScale = 0f;

        // Shuffle the card configs and assign them to the cards
        shuffledConfigs = Shuffle(cardConfigs);

        // Set all cards to inactive initially
        foreach (var card in cardsList)
        {
            card.gameObject.SetActive(false);
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
        }

        // Show on UI that this card is "SELECTED"
        cardClicked.ShowSelectedLabel();

        if (cardClicked.correspondingConfig.cardType == UltimateCardSO.UltimateCardType.KillChickenFraction)
        {
            chosenKillInterval = cardClicked.correspondingConfig.chickenKillInterval;
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
        Time.timeScale = 1f;

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
        SoundManager soundManager = FindObjectOfType<SoundManager>();

        // Combine normal and special chickens
        ChickenHealth[] normalChickens = normalChickenContainer.GetComponentsInChildren<ChickenHealth>();
        ChickenHealth[] specialChickens = specialChickenContainer.GetComponentsInChildren<ChickenHealth>();
        ChickenHealth[] allChickens = normalChickens.Concat(specialChickens).ToArray();

        for (int i = 0; i < allChickens.Length; i++)
        {
            // Use Remainder to kill using interval
            if ((i + 1) % chosenKillInterval == 0)
            {
                if (allChickens[i] == null) continue;
                allChickens[i].TakeDamage(1000);
                // TODO ADD SCORE TO GAMEMANAGER
                yield return new WaitForSecondsRealtime(killDelay);
            }
        }

        Destroy(gameObject);
    }
}
