using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoostUi : MonoBehaviour
{
    [SerializeField] private Animator _animatorUiBoost;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _timerBackside;
    [SerializeField] private Sprite[] _iconsBoosts;
    private bool isShow = false;

    public static BoostUi Instance { get; private set; }

    private void Start()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }

    public void ShowUiBoost(int idBoost)
    {
        if (isShow) return;
        isShow = true;

        SetIconBoost(idBoost);
        SetAnimation();
    }

    public void HideUiBoost()
    {
        if (!isShow) return;
        isShow = false;

        SetAnimation();
    }

    public void Timer(float time) => StartCoroutine(TimerBoost(time));

    private void SetIconBoost(int idBoost) => _icon.sprite = _iconsBoosts[idBoost];

    private void SetAnimation() => _animatorUiBoost.SetBool("isShow", isShow);

    private IEnumerator TimerBoost(float time)
    {
        _timerBackside.fillAmount = 0f;

        for(float i = 0; i < 1; i+= 0.05f)
        {
            _timerBackside.fillAmount = i;
            yield return new WaitForSeconds(time/20);
        }

        _timerBackside.fillAmount = 1f;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
