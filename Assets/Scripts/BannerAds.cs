using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAds : MonoBehaviour
{
    private BannerView bannerView;

    [SerializeField] private RectTransform PlayerDataPanel;
    [SerializeField] private GameObject ContinueButton;

    private const string _adKey = "ca-app-pub-7765717132622752/1464671700";
    private bool _isBoughtAdvertising = false;

    private void Start()
    {
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetSameAppKeyEnabled(true).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        if (PlayerPrefs.GetInt("isBoughtAdvertising", 0) == 1)
            HideFieldForBannerAd();

        RequestBanner();
    }

    private void RequestBanner()
    {
        if (_isBoughtAdvertising) return;

        bannerView = new BannerView(_adKey, AdSize.Banner, AdPosition.Top);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);

        bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;
    }

    private void HideFieldForBannerAd()
    {
        PlayerDataPanel.offsetMax = new Vector2(PlayerDataPanel.offsetMax.x, 0f);
        ContinueButton.SetActive(false);
        _isBoughtAdvertising = true;
        
    }

    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        HideFieldForBannerAd();
    }
}
