using UnityEngine;

[CreateAssetMenu(fileName = "Melee Weapon", menuName = "Getting Ahead/Weapons/Melee Weapon")]
public class MeleeWeapon : Weapon
{
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackAngle;
    
    public float AttackDamage
    {
        get
        {
            return attackDamage;
        }
    }
    public float AttackRadius
    {
        get
        {
            return attackRadius;
        }
    }
    public float AttackAngle
    {
        get
        {
            return attackAngle;
        }
    }
}
