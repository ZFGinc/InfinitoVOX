using TMPro;
using UnityEngine;

public enum CodeBuyType: byte
{
    Successfully = 0,
    NotEnoughMoney,
    AlreadyBought,
    Other
}

public class ShopUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _countCoins;
    [SerializeField] private GameObject[] _alertPanels;
    [SerializeField] private GameObject _getCoinsPanel;

    public static ShopUI Instance { get; private set; } = null;

    private int GetCoins => PlayerPrefs.GetInt("Coins", 0);

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void BuyCode(CodeBuyType code)
    {
        _alertPanels[(int)code].SetActive(true);
        ShowCurrentCoins();
    }

    public void HideAllAlertPAnels()
    {
        foreach (GameObject obj in _alertPanels) obj.SetActive(false);
    }

    public void DebugAddCoins()
    {
        PlayerPrefs.SetInt("Coins", 10000);
        ShowCurrentCoins();
    }

    public void ShowAlertOfGetCoinsAds()
    {
        _getCoinsPanel.SetActive(true);
        ShowCurrentCoins();
    }

    public void ShowCurrentCoins() => _countCoins.text = GetCoins.ToString();
}
