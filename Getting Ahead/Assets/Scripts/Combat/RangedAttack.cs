using System.Collections;
using Kickstarter.Categorization;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField] private RangedWeapon weapon;
    [SerializeField] private CategoryType targetCategory;

    private Coroutine firingRoutine;

    protected override void StartAttack()
    {
        isAttacking = !isAttacking;
        switch (isAttacking)
        {
            case true:
                firingRoutine = StartCoroutine(Firing());
                break;
        }
    }

    protected override void EndAttack()
    {
        isAttacking = !isAttacking;
        switch (isAttacking)
        {
            case false:
                if (firingRoutine == null)
                    break;
                StopCoroutine(firingRoutine);
                firingRoutine = null;
                break;
        }
    }

    private IEnumerator Firing()
    {
        while (true)
        {
            var burst = StartCoroutine(FireBurst());
            yield return new WaitForSeconds(1 / weapon.FireRate);
            StopCoroutine(burst);
        }
    }

    private IEnumerator FireBurst()
    {
        for (int i = 0; i < weapon.BurstAmount; i++)
        {
            var offset = transform.right * weapon.BulletOffset.x + Vector3.up * weapon.BulletOffset.y + transform.forward * weapon.BulletOffset.z;
            var bulletObject = Instantiate(weapon.BulletPrefab, transform.position, transform.rotation);
            bulletObject.TryGetComponent(out Bullet bullet);
            bullet.Source = gameObject;
            bullet.TargetCategory = targetCategory;
            yield return new WaitForSeconds(1 / weapon.BurstFireRate);
        }
    }
}
