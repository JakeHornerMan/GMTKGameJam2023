using System.Collections;
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
    [SerializeField] UltimateSelectableCard cardsList; // The 3 Cards (UI Elements)
    [SerializeField] UltimateCardSO cardConfigs; // The Scriptable Objects for Possible Card Values
    [Space]
    [SerializeField] private bool hideUIOnUltimateActivation = true;

    private void Start()
    {
        // Pause Game and Hide UI If Setting is enabled
        if (hideUIOnUltimateActivation)
        {
            // Find "GameCanvas" Object
        }

        // Assign Configs Randomly on cards List
    }

    public void HandleSelectedCard(UltimateSelectableCard cardClicked)
    {
        if (cardClicked.correspondingConfig.cardType == UltimateCardSO.UltimateCardType.KillChickenFraction)
        {
            // Kill Chicken Amount Here using interval value from Scriptable Object
        }
    }
}
