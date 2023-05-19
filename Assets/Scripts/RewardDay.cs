using System;
using System.Threading;
using UnityEngine;

public class RewardDay : MonoBehaviour
{
    [SerializeField] private GameObject _panelReward;

    private const string c_tagSaveTime = "last_time_get_reward";

    public void CheckReward()
    {
        if (PlayerPrefs.HasKey(c_tagSaveTime))
        {
            DateTime current = CheckGlobalTime();
            DateTime lastTimeReward = DateTime.Parse(PlayerPrefs.GetString(c_tagSaveTime));

            TimeSpan difference = current.Subtract(lastTimeReward);

            Debug.LogWarning(difference);
            Debug.LogWarning(current);
            Debug.LogWarning(lastTimeReward);

            if (difference.Days > 0)
            {
                ShowClaimReward();
            }
        }
        else
        {
            //if first play
            ShowClaimReward();
        }
    }

    private void ShowClaimReward() { while (!CheckPanelActiv()) _panelReward.SetActive(true); }
    private bool CheckPanelActiv() => _panelReward.activeSelf;

    public void GetReward()
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        PlayerPrefs.SetInt("Coins", coins + 199);
        SaveTimeLastReward();
    }

    private void SaveTimeLastReward()
    {
        DateTime dateTime = CheckGlobalTime();
        PlayerPrefs.SetString(c_tagSaveTime, dateTime.ToString());
    }

    DateTime CheckGlobalTime()
    {
        var www = new WWW("https://google.com");
        while (!www.isDone && www.error == null)
            Thread.Sleep(1);

        var str = www.responseHeaders["Date"];
        DateTime dateTime;

        if (!DateTime.TryParse(str, out dateTime))
            return DateTime.MinValue;

        return dateTime.ToUniversalTime();
    }

    public void DebugChageDate()
    {
        string oldDate = "Jan 17, 2023";
        PlayerPrefs.SetString(c_tagSaveTime, oldDate);
    }
}
