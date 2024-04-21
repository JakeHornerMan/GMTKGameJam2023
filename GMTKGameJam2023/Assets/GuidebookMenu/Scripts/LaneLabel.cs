using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for vehicles in Object Blueprint object. <br/>
/// <br/>
/// One of these LaneLabels is created for every placeable lane 
/// in the currently displayed car. <br/>
/// <br/>
/// It shows the lane's name (from its tag) and its sprite for a
/// quick visual guide.
/// </summary>

public class LaneLabel : MonoBehaviour
{
    [System.Serializable]
    private struct LaneNameImagePairs
    {
        public string laneTag;
        public SpriteRenderer laneSprite;
    }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI laneNameText;
    [SerializeField] private Image laneImage;

    [Header("Lane Image Settings")]
    [SerializeField] LaneNameImagePairs[] laneNameImagePairs;

    /// <summary>
    /// Sets label UI based on provided lane tag.
    /// Shows lane tag name and corresponding lane sprite.
    /// </summary>
    /// <param name="laneTag">Tag of the lane to display in label.</param>
    /// <returns>Sends back the tag of the lane</returns>
    public string SetLabelLane(string laneTag)
    {
        laneNameText.text = laneTag;
        // Find corresponding lane image based on reference array
        foreach (LaneNameImagePairs pair in laneNameImagePairs)
        {
            if (pair.laneTag == laneTag)
            {
                laneImage.sprite = pair.laneSprite.sprite;
                break;
            }
        }
        return laneTag;
    }
}
