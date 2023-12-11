using System.Collections;
using Kickstarter.Categorization;
using UnityEngine;

[RequireComponent(typeof(HeadPair))]
public class RangedAttack : Attack
{
    [SerializeField] private CategoryType targetCategory;

    private Coroutine firingRoutine;

    protected override IEnumerator FireBurst()
    {
        if (weapon is not RangedWeapon rangedWeapon)
        {
            Debug.LogWarning("Weapon Type is Invalid");
            yield break;
        }
        for (int i = 0; i < weapon.BurstAmount; i++)
        {
            var offset = transform.right * rangedWeapon.BulletOffset.x + Vector3.up * rangedWeapon.BulletOffset.y + transform.forward * rangedWeapon.BulletOffset.z;
            var bulletObject = Instantiate(rangedWeapon.BulletPrefab, transform.position + offset, transform.rotation);
            bulletObject.TryGetComponent(out Bullet bullet);
            bullet.SourceBody = gameObject;
            bullet.SourceHead = gameObject.GetComponent<HeadPair>().Head;
            bullet.TargetCategory = targetCategory;
            NotifyObservers(true);
            yield return new WaitForSeconds(1 / AttackRate);
        }
    }
}
