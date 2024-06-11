using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardTab : MonoBehaviour
{
    [Header("Tab Settings")]
    [SerializeField] private LeaderboardManager.ViewType tabType;
    [SerializeField] private Sprite inactiveBtnImg;
    [SerializeField] private Sprite activeBtnImg;

    private UnityEngine.UI.Image btnSprite;
    private LeaderboardManager leaderboardManager;

    private void Awake()
    {
        leaderboardManager = FindObjectOfType<LeaderboardManager>();
        btnSprite = GetComponent<UnityEngine.UI.Image>();

        // Cars tab is active by default
        if (tabType == LeaderboardManager.ViewType.Leaderboard)
            btnSprite.sprite = activeBtnImg;
        else
            btnSprite.sprite = inactiveBtnImg;
    }

    private void Update()
    {
        // If button tab is currently active, set sprite accordingly
        if (leaderboardManager.currentViewType == tabType)
            btnSprite.sprite = activeBtnImg;
        else
            btnSprite.sprite = inactiveBtnImg;
    }

    public void OnClick()
    {
        // Run corresponding function based on Type of button
        switch (tabType)
        {
            case LeaderboardManager.ViewType.Leaderboard:
                leaderboardManager.ShowLeaderboard();
                break;
            case LeaderboardManager.ViewType.Top10:
                leaderboardManager.ShowTop10();
                break;
            case LeaderboardManager.ViewType.Friends:
                leaderboardManager.ShowFriends();
                break;
        }
    }
}
