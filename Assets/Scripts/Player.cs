using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject EmotionObject;
    [SerializeField] private GameObject _vfxHealing;

    public static Player Init { get; private set; } = null;
    public static bool isDead { get; private set; } = false;

    private bool _isImmortal = false;
    private float _damageForBombling => 5 + PlayerStats.Level * 3;
    private PlayerSkin _playerDamageEffect;

    private void Start()
    {
        if (Init == null) Init = this;
        else Destroy(gameObject);

        isDead = false;
        _isImmortal = false;
        StartCoroutine(RegenHealth());
    }

    public void PlayEmotionAnimation()
    {
        EmotionObject.SetActive(true);
    }

    public void PlayVFXHealing()
    {
        _vfxHealing.SetActive(true);
    }
    public void EndVFXHealing()
    {
        _vfxHealing.SetActive(false);
    }
    public void SetCurrentDamgeEffect(PlayerSkin effect) => _playerDamageEffect = effect;

    public void GetDamage(float damage)
    {
        if(isDead || _isImmortal) return;
        PlayerStats.GetDamage(damage);

        _playerDamageEffect.DamageEffect();
        HealthCheck();
    }

    private void HealthCheck()
    {
        if (PlayerStats.Hp <= 0)
        {
            isDead = true;
            UIDataPlayer.Instance.OpenWindowDead();
        }
    }

    public void ReSpawnPlayer()
    {
        isDead = false;
        PlayerStats.FullHP();
        StartCoroutine(Immortal());

        StopCoroutine(RegenHealth());
        StartCoroutine(RegenHealth());
    }

    private IEnumerator Immortal()
    {
        _isImmortal = true;
        yield return new WaitForSeconds(5f);
        _isImmortal = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "explosionDamagePlayer")
        {
            other.enabled = false;
            GetDamage(_damageForBombling);
        }
    }

    private IEnumerator RegenHealth()
    {
        while (!isDead)
        {
            PlayerStats.regenHp();
            yield return new WaitForSeconds(PlayerStats.TickRegen);
        }
    }

    private void OnDestroy()
    {
        Init = null;
    }
}
