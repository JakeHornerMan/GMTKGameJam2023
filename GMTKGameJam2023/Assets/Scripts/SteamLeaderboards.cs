using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System.Threading;

public class SteamLeaderboards : MonoBehaviour
{
    private const string s_leaderboardName = "LEADERBOARD";
    private const ELeaderboardUploadScoreMethod s_leaderboardMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest;


    private static SteamLeaderboard_t currentLeaderboard;
    public static bool initialized = false;
    private static string playerName;
    public static List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    public static List<LeaderboardEntry> top10LeaderboardEntries = new List<LeaderboardEntry>();
    public static List<LeaderboardEntry> friendsLeaderboardEntries = new List<LeaderboardEntry>();
    public enum LeaderboardType { Leaderboard, Top10, Friends }
    public static LeaderboardManager leaderboardManager;
    private static int score;
    private static int roundNumber;

    private static CallResult<LeaderboardFindResult_t> leaderboardFindResult = new CallResult<LeaderboardFindResult_t>();
    private static CallResult<LeaderboardScoreUploaded_t> uploadScoreResult = new CallResult<LeaderboardScoreUploaded_t>();
    private static CallResult<LeaderboardScoresDownloaded_t> topScoreResult = new CallResult<LeaderboardScoresDownloaded_t>();
    private static CallResult<LeaderboardScoresDownloaded_t> aroundPlayerScoreResult = new CallResult<LeaderboardScoresDownloaded_t>();

    public static void Init()
    {
        SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(s_leaderboardName);
        leaderboardFindResult.Set(hSteamAPICall, OnLeaderboardFindResult);
        playerName = SteamFriends.GetPersonaName();
        leaderboardManager = FindObjectOfType<LeaderboardManager>();
        InitTimer();
    }

    public static void InitAndUpdateScore(int uploadScore, int roundNum)
    {
        if (!initialized)
        {
            score = uploadScore;
            roundNumber = roundNum;
            SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(s_leaderboardName);
            leaderboardFindResult.Set(hSteamAPICall, OnLeaderboardFoundUpdateScores123);
            InitTimer();
        }
        else
        {
            UpdateScore(uploadScore, roundNum);
        }
    }

    static private void OnLeaderboardFoundUpdateScores123(LeaderboardFindResult_t pCallback, bool failure)
    {
        UnityEngine.Debug.Log("STEAM LEADERBOARDS: Found - " + pCallback.m_bLeaderboardFound + " leaderboardID - " + pCallback.m_hSteamLeaderboard.m_SteamLeaderboard);
        currentLeaderboard = pCallback.m_hSteamLeaderboard;
        initialized = true;
        UpdateScore(score, roundNumber);
        score = 0;
        roundNumber = 0;
    }

    public static void InitAndFindScores()
    {
        SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(s_leaderboardName);
        leaderboardFindResult.Set(hSteamAPICall, OnLeaderboardFoundGetScores);
        playerName = SteamFriends.GetPersonaName();
        leaderboardManager = FindObjectOfType<LeaderboardManager>();
        InitTimer();
    }

