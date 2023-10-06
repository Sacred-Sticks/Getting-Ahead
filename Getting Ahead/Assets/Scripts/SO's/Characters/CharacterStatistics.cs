using UnityEngine;

[CreateAssetMenu(fileName = "Character Stats", menuName = "Getting Ahead/Character Stats")]
public class CharacterStatistics : ScriptableObject
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRate;
    [SerializeField] private float attackDamage;
    [SerializeField] private float health;
    [SerializeField] private float defense;
    [SerializeField] private float iframes;

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }
    public float AttackRate
    {
        get
        {
            return attackRate;
        }
    }
    public float AttackDamage
    {
        get
        {
            return attackDamage;
        }
    }
    public float Health
    {
        get
        {
            return health;
        }
    }
    public float Defense
    {
        get
        {
            return defense;
        }
    }
    public float Iframes
    {
        get
        {
            return iframes;
        }
    }
}
