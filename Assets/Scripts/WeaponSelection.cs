using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class WeaponSelection : MonoBehaviour
{
    public byte idWeapon = 0;
    public bool isRandomWeapon = false;
    public Transform preview;
    public GameObject[] weapons;

    private void Start()
    {
        if (isRandomWeapon)
        {
            byte id = (byte)Random.Range(0, weapons.Length);
            idWeapon = id;
        }
        weapons[idWeapon].SetActive(true);
    }

    private void Update()
    {
        preview.Rotate(0,Time.deltaTime*100,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Weapons.Init.SwitchWeapon(idWeapon);
        }
    }
}
