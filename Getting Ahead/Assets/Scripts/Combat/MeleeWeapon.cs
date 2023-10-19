using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    [SerializeField]
    private WeaponSO weapon;
    [SerializeField]
    private GameObject attackPoint;
    private float nextTimeToFire = 0f;

    void FixedUpdate()
    {
        Attack();
    }
    public void Attack()
    {
        if(CheckFireRate())
        {
            Instantiate(weapon.ProjectilePrefab, new Vector3(attackPoint.transform.position.x, attackPoint.transform.position.y, attackPoint.transform.position.z), attackPoint.transform.rotation);
        }
    }

    private bool CheckFireRate()
    {
        if(Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1f / weapon.FireRate);
            return true;
        }
        return false;
    }
}
