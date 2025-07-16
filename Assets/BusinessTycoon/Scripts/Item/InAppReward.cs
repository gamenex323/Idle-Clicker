using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAppReward : MonoBehaviour
{
    public void BuyAds()
    {
        AdsManager.Instance.OnRemoveADBought();
    }
    public void StarterPack()
    {
        PlayerPrefs.SetInt("StarterPack", 1);
        BuyAds();
        AddBalance();
    }
    public void AddBalance()
    {
        MainUIController.instance.gameManager.LevelData.Balance += 100000;
    }
}
