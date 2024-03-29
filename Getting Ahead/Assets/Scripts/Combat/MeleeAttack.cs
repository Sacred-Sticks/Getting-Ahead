using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kickstarter.Categorization;
using Kickstarter.Events;
using UnityEngine;

public class MeleeAttack : Attack
{
    [SerializeField] private CategoryType enemyCategory;
    [SerializeField] private Service onAudioTrigger;

    protected override IEnumerator FireBurst()
    {
        if (weapon is not MeleeWeapon meleeWeapon)
        {
            Debug.LogWarning("Weapon Type is Invalid");
            yield break;
        }
        for (int i = 0; i < weapon.BurstAmount; i++)
        {
            var enemies = CollectEnemyHealth(Physics.OverlapSphere(transform.position, meleeWeapon.AttackRadius), meleeWeapon);
            foreach (var enemyHealth in enemies.Where(enemyHealth => enemyHealth.gameObject != gameObject))
                enemyHealth.TakeDamage(meleeWeapon.AttackDamage, gameObject);
            NotifyObservers(true);
            NotifyObservers(PlayerActions.Shooting);
            NotifyObservers(PlayerActions.STOP);
            yield return new WaitForSeconds(1 / meleeWeapon.BurstFireRate);
        }
    }

    private List<Health> CollectEnemyHealth(Collider[] colliders, MeleeWeapon meleeWeapon)
    {
        var enemies = new List<Health>();
        var transforms = colliders.Select(c => c.transform);
        var visibleTransforms = ReduceTransformsByAngle(transforms, meleeWeapon);
        var categoriesComponents = visibleTransforms.Where(t => t.GetComponent<ObjectCategories>()).Select(c => c.GetComponent<ObjectCategories>());
        enemies.AddRange(categoriesComponents.Where(c => c.Categories.Contains(enemyCategory)).Select(c => c.GetComponent<Health>()));
        return enemies;
    }

    private IEnumerable<Transform> ReduceTransformsByAngle(IEnumerable<Transform> transforms, MeleeWeapon meleeWeapon)
    {
        var visibleTargets = new List<Transform>();
        var forward = Vector3.forward;

        foreach (var target in transforms) {
            var direction = target.transform.position - transform.position;
            direction = transform.InverseTransformDirection(direction).normalized;
            float angle = Vector3.Angle(forward, direction);

            if (angle <= meleeWeapon.AttackAngle && angle >= -meleeWeapon.AttackAngle) {
                visibleTargets.Add(target);
            }
        }

        visibleTargets.Remove(transform);
        return visibleTargets;
    }
}
