using Boosts;
using System.Collections;
using UnityEngine;

public sealed class BoostApplication: MonoBehaviour
{
    private IBoost _boost;
    private bool _isTempBoosting = false;

    public static BoostApplication Instance { get; private set; }

    public static bool IsTempBoosting => Instance._isTempBoosting;

    private void Start() 
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetConstantBoost(ConstantBoosts boost)
    {
        switch (boost)
        {
            case ConstantBoosts.AddMaxHP: PlayerStats.upMaxHp(50); break;
            case ConstantBoosts.PutCDRegenHP: PlayerStats.upRegenTick(); break;
            case ConstantBoosts.AddMoveSpeed: PlayerStats.Speed += .2f; break;
            case ConstantBoosts.AddDamage: PlayerStats.BaseDamage += 5f; break;
            case ConstantBoosts.PutCDShooting: PlayerStats.BaseCoolDownForShooting -= 0.05f; break;
            case ConstantBoosts.NewWeapon: Weapons.Init.UpgradeWeapon(); break;
            default: break;
        }
    }
    public void SetTemporaryBoosts(IBoost boost)
    {
        _boost = boost;
        StartCoroutine(TempBoost());
    }

    private IEnumerator TempBoost()
    {
        _isTempBoosting = true;

        _boost.StartBoosting();
        BoostUi.Instance.Timer(_boost.GetUseTime());

        yield return new WaitForSeconds(_boost.GetUseTime());

        _boost.EndBoosting();
        BoostUi.Instance.HideUiBoost();

        _isTempBoosting = false;

    }



    private void OnDestroy()
    {
        Instance = null;
    }
}

public enum ConstantBoosts : int
{
    None = -1,
    AddMaxHP = 0,
    PutCDRegenHP = 1,
    AddMoveSpeed = 2,
    AddDamage = 3,
    PutCDShooting = 4,
    NewWeapon = 5

}
public enum TemporaryBoosts : int
{
    None = -1,
    Random = 0,
    AddMoveSpeed = 1,
    AddDamage = 2,
    PutCDShooting = 3,
    RegenHp = 4
}