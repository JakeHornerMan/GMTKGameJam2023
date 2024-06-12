using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System;

public class LeaderboardEntryVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Color playerColor;

    public void SetValues(LeaderboardEntry leaderboardEntry)
    {
        rankText.text = leaderboardEntry.GlobalRank.ToString() + ".";
        usernameText.text = leaderboardEntry.UserName.ToString();
        scoreText.text = leaderboardEntry.Score.ToString();

        if(leaderboardEntry.IsPlayer){
            rankText.color = playerColor;
            usernameText.color = playerColor;
            scoreText.color = playerColor;
        }
    }
}
