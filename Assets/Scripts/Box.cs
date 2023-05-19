using Boosts;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private TemporaryBoosts _boostType = TemporaryBoosts.None;
    [SerializeField] private GameObject _vfxStand;
    [SerializeField] private GameObject _vfxPickUp;

    private readonly IBoost[] _allBoosts = new IBoost[4] 
    {
        new AddMoveSpeedBoost(), new AddDamageBoost(), new PutCDShootingBoost(), new RegenHpBoost()
    };

    private const float _speedRotation = 100f;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        while(_boostType == TemporaryBoosts.Random) _boostType = (TemporaryBoosts)Random.Range(0, _allBoosts.Length);
    }

    private void Update()
    {
        transform.Rotate(0, Time.deltaTime * _speedRotation, 0);
    }

    public void AnimationTrigger()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    private void Pickup()
    {
        _vfxStand.SetActive(false);
        Instantiate(_vfxPickUp, _vfxStand.transform.position, transform.rotation);
    }

    private void OnTriggerEnter (Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            int _index = ((int)_boostType)-1;

            BoostApplication.Instance.SetTemporaryBoosts(_allBoosts[_index]);
            BoostUi.Instance.ShowUiBoost(_index);

            _animator.SetBool("isPickUp", true);
            Pickup();
        }
    }
}
