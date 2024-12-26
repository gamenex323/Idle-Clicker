using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System;

public class PlayFabLoginManager : MonoBehaviour
{
    public static PlayFabLoginManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LoginWithCustomID();
    }
    public void LoginWithCustomID()
    {
        string customId = SystemInfo.deviceUniqueIdentifier; // Unique ID for the device/user

        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = true // Create a new account if one doesn't exist
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login successful!");
        Debug.Log("PlayFab ID: " + result.PlayFabId);
        Debug.Log("Level Balance " + GameManager.instance.LevelData.Balance);
        UpdateSubmitScore(GameManager.instance.LevelData.Balance);
        SubmitScore(GameManager.instance.LevelData.Balance);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login failed: " + error.GenerateErrorReport());
    }

    public void LoginWithUsernameAndPassword(string username, string password)
    {
        var request = new LoginWithPlayFabRequest
        {
            Username = username,
            Password = password
        };

        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }

    public void RegisterWithUsernameAndPassword(string username, string password, string email)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Username = username,
            Password = password,
            Email = email
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Registration successful!");
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError("Registration failed: " + error.GenerateErrorReport());
    }

    // Set Leaderboard
    public void SubmitScore(double score)
    {
        int clampedScore = (int)Math.Clamp(score, int.MinValue, int.MaxValue);
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
        {
            new StatisticUpdate { StatisticName = "PlayerScore", Value = clampedScore }
        }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnScoreUpdateSuccess, OnScoreUpdateFailure);
    }

    public void UpdateSubmitScore(double score)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
        {
            { "PlayerScore", score.ToString("F2") } // Store as a string with two decimal places
        }
        };

        PlayFabClientAPI.UpdateUserData(request, OnDataUpdateSuccess, OnDataUpdateFailure);
    }

    private void OnDataUpdateFailure(PlayFabError obj)
    {
        Debug.LogError("Failed to update score: " + obj.ErrorMessage);
    }

    private void OnDataUpdateSuccess(UpdateUserDataResult obj)
    {
        Debug.Log("Score updated successfully!");
    }

    void OnScoreUpdateSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Score updated successfully!");
    }

    void OnScoreUpdateFailure(PlayFabError error)
    {
        Debug.LogError("Failed to update score: " + error.ErrorMessage);
    }

    // Get Leaderboard....
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlayerScore",
            StartPosition = 0,
            MaxResultsCount = 10 // Adjust to fetch more or fewer entries
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardSuccess, OnLeaderboardFailure);
    }

    void OnLeaderboardSuccess(GetLeaderboardResult result)
    {
        Debug.Log("Leaderboard fetched successfully!");
        MainUIController.instance.DeleteLeaderBoard();
        foreach (var entry in result.Leaderboard)
        {
            Debug.Log($"Rank: {entry.Position}, Player: {entry.DisplayName ?? entry.PlayFabId}, Score: {entry.StatValue}");
            GameObject card = Instantiate(MainUIController.instance.ScoreCardPrefab, MainUIController.instance.Content);
            card.GetComponent<CardInfo>().rank.text = (entry.Position + 1).ToString();
            card.GetComponent<CardInfo>().playerName.text = entry.DisplayName ?? entry.PlayFabId;
            card.GetComponent<CardInfo>().playerScore.text = entry.StatValue.ToString();
            MainUIController.instance.LeaderboardCards.Add(card);
        }
    }

    void OnLeaderboardFailure(PlayFabError error)
    {
        Debug.LogError("Failed to fetch leaderboard: " + error.ErrorMessage);
    }
}
