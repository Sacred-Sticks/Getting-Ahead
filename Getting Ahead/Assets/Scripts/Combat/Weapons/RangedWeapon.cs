using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Getting Ahead/Weapons/Ranged Weapon")]
public class RangedWeapon : ScriptableObject
{
    [SerializeField] private float fireRate;
    [Space]
    [SerializeField] private int burstAmount;
    [SerializeField] private float burstFireRate;
    [Space]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Vector3 bulletOffset;
    
    public float FireRate
    {
        get
        {
            return fireRate;
        }
    }
    public int BurstAmount
    {
        get
        {
            return burstAmount;
        }
    }
    public float BurstFireRate
    {
        get
        {
            return burstFireRate;
        }
    }
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
