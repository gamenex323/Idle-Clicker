using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftUI : MonoBehaviour
{
    public static DailyGiftUI instance;

    [SerializeField]
    private Image giftImage;

    [SerializeField]
    private Text body;

    private RectTransform rectTransform;

    private RewardBusiness[] rewards;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        rewards = new RewardBusiness[11];
        rewards[0] = RewardBusiness.RewardGold(1);
        rewards[1] = RewardBusiness.RewardGold(2);
        rewards[2] = RewardBusiness.RewardGold(3);
        rewards[3] = RewardBusiness.RewardGold(4);
        rewards[4] = RewardBusiness.RewardGold(5);
        rewards[5] = RewardBusiness.RewardGold(6);
        rewards[6] = RewardBusiness.RewardGold(7);
        rewards[7] = RewardBusiness.RewardGold(8);
        rewards[8] = RewardBusiness.RewardGold(9);
        rewards[9] = RewardBusiness.RewardGold(10);
        rewards[10] = RewardBusiness.RewardGold(15);
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.Hide();
    }

    public void Show()
    {
        if (instance != null)
        {
            if ((DateTime.FromOADate(GameManager.instance.GameState.DailyGift) - DateTime.Today.AddHours(-24)).TotalSeconds <= 0)
            {
                GameManager.instance.GameState.DailyGiftCount = 0;
            }

            var reward = rewards[GameManager.instance.GameState.DailyGiftCount];
            reward.Title = "Daily gift";

            giftImage.sprite = GameManager.instance.FindItemSpriteSheetIcon(reward.Type);
            instance.body.text = string.Format(reward.Description, reward.Count);
            rectTransform.Show();
        }
    }

    public void OnCollectClicked()
    {
        var reward = rewards[GameManager.instance.GameState.DailyGiftCount];
        reward.Title = "Daily gift";
        GameManager.instance.GameState.DailyGift = DateTime.Today.AddHours(24).ToOADate();
        GameManager.instance.GameState.DailyGiftCount++;
        if (GameManager.instance.GameState.DailyGiftCount >= rewards.Length)
        {
            GameManager.instance.GameState.DailyGiftCount -= 3;
        }
        RewardUI.instance.Show(reward);
        OnCloseClick();
    }

    public void OnCloseClick()
    {
        rectTransform.Hide();
    }
}