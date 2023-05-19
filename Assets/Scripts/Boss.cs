using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NavMeshAgent))]
public class Boss : MonoBehaviour, EnemyMain
{
    [SerializeField] private bool isImmortal = true;
    [SerializeField] private int _priceToKill = 3;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject _effectTakeDamage;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private bool _immortalInAnimationAttack = false;
    [SerializeField] private float _targetForAttack = 6f;

    private float Health = 20;
    private bool isLive = true;
    private bool _isAttack = false;
    private bool _isTakeDamage = false;

    private AudioSource audioSource;
    private Transform target;
    private NavMeshAgent agent;

    private const int c_countExp = 3;

    void Start()
    {
        animator.SetInteger("state", 1);
        _effectTakeDamage.SetActive(false);

        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        Health = 100 + PlayerStats.Level * 15;

        _healthBar.maxValue = Health;
        _healthBar.value = Health;

        isImmortal = true;
        transform.tag = "Boss";

        StartCoroutine(Attack());
    }

    private void FixedUpdate()
    {
        if (isLive && !_isAttack && !isImmortal)
        {
            agent.destination = target.position;
            animator.SetInteger("state", 1);
        }
    }

    public void GetDamage(float damage)
    {
        if (isImmortal) return;
            
        Health -= damage + PlayerStats.BaseDamage;
        _healthBar.value = Health;

        StartCoroutine(EffectTakeDamage());
        HealthCheck();
    }

    public void DestroyEnemy()
    {
        AltarController altar = GameObject.Find("GameManager").GetComponent<AltarController>();
        altar.EndBossFight();

        Destroy(gameObject);
    }

    public void DisableSounds()
    {
        audioSource.Stop();
    }

    private void HealthCheck()
    {
        if (Health <= 0)
        {
            _healthBar.transform.parent.gameObject.SetActive(false);
            for(int i = 0; i < c_countExp; i++) PlayerStats.upExpOne();
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + _priceToKill * PlayerStats.BaseAdderCoins);
            UIDataPlayer.Instance.UpdateCoinsText();
            isLive = false;
            agent.speed = 0;
            transform.tag = "Untagged";
            animator.SetBool("isDead", true);

            if (Random.Range(0, 3) == 1) Player.Init.PlayEmotionAnimation();
        }
    }
    private IEnumerator EffectTakeDamage()
    {
        if (_isTakeDamage) yield break;

        _isTakeDamage = true;
        _effectTakeDamage.SetActive(true);

        yield return new WaitForSeconds(.7f);

        _effectTakeDamage.SetActive(false);
        _isTakeDamage = false;
    }

    private void OnDestroy()
    {
        StopCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (isLive)
        {
            yield return new WaitForSeconds(_targetForAttack);
            _isAttack = true;

            animator.SetInteger("state", Random.Range(2, 4));

            yield return new WaitForSeconds(1f);

            if (_immortalInAnimationAttack)
            {
                isImmortal = true;

                yield return new WaitForSeconds(1f);
                isImmortal = false;
            }

            _isAttack = false;
        }
    }

    public void EndImmortal()
    {
        isImmortal = false;
    }
}
