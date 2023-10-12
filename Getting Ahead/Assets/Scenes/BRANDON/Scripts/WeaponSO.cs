using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon SO", order = 1)]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public float fireRate;
    public float reloadTime;
    public GameObject projectilePrefab;
    public int ammoMax;
    public int ammoCurrent;

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
    
}