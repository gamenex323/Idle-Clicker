using System;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeBackUI : MonoBehaviour
{
    public Text offlineText;
    public Text earningText;
    public Text doubleText;

    private double totalOfflineCash;

    public void SetUp(double idleTime, double offlineCash)
    {
        totalOfflineCash = offlineCash;
        var idleTimeConverted = Convert.ToInt32(idleTime);

        var cash = NumberUtility.Convert(offlineCash, 1000000.0, true);
        var cashX2 = NumberUtility.Convert(offlineCash * 2, 1000000.0, true);

        offlineText.text = "You were offline for\n<b>" + TimeOffline(idleTimeConverted) + "</b>.";
        earningText.text = "Your managers worked hard and earned\n<b><color=#192cd6>" + cash + "</color></b>\n for you while your were away.";
        doubleText.text = "Watch an ad to double your earnings\n<b><color=#188126>" + cashX2 + "</color></b>";
    }

    private string TimeOffline(int timeOffline)
    {
        var text = "{0} {1} and {2} {3}";
        var hours = timeOffline / 3600;
        var minutes = timeOffline / 60 % 60;
        return string.Format(text, hours, (hours > 1 ? "hours" : "hour"), minutes, (minutes > 1 ? "minutes" : "minute"));
    }

    public void OnEarningClick()
    {
        // Offline cash already added, just show message.
        ToastUI.instance.FlashToast("Offline earnings", NumberUtility.Convert(totalOfflineCash, 1000000.0, true), 5f);
        MainUIController.instance.OnClickWelcomeBackHide();
    }
}
