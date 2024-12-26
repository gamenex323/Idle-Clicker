using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    public static LeaderboardUI instance;
    [HideInInspector]
    private RectTransform rectTransform;
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
    }
    // Use this for initialization
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.Hide();
    }

    public void Show()
    {
        instance.rectTransform.Show();
        PlayFabLoginManager.Instance.GetLeaderboard();
    }

    public void Hide()
    {
        instance.rectTransform.Hide();
    }
}