    static private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool failure)
    {
        UnityEngine.Debug.Log("STEAM LEADERBOARDS: Found - " + pCallback.m_bLeaderboardFound + " leaderboardID - " + pCallback.m_hSteamLeaderboard.m_SteamLeaderboard);
        currentLeaderboard = pCallback.m_hSteamLeaderboard;
        initialized = true;
    }

    static private void OnLeaderboardFoundGetScores(LeaderboardFindResult_t pCallback, bool failure)
    {
        UnityEngine.Debug.Log("STEAM LEADERBOARDS: Found - " + pCallback.m_bLeaderboardFound + " leaderboardID - " + pCallback.m_hSteamLeaderboard.m_SteamLeaderboard);
        currentLeaderboard = pCallback.m_hSteamLeaderboard;
        initialized = true;
        DownloadScoresAroundUser();
        DownloadScoresTop();
        DownloadScoresForFriends();
    }

    public static void UpdateScore(int score, int roundNum)
    {
        int[] roundNums = new int[] { roundNum };
        if (!initialized)
        {
            UnityEngine.Debug.Log("Can't upload to the leaderboard because isn't loadded yet");
        }
        else
        {
            UnityEngine.Debug.Log("uploading score(" + score + ") to steam leaderboard(" + s_leaderboardName + ")");
            SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(currentLeaderboard, s_leaderboardMethod, score, roundNums, roundNums.Length);
            uploadScoreResult.Set(hSteamAPICall, OnLeaderboardUploadResult);
        }
    }

    static private void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t pCallback, bool failure)
    {
        UnityEngine.Debug.Log("STEAM LEADERBOARDS: failure - " + failure + " Completed - " + pCallback.m_bSuccess + " NewScore: " + pCallback.m_nGlobalRankNew + " Score " + pCallback.m_nScore + " HasChanged - " + pCallback.m_bScoreChanged);
        InitAndFindScores();
    }

    public static void DownloadScoresTop(int toValue = 10, LeaderboardType currentLeaderboardType = LeaderboardType.Top10)
    {
        if (initialized)
        {
            SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(currentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, toValue);
            // topScoreResult.Set(handle, OnLeaderboardScoresDownloaded);
            CallResult<LeaderboardScoresDownloaded_t> callResult = new CallResult<LeaderboardScoresDownloaded_t>();
            callResult.Set(handle, (result, failure) => OnLeaderboardScoresDownloaded(result, failure, LeaderboardType.Top10));

        }
        else
        {
            Debug.LogError("Leaderboard not found. Cannot download scores.");
        }
    }

    public static void DownloadScoresAroundUser()
    {
        if (initialized)
        {
            SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(currentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -50, 50);
            // aroundPlayerScoreResult.Set(handle, OnLeaderboardScoresDownloaded);
            CallResult<LeaderboardScoresDownloaded_t> callResult = new CallResult<LeaderboardScoresDownloaded_t>();
            callResult.Set(handle, (result, failure) => OnLeaderboardScoresDownloaded(result, failure, LeaderboardType.Leaderboard));

        }
        else
        {
            Debug.LogError("Leaderboard not found. Cannot download scores.");
        }
    }

    public static void DownloadScoresForFriends()
    {
        if (initialized)
        {
            SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(currentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends, 0, 0);
            CallResult<LeaderboardScoresDownloaded_t> callResult = new CallResult<LeaderboardScoresDownloaded_t>();
            // callResult.Set(handle, OnLeaderboardScoresDownloaded);
            callResult.Set(handle, (result, failure) => OnLeaderboardScoresDownloaded(result, failure, LeaderboardType.Friends));

        }
        else
        {
            Debug.LogError("Leaderboard not found. Cannot download scores.");
        }
    }

    private static void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t result, bool failure, LeaderboardType currentLeaderboardType)
    {
        if (failure || result.m_cEntryCount == 0)
        {
            Debug.LogError("Failed to download leaderboard scores or no entries found.");
            DownloadScoresTop(20, LeaderboardType.Leaderboard);
            return;
        }

        if (currentLeaderboardType == LeaderboardType.Leaderboard)
        {
            PopulateListWithLeaderboardEntries(leaderboardEntries, result, currentLeaderboardType);
        }
        if (currentLeaderboardType == LeaderboardType.Top10)
        {
            PopulateListWithLeaderboardEntries(top10LeaderboardEntries, result, currentLeaderboardType);
        }
        if (currentLeaderboardType == LeaderboardType.Friends)
        {
            PopulateListWithLeaderboardEntries(friendsLeaderboardEntries, result, currentLeaderboardType);
        }
    }

    public static void PopulateListWithLeaderboardEntries(List<LeaderboardEntry> list, LeaderboardScoresDownloaded_t result, LeaderboardType currentLeaderboardType)
    {
        list.Clear();

        for (int i = 0; i < result.m_cEntryCount; i++)
        {
            LeaderboardEntry_t entry;
            int[] details = new int[1];

            SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, i, out entry, details, details.Length);
            string userName = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser);
            bool isPlayer = (userName == playerName) ? true : false;
            list.Add(new LeaderboardEntry(entry.m_steamIDUser, entry.m_nGlobalRank, entry.m_nScore, userName, isPlayer, details[0]));
        }

        // foreach (var entry in list)
        // {
        //     Debug.Log($"Rank: {entry.GlobalRank}, Score: {entry.Score}, User: {entry.UserName}");
        // }

        if (currentLeaderboardType == LeaderboardType.Leaderboard)
        {
            leaderboardManager.SetLeaderboardList();
        }
        if (currentLeaderboardType == LeaderboardType.Top10)
        {
            leaderboardManager.SetTop10LeaderboardList();
        }
        if (currentLeaderboardType == LeaderboardType.Friends)
        {
            leaderboardManager.SetFriendsLeaderboardList();
        }
    }

    private static Timer timer1;
    public static void InitTimer()
    {
        timer1 = new Timer(timer1_Tick, null, 0, 1000);
    }

    private static void timer1_Tick(object state)
    {
        SteamAPI.RunCallbacks();
    }
}

public class LeaderboardEntry
{
    public CSteamID User;
    public int GlobalRank;
    public int Score;
    public string UserName;
    public bool IsPlayer;
    public int Rank;

    public LeaderboardEntry(CSteamID user, int globalRank, int score, string userName, bool isPlayer, int rank)
    {
        User = user;
        GlobalRank = globalRank;
        Score = score;
        UserName = userName;
        IsPlayer = isPlayer;
        Rank = rank;
    }

    public void Log()
    {
        // Uncomment if needed again
        // Debug.Log($"Rank: {GlobalRank}, Score: {Score}, User: {UserName}, isPlayer: {IsPlayer}, Rank: {Rank}");
    }
}
