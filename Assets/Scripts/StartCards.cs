using TMPro;
using UnityEngine;

public class StartCards : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _textsCount;
    [SerializeField] private GameObject[] _chooseObjView;

    [SerializeField] private GameObject _mainWindow;

    private bool[] _cards = new bool[_maxCountStartCards];
    private int[] _cardCounts = new int[_maxCountStartCards];

    private const int _maxCountStartCards = 5;

    private void Start()
    {
        Time.timeScale = 0f;

        for(int i = 0; i < _maxCountStartCards; i++)
        {
            _cardCounts[i] = GetCountBouth(i);
            _textsCount[i].text = _cardCounts[i].ToString();
        }
    }

    public void ChooseCard(int id)
    {
        if (_cardCounts[id] > 0)
        {
            _cards[id] = !_cards[id];
            _chooseObjView[id].SetActive(_cards[id]);
        }
    }

    private int GetCountBouth(int id) => PlayerPrefs.GetInt("Product-" + id.ToString(), 0);
    private void SetCountBouth(int id) => PlayerPrefs.SetInt("Product-" + id.ToString(), (_cards[id] ? _cardCounts[id] -1 : _cardCounts[id]));

    public void StartGame()
    {
        PlayerStats.Initialization(_cards);

        for (int i = 0; i < _maxCountStartCards; i++) SetCountBouth(i);

        _mainWindow.SetActive(false);
        Time.timeScale = 1f;
    }
}
