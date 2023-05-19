using TMPro;
using UnityEngine;

public class Product : MonoBehaviour
{
    [SerializeField] private uint _id = 0;
    [SerializeField] private int _price = 100;
    [SerializeField] private ShopUI _shop;
    [SerializeField] private TMP_Text _textCountBouth;
    [SerializeField] private TMP_Text _textPrice;
    [SerializeField] private GameObject _selectButton = null;

    private bool IsHasCoins => GetCoins >= _price;
    private bool IsAlreadyBought => (_id > 10) ? GetCountBouth >= 1 : false;
    private int GetCountBouth => PlayerPrefs.GetInt("Product-" + _id.ToString(), 0);
    private int GetCoins => PlayerPrefs.GetInt("Coins", 0);
    private int GetRemainder => GetCoins - _price;
    private string _textCount = "”же в наличии";

    private void Awake()
    {
        LanguageDB.UpdateLanguageEvent += UpdateText;
    }
    private void Start()
    {
        ShowBouthCount();
        ShowPrice();

        if (_selectButton != null) ShowButtonForSelect();
    }

    public void UpdateText()
    {
        _textCount = LanguageDB.GetTranslate(_textCount);
        ShowBouthCount();
    }
    public void Buy()
    {
        if (IsAlreadyBought) ExceptionBuy(CodeBuyType.AlreadyBought);
        else if (IsHasCoins) BuyProduct();
        else ExceptionBuy(CodeBuyType.NotEnoughMoney);
    }
    public void SetSkin(int id) {
        PlayerPrefs.SetInt("PlayerSkin", id);
        SkinsMenu.instance.SetSkin();
    }

    private void ExceptionBuy(CodeBuyType code)
    {
        _shop.BuyCode(code);
    }
    private void BuyProduct()
    {
        SetBougth();
        PlayerPrefs.SetInt("Coins", GetRemainder);
        _shop.BuyCode(CodeBuyType.Successfully);
        if (_selectButton != null) ShowButtonForSelect();
    }
    private void SetBougth() {
        PlayerPrefs.SetInt("Product-" + _id.ToString(), GetCountBouth + 1);
        ShowBouthCount();
    }
    private void ShowBouthCount() =>  _textCountBouth.text = _textCount + ": " + GetCountBouth.ToString();
    private void ShowPrice() => _textPrice.text = _price.ToString();
    private void ShowButtonForSelect()
    {
        if (IsAlreadyBought) _selectButton.SetActive(true);
        else _selectButton.SetActive(false);
    }
}
