using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon SO", order = 1)]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private string weaponName;
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int ammoMax;
    [SerializeField] private int ammoCurrent;

    public int AmmoCurrent
    {
        get
        {
            return ammoCurrent;
        }

        set
        {
            if(value >= 0 )
            {
                ammoCurrent = value;
            }
        }
    }

    public string WeaponName
    {
        get
        {
            return weaponName;
        }
    }
    public float FireRate
    {
        get
        {
            return fireRate;
        }
    }
    public float ReloadTime
    {
        get
        {
            return reloadTime;
        }
    }
    public GameObject ProjectilePrefab
    {
        get
        {
            return projectilePrefab;
        }
    }
    public int AmmoMax
    {
        get
        {
            return ammoMax;
        }
    }
}