using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, EnemyMain
{ 
    [SerializeField] private Animator animator;
    [SerializeField] private bool isImmortal = false;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private GameObject _effectTakeDamage;
    [SerializeField] private int _priceToKill = 3;
    [SerializeField] private Slider _healthBar;

    private float Health = 20;
    private float Damage = 5;
    private bool isLive = true;
    private bool _isAttack = false;
    private bool _isTakeDamage = false;
    private AudioSource audioSource;

    private const float _targetForAttack = 1.3f;

    Transform target;
    NavMeshAgent agent;

    void Start()
    {
        _effectTakeDamage.SetActive(false);

        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        Health = 5 + PlayerStats.Level * 3;
        Damage = 11 + PlayerStats.Level * 2;

        _healthBar.maxValue = Health;
        _healthBar.value = Health;
    }

    private void FixedUpdate()
    {
        if (isLive) agent.destination = target.position;
    }

    private void Update()
    {
        float distance = Vector3.Distance(target.transform.position, agent.transform.position);
        if (distance < _targetForAttack) StartCoroutine(Attack());
    }

    public void GetDamage(float damage)
    {
        if (!isImmortal) Health -= damage + PlayerStats.BaseDamage;

        _healthBar.value = Health;

        StartCoroutine(EffectTakeDamage());
        HealthCheck();
    }

    public void DestroyEnemy()
    {
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
            PlayerStats.upExpOne(); 
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + _priceToKill * PlayerStats.BaseAdderCoins);
            UIDataPlayer.Instance.UpdateCoinsText();
            isLive = false;
            agent.speed = 0;
            transform.tag = "Untagged";
            if (destroyEffect == null) animator.SetBool("isDead", true);
            else
            {
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            if (Random.Range(0, 3) == 1) Player.Init.PlayEmotionAnimation();
        }
    }
    private IEnumerator EffectTakeDamage()
    {
        if(_isTakeDamage) yield break;

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
        if(_isAttack || !isLive) yield break;
        _isAttack = true;

        Player.Init.GetDamage(Damage);

        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        yield return new WaitForSeconds(1f);
        _isAttack = false;  
    }

    public void EndImmortal()
    {

    }
}
