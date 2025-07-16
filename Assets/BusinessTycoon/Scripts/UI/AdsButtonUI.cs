using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AdsButtonUI : MonoBehaviour
{
    Button button;
    Text buttonText;

    public string rewardType;

    void Start()
    {
        button = GetComponent<Button>();
        if (button)
        {
            button.onClick.AddListener(ShowAd);
            buttonText = button.GetComponentInChildren<Text>();
        }
    }

    void Update()
    {
        //var ready = (AdManager.instance != null && AdManager.instance.UnityAdReady());

        //if (button)
        //{
        //    button.interactable = ready;
        //}

        //if (buttonText)
        //{
        //    if (ready)
        //    {
        //        buttonText.color = Color.white;
        //    }
        //    else
        //    {
        //        buttonText.color = GameManager.instance.InactiveButtonTextColor;
        //    }
        //}
    }

    void ShowAd()
    {
        MainUIController.instance.rewarType = rewardType;
        AdsManager.Instance.ShowRewardAd(GiveReward);

    }

    void GiveReward(string callback)
    {
        switch (MainUIController.instance.rewarType)
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