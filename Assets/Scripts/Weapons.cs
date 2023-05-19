using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [Header("Текщий номер оружия")]
    public byte currentIdWeapon = 0;

    //Пока что публичные, потом нужно скрыть!!!
    [Space(20)]
    [SerializeField] private GameObject currentWeaponObject;
    [SerializeField] private GameObject currentBulletPrefab;
    [SerializeField] private StatsWeapon currentWeaponStats;
    [SerializeField] private GameObject _effectShooting;

    ///////////////////////////////////////////
    [Space(20)]
    [Header("Объекты оружий на сцене")]
    [SerializeField] private GameObject[] WeaponsObjects;
    [Header("Префабы патрон оружий")]
    [SerializeField] private GameObject[] BulletsPrefabs;

    //Приватные переменные, которые не должны быть видны в Unity
    private List<StatsWeapon> BaseStatsWeapons;
    private Transform muzzle;

    public static Weapons Init;
    [HideInInspector] public bool isShoot;

    private void Start()
    {
        Init = this;
        isShoot = false;
        LoadStatsWeapons();
        SetWeapon(0);

        StartCoroutine(Shooting());
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            if (isShoot) 
            {
                Shoot();
            }
            yield return new WaitForSeconds(currentWeaponStats.CoolDown + PlayerStats.BaseCoolDownForShooting);
        }
    }

    private void SetWeapon(byte idWeapon)
    {
        currentWeaponObject = WeaponsObjects[idWeapon];
        currentWeaponObject.SetActive(true);

        muzzle = currentWeaponObject.transform.GetChild(1);

        currentBulletPrefab = BulletsPrefabs[idWeapon];
        currentWeaponStats = BaseStatsWeapons[idWeapon];
        currentIdWeapon = idWeapon;
    }
    public void SwitchWeapon(int idWeapon)
    {
        if (currentIdWeapon == (byte)idWeapon) return;

        currentWeaponObject.SetActive(false);
        SetWeapon((byte)idWeapon);
    }
    public void UpgradeWeapon()
    {
        if (currentIdWeapon == WeaponsObjects.Length) return;
        SwitchWeapon(currentIdWeapon + 1);
    }

    public void Shoot()
    {
        Instantiate(_effectShooting, muzzle.position, muzzle.rotation);
        var bullet = Instantiate(currentBulletPrefab, muzzle.position, muzzle.rotation);
        if (bullet.GetComponent<Bullet>()) bullet.GetComponent<Bullet>().Damage = currentWeaponStats.Damage;
        else bullet.GetComponent<Bomb>().Damage = currentWeaponStats.Damage;
    }

    private void LoadStatsWeapons()
    {
        string[] texts;
        BaseStatsWeapons = new List<StatsWeapon>();

        TextAsset mytxtData = (TextAsset)Resources.Load("GunTable");
        texts = mytxtData.text.Split('\n');

        for (int i = 1; i < texts.Length; i++)
        {
            string[] cuted_row = texts[i].Split(",");
            if(cuted_row.Length < 5) continue;

            StatsWeapon stats = new StatsWeapon();

            MyTryParse(cuted_row[3], out stats.Damage);
            MyTryParse(cuted_row[4], out stats.CoolDown);
            MyTryParse(cuted_row[5], out stats.Heaviness);

            BaseStatsWeapons.Add(stats);
        }
    }

    private void MyTryParse(string s, out float result)
    {
        if(s.Contains('.'))
        {
            var t = s.ToCharArray();
            t[s.IndexOf('.')] = ',';
            s = new string(t);
        }
        float.TryParse(s, out result);
    }

    private void OnDestroy()
    {
        Init = null;
    }
}

public class StatsWeapon
{
    public float Damage = 10f;
    public float CoolDown = 1f;
    public float Heaviness = 1f;
}
