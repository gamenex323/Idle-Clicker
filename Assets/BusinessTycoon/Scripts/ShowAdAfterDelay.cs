using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class ShowAdAfterDelay : MonoBehaviour
{
    public float delayInSeconds = 30f;

    void Start()
    {
        InvokeRepeating(nameof(ShowAdAfterTime), delayInSeconds, delayInSeconds);
    }

    void ShowAdAfterTime()
    {
        print("Show Ads After Delay");
        ShowInterstitial();
    }

    public void ShowInterstitial()
    {
        AdsManager.Instance.ShowInterstitial();
    }
}
