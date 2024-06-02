using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
/// Kills chicken by iterating throguh the list and killing at regular intervals.
/// For example, 25% kill => kill every 4th chicken, 50% => kill every second chicken
/// </summary>
public class CardDraw : Ultimate
{
    [Header("Settings")]
    [SerializeField] List<UltimateSelectableCard> cardsList; // The 3 Cards (UI Elements)
    [SerializeField] List<UltimateCardSO> cardConfigs; // The Scriptable Objects for Possible Card Values
    [Space]
    [SerializeField] private bool hideUIOnUltimateActivation = true;

    private Transform chickenContainer;

    private List<UltimateCardSO> shuffledConfigs;

    private void Awake()
    {
        chickenContainer = GameObject.Find("ChickenContainer").transform;
    }

    private void Start()
    {
        // Pause Game and Hide UI If Setting is enabled
        if (hideUIOnUltimateActivation)
        {
            // Find "GameCanvas" Object
            GameObject.Find("GameCanvas").SetActive(false);
        }
        Time.timeScale = 0f;

        // Assign Configs Randomly on cards List
        shuffledConfigs = Shuffle(cardConfigs);
        for (int i = 0; i < cardsList.Count; i++)
        {
            UltimateSelectableCard card = cardsList[i];
            card.correspondingConfig = shuffledConfigs[i];
        }
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

    public void HandleSelectedCard(UltimateSelectableCard cardClicked)
    {
        // Flip every card to front
        foreach (UltimateSelectableCard card in cardsList)
        {
            card.FlipCard(front: true);
            card.SetUIActivation(true);
        }

        if (cardClicked.correspondingConfig.cardType == UltimateCardSO.UltimateCardType.KillChickenFraction)
        {
            if (cardClicked.correspondingConfig.chickenKillInterval == 0)
            {
                return;
            }
            // Kill Chicken Amount Here using interval value from Scriptable Object
            ChickenHealth[] chickens = chickenContainer.GetComponentsInChildren<ChickenHealth>();
            for (int i = 0; i < chickens.Length; i++)
            {
                // Use Remainder to kill using interval
                if ((i + 1) % cardClicked.correspondingConfig.chickenKillInterval == 0)
                    chickens[i].TakeDamage(1000);
            }
        }

        ResumeAndEnd();
    }

    /// <summary>
    /// Unpause Game, Re-enable UI, and Destroy Self
    /// Run by Resume Button
    /// </summary>
    public void ResumeAndEnd()
    {
        GameObject.Find("GameCanvas").SetActive(true);
        Time.timeScale = 1f;
        Destroy(gameObject);
    }
}
