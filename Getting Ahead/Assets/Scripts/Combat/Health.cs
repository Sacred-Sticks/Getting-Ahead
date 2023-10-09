using System;
using Kickstarter.Events;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int intialHealth;
    [Header("Outgoing Services")]
    [SerializeField] private Service onDeath;

    private float currentHealth;
    public float CurrentHealth
    {
        private get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            if (currentHealth <= 0)
                onDeath.Trigger(new OnDeathArgs(gameObject));
        }
    }

    public class OnDeathArgs : EventArgs
    {
        public OnDeathArgs(GameObject dyingPlayerGameObject)
        {
            DyingPlayerGameObject = dyingPlayerGameObject;
        }
        
        public GameObject DyingPlayerGameObject { get; }
    }
}
