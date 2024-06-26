using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System;

public class LeaderboardEntryVisual : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField] private Image entrySpriteImg;
    [SerializeField] private GameObject selfIcon;
    [SerializeField] private GameObject trophyIcon;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Color playerColor;

    [Header("Sprite images")]
    [SerializeField] private Sprite normalEntrySprite;
    [SerializeField] private Sprite firstPlaceEntrySprite;
    [SerializeField] private Sprite selfEntrySprite;

    public void SetValues(LeaderboardEntry leaderboardEntry)
    {
        trophyIcon.SetActive(false);
        selfIcon.SetActive(false);

        rankText.text = leaderboardEntry.GlobalRank.ToString() + ".";
        usernameText.text = leaderboardEntry.UserName.ToString();
        scoreText.text = leaderboardEntry.Score.ToString();

        entrySpriteImg.sprite = normalEntrySprite;

        if (leaderboardEntry.IsPlayer)
        {
            rankText.color = playerColor;
            usernameText.color = playerColor;
            scoreText.color = playerColor;

            entrySpriteImg.sprite = selfEntrySprite;
            selfIcon.SetActive(true);
        }

        if (leaderboardEntry.GlobalRank == 1)
        {
            entrySpriteImg.sprite = firstPlaceEntrySprite;
            trophyIcon.SetActive(true);
        }
    }
}
