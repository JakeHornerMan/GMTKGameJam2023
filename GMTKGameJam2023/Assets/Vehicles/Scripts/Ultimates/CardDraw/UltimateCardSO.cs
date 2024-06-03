using System.ComponentModel;
using UnityEngine;

/// <summary>
/// Part of Card Draw Ultimate
/// Defines possible type of card and corresponding values.
/// </summary>

[CreateAssetMenu(fileName = "UltimateCard", menuName = "Possible Ultimate Card Condig", order = 0)]
public class UltimateCardSO : ScriptableObject
{
    public enum UltimateCardType
    {
        KillChickenFraction
    }

    [Header("Main Settings")]
    [SerializeField] public UltimateCardType cardType;

    [Header("Settings for Kill Chicken Fraction")]
    [Description("Cards that will kill a certain fraction of chicken when selected.")]

    [Tooltip("Step for how many chicken to kill, e.g. 4 = 25%, 2 = 50%, etc.")]
    [SerializeField] public int chickenKillInterval = 0;
    [SerializeField] public string percentageString = "0%"; // Used to show on card UI
    [SerializeField] public string descriptionString = "Kills {%} of chicken"; // {%} to be replaced with percentageString.

}