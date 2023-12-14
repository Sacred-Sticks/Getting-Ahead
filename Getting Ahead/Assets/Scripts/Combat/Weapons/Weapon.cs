using UnityEngine;

public class Weapon : ScriptableObject
{
    [SerializeField] private float attackRate;
    [Space]
    [SerializeField] private int burstAmount;
    [SerializeField] private float burstFireRate;

    public float AttackRate => attackRate;
    public float BurstAmount => burstAmount;
    public float BurstFireRate => burstFireRate;
}
