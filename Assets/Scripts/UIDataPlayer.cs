using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDataPlayer : MonoBehaviour
{
    [Header("Во время игры")]
    [SerializeField] private TMP_Text LevelText;
    [SerializeField] private TMP_Text CoinsText;
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private Slider ExperiansSlider;
    [SerializeField] private GameObject LvlUpWindow;
    [SerializeField] private GameObject Joystick;
    [SerializeField] private AudioSource _chooseCard;
    [Space]

    [Header("Карты с прокачкой")]
    public Sprite[] cardImages;
    [SerializeField] private Card[] CardObjects;
    [Space]

    [Header("Для окна проигрыша | победы")]
    public GameObject WindowDead;
    public GameObject[] objectsForWindowDead;
    public AudioController audioController;
    public bool IsBlockMove { get; private set; } = false;

    public static UIDataPlayer Instance { get; private set; } = null;

    private Image _fon;
    private float _oldSpeed;

    private const float _tick = .001f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _fon = LvlUpWindow.GetComponent<Image>();

        UpdateCoinsText();
    }

    private void ShowCards()
    {
        foreach (Card card in CardObjects)
        {
            card.gameObject.SetActive(true);
            card.ShowCard();
        }
    }
    private void HideCards()
    {
        foreach (Card card in CardObjects)
            card.HideCard();
    }

    private IEnumerator FadeCloseWindowWithCards()
    {
        _fon.color = new Color(0, 0, 0, 0.48f);

        HideCards();

        for (int i = 0; i < 50; i++)
        {
            Time.timeScale += .01f;
            _fon.color = new Color(0, 0, 0, (10 - i) * 0.0096f);
            yield return new WaitForSecondsRealtime(_tick);
        }
        Time.timeScale = 1;
        LvlUpWindow.SetActive(false);
    }
    private IEnumerator FadeOpenWindowWithCards()
    {
        _fon.color = new Color(0, 0, 0, 0);
        LvlUpWindow.SetActive(true);
        _chooseCard.Play();

        for (int i = 0; i < 50; i++)
        {
            Time.timeScale -= .01f;
            _fon.color = new Color(0, 0, 0, (i + 1) * 0.0096f);
            yield return new WaitForSecondsRealtime(_tick);
        }
        ShowCards();
        Time.timeScale = 0.0001f;

        if (!AudioController.IsMuteSfx)
            audioController.AttenuationOfSfx();
    }
    private IEnumerator FadeOpenWindowDead()
    {
        foreach (GameObject obj in objectsForWindowDead)
            obj.SetActive(false);

        Image backgr = WindowDead.GetComponent<Image>();
        backgr.color = new Color(.35f, 0, 0, 0);

        WindowDead.SetActive(true);

        for (int i = 0; i < 50; i++)
        {
            Time.timeScale -= .01f;
            backgr.color = new Color(.35f, 0, 0, (i + 1) * 0.0096f);
            yield return new WaitForSecondsRealtime(_tick);
        }
        Time.timeScale = 0.1f;

        foreach (GameObject obj in objectsForWindowDead)
        {
            obj.SetActive(true);
            yield return new WaitForSecondsRealtime(.02f);
        }

        Time.timeScale = 0f;
    }

    public void UpdateHealthSlider(float value) => HealthSlider.value = value;
    public void UpdateMaxHealthSlider(float value) => HealthSlider.maxValue = value;
    public void UpdateExperiansSlider(float value) => ExperiansSlider.value = value; 
    public void UpdateMaxExperiansSlider(float value) => ExperiansSlider.maxValue = value;
    public void UpdateCurrentLevelText(uint level) => LevelText.text = level.ToString();
    public void UpdateCoinsText() => CoinsText.text = PlayerPrefs.GetInt("Coins", 0).ToString();

    public void ShowLvlupWindow()
    {
        if (Player.isDead) return;
        Weapons.Init.isShoot = false;
        Joystick.SetActive(false);
        StartCoroutine(FadeOpenWindowWithCards());
    }

    public void CloseLvlUpWindow()
    {
        Weapons.Init.isShoot = true;
        Joystick.SetActive(true);
        StartCoroutine(FadeCloseWindowWithCards());
        PlayerStats.ResetExp();

        if (!AudioController.IsMuteSfx)
            audioController.SetPreviousSfx();
    }

    public void OpenWindowDead()
    {
        Weapons.Init.isShoot = false;
        Joystick.SetActive(false);
        StartCoroutine(FadeOpenWindowDead());

        if (!AudioController.IsMuteSfx)
            audioController.AttenuationOfSfx();
    }

    public void BlockMove()
    {
        Joystick.SetActive(false);
        IsBlockMove = true;
        _oldSpeed = PlayerStats.Speed;
        PlayerStats.Speed = 0;
    }
    public void UnblockMove()
    {
        Joystick.SetActive(true);
        IsBlockMove = false;
        PlayerStats.Speed = _oldSpeed;
    }


    private void OnDestroy()
    {
        Instance = null;
    }
}
