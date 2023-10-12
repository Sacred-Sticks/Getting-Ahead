using System.Collections;
using UnityEngine;
/* This is an example weapon (a bow or ammo based ranged weapon)
The weapon is a weapon and is reloadable but is not magic, thus the implementations.
Each weapon will need its own logic for attacking and reloading and such.

*/
public class RangedWeapon : MonoBehaviour, IWeapon, IReloadable
{
    [SerializeField]
    private WeaponSO weapon;
    [SerializeField]
    private GameObject attackPoint;
    private bool isReloading = false;

    void FixedUpdate()
    {
        Attack();
    }
    
    public void Attack()
    {
        if(weapon.AmmoCurrent > 0 && !isReloading)
        {
            //Need to parent the instance if it is melee (or not?)
            Instantiate(weapon.ProjectilePrefab, attackPoint.transform.position, attackPoint.transform.rotation);
            weapon.AmmoCurrent--;
        }
        else
        {
            if(!isReloading)
            {
                Reload();
                isReloading = true;
            }
        }
    }

    public void Reload()
    {
        StartCoroutine(ReloadWait());
        //Check current ammo value (if weapon isn't single shot) to make sure reloading is viable
        //Start reloading coroutine and stop attacks until finished. Set ammo to max or available.
    }

    private IEnumerator ReloadWait()
    {
        yield return new WaitForSeconds(weapon.ReloadTime);
        weapon.AmmoCurrent = weapon.AmmoMax;
        isReloading = false;
    }


}
