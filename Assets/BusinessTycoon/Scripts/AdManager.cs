using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager instance = null;

    public string GameId_Android = "5546862";
    public string GameId_iOS = "5546863";

    public string RewardedId_Android = "Rewarded_Android";
    public string RewardedId_iOS = "Rewarded_iOS";

    public bool TestMode = false;

    private string gameId;
    private string rewardedId;
    private static string rewardType;
    private bool isReady = false;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        Initialize();
    }

    public void Initialize()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            gameId = GameId_iOS;
            rewardedId = RewardedId_iOS;
        }
        else
        {
            gameId = GameId_Android;
            rewardedId = RewardedId_Android;
        }

        Advertisement.Initialize(gameId, TestMode, this);
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(rewardedId, this);
    }

    #region Interface Implementations
    public void OnInitializationComplete()
    {
        Debug.Log("Init Success");
        LoadRewardedAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Init Failed: [{error}]: {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        isReady = true;
        Debug.Log($"Load Success: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        isReady = false;
        Debug.Log($"Load Failed: [{error}:{placementId}] {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        MessageBoxUI.instance.Show("Error showing ad", "No internet connection available or there was a problem on the server.\r\nTry again later.", null, MessageBoxUI.ButtonOptions.OK, "OK");
        LoadRewardedAd();
        Debug.Log($"OnUnityAdsShowFailure: [{error}]: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"OnUnityAdsShowStart: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"OnUnityAdsShowClick: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        AddRewardToPlayer();
        LoadRewardedAd();
        Debug.Log($"OnUnityAdsShowComplete: [{showCompletionState}]: {placementId}");
    }
    #endregion

    public bool UnityAdReady()
    {
        return isReady;
    }

    public void ShowAd(string reward)
    {
        rewardType = reward;
        if (isReady)
        {
            MainUIController.instance.ShowLoadingOverlay(1.5f);
            Advertisement.Show(rewardedId, this);
        }
    }

    private void AddRewardToPlayer()
    {
        // Video completed - Offer a reward to the player
        switch (rewardType)
        {
            case "OfflineEarning":
                MainUIController.instance.DoubleOfflineEarning();
                break;
            case "DoubleGift":
                FreeGiftUI.instance.DoubleReward();
                break;
            case "DoubleProfit":
                ProfitBoostUI.instance.ExtendBoostTime();
                break;
        }
    }
}
