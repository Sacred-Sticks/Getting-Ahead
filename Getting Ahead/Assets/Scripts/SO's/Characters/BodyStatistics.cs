using UnityEngine;

[CreateAssetMenu(fileName = "Body Stats", menuName = "Getting Ahead/Character Body Stats")]
public class BodyStatistics : ScriptableObject
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRate;
    [SerializeField] private float maxHealth;
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
    public float MaxHealth
    {
        get
        {
            return maxHealth;
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
