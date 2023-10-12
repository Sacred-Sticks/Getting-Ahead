using UnityEngine;

[CreateAssetMenu(fileName = "Stat Multipliers", menuName = "Getting Ahead/Character Stat Multipliers")]
public class HeadStatistics : ScriptableObject
{
    [SerializeField] private float moveSpeedMultiplier;
    [SerializeField] private float attackRateMultiplier;
    [SerializeField] private float maxHealthMultiplier;
    [SerializeField] private float iframesMultiplier;

    public float MoveSpeedMultiplier
    {
        get
        {
            return moveSpeedMultiplier;
        }
    }
    public float AttackRateMultiplier
    {
        get
        {
            return attackRateMultiplier;
        }
    }
    public float MaxHealthMultiplier
    {
        get
        {
            return maxHealthMultiplier;
        }
    }
    public float IframesMultiplier
    {
        get
        {
            return iframesMultiplier;
        }
    }
}
