using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Getting Ahead/Weapons/Ranged Weapon")]
public class RangedWeapon : Weapon
{
    [Space]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Vector3 bulletOffset;
    
    public GameObject BulletPrefab
    {
        get
        {
            return bulletPrefab;
        }
    }
    public Vector3 BulletOffset
    {
        get
        {
            return bulletOffset;
        }
    }
}
