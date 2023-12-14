using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Getting Ahead/Weapons/Ranged Weapon")]
public class RangedWeapon : Weapon
{
    [Space]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Vector3 bulletOffset;
    [SerializeField] private float attackRange;
    
    public GameObject BulletPrefab => bulletPrefab;
    public Vector3 BulletOffset => bulletOffset;
    public float AttackRange => attackRange;
}
