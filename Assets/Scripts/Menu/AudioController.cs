using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private bool _isStartLoadData = true;
    [Space]
    [SerializeField] private AudioMixer _mainMixer;
    [Space]
    [SerializeField] private Sprite[] _spritesForFxButton;
    [SerializeField] private Sprite[] _spritesForMusicButton;
    [Space]
    [SerializeField] private Image _imageFxButton;
    [SerializeField] private Image _imageMusicButton;
    

    private bool _isMuteFx = false;
    private bool _isMuteMusic = false;
    private float _previousValue;

    private const int _maxValume = 0;
    private const int _minValume = -80;
    private const string _tagSfx = "SFX";
    private const string _tagMusic = "Music";

    public static AudioController instance = null;

    public static bool IsMuteSfx => instance._isMuteFx;
    public static bool IsMuteMusic => instance._isMuteMusic;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        LoadSettingsAudio();

        if (_isStartLoadData)
            UpdateUIButtons();
    }

    public void SwitchFxValume()
    {
        if (_isMuteFx) _mainMixer.SetFloat(_tagSfx, _maxValume);
        else _mainMixer.SetFloat(_tagSfx, _minValume);

        _isMuteFx = !_isMuteFx;
        UpdateUIButtons();
    }

    public void SwitchSoundsValume()
    {
        if (_isMuteMusic) _mainMixer.SetFloat(_tagMusic, _maxValume);
        else _mainMixer.SetFloat(_tagMusic, _minValume);

        _isMuteMusic = !_isMuteMusic;
        UpdateUIButtons();
    }

    private void UpdateUIButtons()
    {
        _imageFxButton.sprite = _spritesForFxButton[_isMuteFx ? 0 : 1];
        _imageMusicButton.sprite = _spritesForMusicButton[_isMuteMusic ? 0 : 1];

        SaveSettingsAudio();
    }

    private void SaveSettingsAudio()
    {
        PlayerPrefs.SetInt("ValumeSFX", _isMuteFx ? _minValume : _maxValume);
        PlayerPrefs.SetInt("ValumeMusic", _isMuteFx ? _minValume : _maxValume);
    }
    private void LoadSettingsAudio()
    {
        if (PlayerPrefs.GetInt("ValumeSFX", 0) == -80) _isMuteFx = true;
        if (PlayerPrefs.GetInt("ValumeMusic", 0) == -80) _isMuteMusic = true;

        if (_isMuteMusic) _mainMixer.SetFloat(_tagMusic, _minValume);
        else _mainMixer.SetFloat(_tagMusic, _maxValume);

        if (_isMuteFx) _mainMixer.SetFloat(_tagSfx, _minValume);
        else _mainMixer.SetFloat(_tagSfx, _maxValume);
    }

    public void AttenuationOfSfx()
    {
        _previousValue = _isMuteFx ? _minValume : _maxValume;
        _mainMixer.SetFloat(_tagSfx, _minValume);
    }

    public void SetPreviousSfx()
    {
        _mainMixer.SetFloat(_tagSfx, _previousValue);
    }

    public void BoostingSfx()
    {
        _mainMixer.SetFloat(_tagSfx, _maxValume);
    }
}
