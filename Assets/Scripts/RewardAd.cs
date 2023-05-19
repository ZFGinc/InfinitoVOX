using GoogleMobileAds.Api;
using System;
using UnityEngine;

public enum TypeReward
{
    RespawnAndContinue,
    Coins
}

public class RewardAd : MonoBehaviour
{
    private RewardedAd rewardedAd;

    [SerializeField] private TypeReward _typeReward;
    [SerializeField] private GameObject _buttonContinue;
    [SerializeField] private GameObject _windowDead;
    [SerializeField] private GameObject _joyStick;

    private string _adKey = "ca-app-pub-7765717132622752/2810361204";
    private string _adKeyCoins = "ca-app-pub-7765717132622752/2483078597";

    void Start()
    {
        if (_typeReward == TypeReward.RespawnAndContinue)
            rewardedAd = new RewardedAd(_adKey);
        else
            rewardedAd = new RewardedAd(_adKeyCoins);

        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);

        rewardedAd.OnAdClosed += HandleRewardedAdCloseed;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedRewards;
    }

    public void ShowRewardBasedVideoAds()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();  
        }
    }

    public void HandleRewardedAdCloseed(object sender, EventArgs args)
    {
        if (_typeReward == TypeReward.RespawnAndContinue)
        {
            _buttonContinue.SetActive(true);
            _windowDead.SetActive(true);
            _joyStick.SetActive(false);
        }
    }

    public void HandleUserEarnedRewards(object sender, Reward args)
    {
        if (_typeReward == TypeReward.RespawnAndContinue)
        {
            _buttonContinue.SetActive(false);
            _windowDead.SetActive(false);
            _joyStick.SetActive(true);
            Player.Init.ReSpawnPlayer();

            if (!AudioController.IsMuteSfx)
                AudioController.instance.SetPreviousSfx();
        }
        else
        {
            int coins = PlayerPrefs.GetInt("Coins", 0);
            PlayerPrefs.SetInt("Coins", coins + 250);
            ShopUI.Instance.ShowAlertOfGetCoinsAds();
        }
    }
}
