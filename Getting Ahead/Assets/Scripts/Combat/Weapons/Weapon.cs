using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject
{
    [SerializeField] private float attackRate;
    [Space]
    [SerializeField] private int burstAmount;
    [SerializeField] private float burstFireRate;

    public float AttackRate
    {
        get
        {
            return attackRate;
        }
    }
    public float BurstAmount
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
}
